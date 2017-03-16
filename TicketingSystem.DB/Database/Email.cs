using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketingSystem.DB.Database
{
    [Table("Email")]
    public partial class Email
    {
        public int ID { get; set; }
        public string From { get; set; }
        public List<string> CC { get; set; }
        public List<string> To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public int? TicketID { get; set; }
        public int? PreviousEmail { get; set; }
        public int? AttachmentId { get; set; }
        public DateTime DateTimeCreated { get; set; }
        public DateTime DateTimeReceived { get; set; }
        public DateTime DateTimeSent { get; set; }
    }
}
