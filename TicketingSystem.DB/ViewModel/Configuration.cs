using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketingSystem.DB.ViewModel
{
    public class Configuration
    {
        public int ID { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
        public bool? IsActive { get; set; }
    }
}
