using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketingSystem.DB.ViewModel
{
    public class SubCategory
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool? IsDefault { get; set; }
        public bool? IsActive { get; set; }
    }
}
