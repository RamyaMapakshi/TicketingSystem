using System;

namespace TicketingSystem.DB.ViewModel
{
    public class Comment 
    {
        public int ID { get; set; }

        public string Details { get; set; }

        public bool? IsPrivate { get; set; }

        public DateTime Created { get; set; }

        public DateTime Modified { get; set; }

        public User CreatedBy { get; set; }

        public User ModifiedBy { get; set; }
        public int TicketId { get; set; }
        public Attachment Attachment { get; set; }
    }
}
