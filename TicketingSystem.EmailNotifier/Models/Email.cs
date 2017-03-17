using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketingSystem.EmailNotifier.Model
{
    public class Email
    {
        public int ID { get; set; }
        public User From { get; set; }
        public List<User> CC { get; set; }
        public List<User> To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public int? TicketID { get; set; }
        public int? PreviousEmail { get; set; }
        public List<Attachment> Attachments { get; set; }
        public DateTime DateTimeCreated { get; set; }
        public DateTime DateTimeReceived { get; set; }
        public DateTime DateTimeSent { get; set; }

        public Email()
        {
            this.Attachments = new List<Attachment>();
            this.To = new List<User>();
            this.CC = new List<User>();
        }
    }
}
