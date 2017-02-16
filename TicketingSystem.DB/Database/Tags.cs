using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketingSystem.DB.Database
{
    [Table("Tags")]
    public partial class Tags
    {
        public int ID { get; set; }

        public string Details { get; set; }
    }
}
