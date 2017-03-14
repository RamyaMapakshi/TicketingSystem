namespace TicketingSystem.DB.Database
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Tag
    {
        public int ID { get; set; }

        [Required]
        [StringLength(50)]
        public string TagName { get; set; }

        public string Details { get; set; }
    }
}
