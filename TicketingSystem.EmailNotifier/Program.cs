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
        private static AutoResetEvent Signal;

        static void Main(string[] args)
        {
            service.Credentials = new WebCredentials(SupportMailBoxEmailId, SupportMailBoxEmailPassword);
            service.AutodiscoverUrl(SupportMailBoxEmailId, RedirectionUrlValidationCallback);
            SetStreamingNotifications(service);
            Signal = new AutoResetEvent(false);
            Signal.WaitOne();

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
            StreamingSubscription streamingsubscription = service.SubscribeToStreamingNotifications(
                new FolderId[] { WellKnownFolderName.Inbox },
                EventType.NewMail);

            StreamingSubscriptionConnection connection = new StreamingSubscriptionConnection(service, 5);

            connection.AddSubscription(streamingsubscription);
            connection.OnNotificationEvent += new StreamingSubscriptionConnection.NotificationEventDelegate(OnEvent);
            connection.OnSubscriptionError += new StreamingSubscriptionConnection.SubscriptionErrorDelegate(OnError);
            connection.OnDisconnect += new StreamingSubscriptionConnection.SubscriptionErrorDelegate(OnDisconnect);

            connection.Open();

            Console.WriteLine("--------- StreamSubscription event -------");
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
                    Model.Email _email =ConvertExchangeEmailObject(email) ;
                    GetPOSTResponse(new Uri("http://venus/api/Email/CreateTicketViaEmail/"),_email);
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
            _email.From = new Model.User() {
                Email = email.From.Address,
                IsActive = true,
                Name = email.From.Name,
                IsExternalUser = email.From.Address.Contains(SupportMailBoxEmailId.Split('@')[1])
            }; 
            _email.Subject = email.Subject;
            _email.ID = 0;
            foreach (var attachment in email.Attachments)
            {
                Model.Attachment _attachment = new Model.Attachment();
                _attachment.FileName = attachment.Name;
                _attachment.UploadedBy = _email.From;
                _email.Attachments.Add(_attachment);
            }
            foreach (var to in email.ToRecipients)
            {
                _email.To.Add(new Model.User() {
                    Name = to.Name,
                    Email = to.Address,
                    IsActive = true,
                    IsExternalUser = to.Address.Contains(SupportMailBoxEmailId.Split('@')[1])
                });
            }
            foreach (var cc in email.CcRecipients)
            {
                _email.CC.Add(new Model.User()
                {
                    Name = cc.Name,
                    Email = cc.Address,
                    IsActive = true,
                    IsExternalUser = cc.Address.Contains(SupportMailBoxEmailId.Split('@')[1])
                });
            }
            return _email;
        }
        static void OnError(object sender, SubscriptionErrorEventArgs args)
        {
            Exception e = args.Exception;
            Console.WriteLine("\n-------------Error ---" + e.Message + "-------------");
        }
        private static void GetPOSTResponse(Uri uri, Model.Email data)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            string jsonData=js.Serialize(data);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            request.Method = "POST";
            request.ContentType = "application/json";
            request.ContentLength = jsonData.Length;
            using (Stream webStream = request.GetRequestStream())
            using (StreamWriter requestWriter = new StreamWriter(webStream, System.Text.Encoding.ASCII))
            {
                requestWriter.Write(jsonData);
            }

            try
            {
                WebResponse webResponse = request.GetResponse();
                using (Stream webStream = webResponse.GetResponseStream())
                {
                    if (webStream != null)
                    {
                        using (StreamReader responseReader = new StreamReader(webStream))
                        {
                            string response = responseReader.ReadToEnd();
                            Console.Out.WriteLine("----------Ticket created with ID:{0}--------",response);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.Out.WriteLine("-----------------");
                Console.Out.WriteLine(e.Message);
            }
        }
    }
}
