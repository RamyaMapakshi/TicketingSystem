namespace TicketingSystem.DB.Database
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Comment")]
    public partial class Comment
    {
        public int ID { get; set; }

        public string Details { get; set; }

        public bool? IsPrivate { get; set; }

        public DateTime Created { get; set; }

        public DateTime Modified { get; set; }

        public int CreatedBy { get; set; }

        public int? AttachmentId { get; set; }

        public int ModifiedBy { get; set; }

        public int Ticket { get; set; }

        public virtual Ticket _Ticket { get; set; }

        public virtual User _CreatedBy { get; set; }

        public virtual User _ModifiedBy { get; set; }
    }
}
