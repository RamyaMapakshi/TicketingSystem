using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketingSystem.DB.Database
{
    [Table("EmailTemplate")]
    public partial class EmailTemplate
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string CC { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string BCC { get; set; }
        public bool IsActive { get; set; }
    }
}
