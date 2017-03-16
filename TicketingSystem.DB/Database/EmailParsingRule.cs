namespace TicketingSystem.DB.Database
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("EmailParsingRule")]
    public partial class EmailParsingRule
    {
        public int ID { get; set; }

        [Required]
        public string ParseRule { get; set; }
        public string ParseProperty { get; set; }

        [StringLength(50)]
        public string ParseType { get; set; }
        public string ExceptionParseProperty { get; set; }

        public string ExceptionType { get; set; }

        [StringLength(50)]
        public string ExceptionToRule { get; set; }

        public bool? IsActive { get; set; }
    }
}
