using Microsoft.Exchange.WebServices.Data;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.ServiceModel.Web;
using System.Web.Script.Serialization;

namespace TicketingSystem.EmailNotifier
{
    class Program
    {

        static ExchangeService service = new ExchangeService(ExchangeVersion.Exchange2010_SP2);
        static string SupportMailBoxEmailId = ConfigurationManager.AppSettings["SupportMailBoxEmailId"];
        static string SupportMailBoxEmailPassword = ConfigurationManager.AppSettings["SupportMailBoxEmailPassword"];
        static string NewTicketCreationAPIUrl = ConfigurationManager.AppSettings["NewTicketCreationAPIUrl"];
        private static AutoResetEvent Signal;

        static void Main(string[] args)
        {
            service.Credentials = new WebCredentials(SupportMailBoxEmailId, SupportMailBoxEmailPassword);
            service.AutodiscoverUrl(SupportMailBoxEmailId, RedirectionUrlValidationCallback);
            CheckForUnreadEmailsAndUpsertTicket(service);
            SetStreamingNotifications(service);
            Signal = new AutoResetEvent(false);
            Signal.WaitOne();
        }
        static void CheckForUnreadEmailsAndUpsertTicket(ExchangeService service,int nextPageOffset=0 )
        {
            PropertySet propSet = new PropertySet(BasePropertySet.FirstClassProperties, ItemSchema.Subject, ItemSchema.Body, ItemSchema.IsResend);

            Folder inbox = Folder.Bind(service, WellKnownFolderName.Inbox,propSet);
            Console.WriteLine("---------Connection Established-------");

            SearchFilter searchFilter = new SearchFilter.SearchFilterCollection(LogicalOperator.And, new SearchFilter.IsEqualTo(EmailMessageSchema.IsRead, false));
            ItemView view = new ItemView(100,nextPageOffset);

            FindItemsResults<Item> findResults = service.FindItems(WellKnownFolderName.Inbox, searchFilter,view);

            
            
            Console.WriteLine("--------- Total Unread Emails Found {0} -------", findResults.TotalCount);

            foreach (var res in findResults)
            {

                EmailMessage email = Item.Bind(service, res.Id, propSet) as EmailMessage;
                
                Model.Email _email = ConvertExchangeEmailObject(email);
                GetPOSTResponse(new Uri(NewTicketCreationAPIUrl), _email, email);
            }
            if (findResults.MoreAvailable)
            {
                CheckForUnreadEmailsAndUpsertTicket(service,findResults.NextPageOffset.Value);
            }
        }
        private static bool RedirectionUrlValidationCallback(string redirectionUrl)
        {
            bool result = false;

            Uri redirectionUri = new Uri(redirectionUrl);
            if (redirectionUri.Scheme == "https")
            {
                result = true;
            }
            return result;
        }
        static void SetStreamingNotifications(ExchangeService service)
        {
            try
            {
                StreamingSubscription streamingsubscription = service.SubscribeToStreamingNotifications(
                new FolderId[] { WellKnownFolderName.Inbox },
                EventType.NewMail);



                StreamingSubscriptionConnection connection = new StreamingSubscriptionConnection(service, 5);

                connection.AddSubscription(streamingsubscription);
                connection.OnNotificationEvent += new StreamingSubscriptionConnection.NotificationEventDelegate(OnEvent);
                connection.OnSubscriptionError += new StreamingSubscriptionConnection.SubscriptionErrorDelegate(OnError);
                connection.OnDisconnect += new StreamingSubscriptionConnection.SubscriptionErrorDelegate(OnDisconnect);

                connection.Open();

                Console.WriteLine("--------- Subscribed to {0} event -------",EventType.NewMail);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        static private void OnDisconnect(object sender, SubscriptionErrorEventArgs args)
        {
            Console.WriteLine("----------Connection Reopening--------");
            StreamingSubscriptionConnection connection = (StreamingSubscriptionConnection)sender;
            connection.Open();
            Console.WriteLine("----------Connection Reopened--------");
        }

        static void OnEvent(object sender, NotificationEventArgs args)
        {
            StreamingSubscription subscription = args.Subscription;

            foreach (NotificationEvent notification in args.Events.Where(x => x.EventType == EventType.NewMail))
            {
                if (notification is ItemEvent)
                {
                    Console.WriteLine("----------Email Received--------");
                    ItemEvent itemEvent = (ItemEvent)notification;
                    PropertySet propSet = new PropertySet(BasePropertySet.FirstClassProperties, ItemSchema.Subject, ItemSchema.Body, ItemSchema.IsResend);
                    EmailMessage email = Item.Bind(service, itemEvent.ItemId.UniqueId, propSet) as EmailMessage;
                    Model.Email _email = ConvertExchangeEmailObject(email);
                    GetPOSTResponse(new Uri(NewTicketCreationAPIUrl), _email,email);
                }
            }
        }
        static Model.Email ConvertExchangeEmailObject(EmailMessage email)
        {
            Model.Email _email = new Model.Email();
            _email.Body = email.Body;
            _email.DateTimeCreated = email.DateTimeCreated;
            _email.DateTimeReceived = email.DateTimeReceived;
            _email.DateTimeSent = email.DateTimeSent;
            _email.From = new Model.User()
            {
                Email = email.From.Address,
                IsActive = true,
                Name = email.From.Name,
                IsExternalUser = !email.From.Address.Contains(SupportMailBoxEmailId.Split('@')[1])
            };
            _email.Subject = email.Subject;
            _email.ID = 0;
            foreach (var attachment in email.Attachments)
            {
                Model.Attachment _attachment = new Model.Attachment();
                _attachment.FileName = attachment.Name;
                _attachment.FileUrl = "/";
                _attachment.UploadedBy = _email.From;
                _email.Attachments.Add(_attachment);
            }
            foreach (var to in email.ToRecipients)
            {
                _email.To.Add(new Model.User()
                {
                    Name = to.Name,
                    Email = to.Address,
                    IsActive = true,
                    IsExternalUser = !to.Address.Contains(SupportMailBoxEmailId.Split('@')[1])
                });
            }
            foreach (var cc in email.CcRecipients)
            {
                _email.CC.Add(new Model.User()
                {
                    Name = cc.Name,
                    Email = cc.Address,
                    IsActive = true,
                    IsExternalUser = !cc.Address.Contains(SupportMailBoxEmailId.Split('@')[1])
                });
            }
            return _email;
        }
        static void OnError(object sender, SubscriptionErrorEventArgs args)
        {
            Exception e = args.Exception;
            Console.WriteLine("\n-------------Error ---" + e.Message + "-------------");
        }
        private static void GetPOSTResponse(Uri uri, Model.Email _email, EmailMessage email = null)
        {
            
            JavaScriptSerializer js = new JavaScriptSerializer();
            string jsonData = js.Serialize(_email);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            request.Method = "POST";
            request.ContentType = "application/json";
            request.ContentLength = jsonData.Length;
            using (Stream webStream = request.GetRequestStream())
            using (StreamWriter requestWriter = new StreamWriter(webStream, System.Text.Encoding.ASCII))
            {
                requestWriter.Write(jsonData);
            }
            var watch = System.Diagnostics.Stopwatch.StartNew();
            try
            {
                WebResponse webResponse = request.GetResponse();
                using (Stream webStream = webResponse.GetResponseStream())
                {
                    if (webStream != null)
                    {
                        using (StreamReader responseReader = new StreamReader(webStream))
                        {
                            Console.WriteLine("Took {0} second(s) to create the ticket", (float)watch.ElapsedMilliseconds / 1000);
                            string response = responseReader.ReadToEnd();
                            Model.Email e = js.Deserialize<Model.Email>(response);
                            Console.Out.WriteLine("----------Ticket created with ID:{0}--------", e.TicketID);
                            Console.Out.WriteLine("----------Sending Email response--------");
                            ResponseMessage responseMessage = email.CreateReply(true);
                            responseMessage.Subject = e.Subject;
                            responseMessage.BodyPrefix = e.Body;
                            responseMessage.SendAndSaveCopy();
                            Console.Out.WriteLine("----------Sucessfully sent Email response--------");
                            watch.Stop();
                            email.IsRead = true;
                            email.Update(ConflictResolutionMode.AlwaysOverwrite);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.Out.WriteLine("-------Error----------");
                Console.Out.WriteLine(e.Message);
                watch.Stop();
            }
        }
    }
}
