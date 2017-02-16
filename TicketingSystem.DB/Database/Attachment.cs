namespace TicketingSystem.DB.Database
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Attachment")]
    public partial class Attachment
    {
        public int ID { get; set; }

        [Required]
        [StringLength(50)]
        public string FileName { get; set; }

        [Required]
        public string FileUrl { get; set; }

        public int UploadedBy { get; set; }

        public int Ticket { get; set; }

        public virtual Ticket _Ticket { get; set; }

        public virtual User _UploadedBy { get; set; }
    }
}
