namespace TicketingSystem.DB.Database
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TicketType")]
    public partial class TicketType
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TicketType()
        {
            Tickets = new HashSet<Ticket>();
        }

        public int ID { get; set; }

        [Required]
        [StringLength(50)]
        public string Title { get; set; }

        public string Description { get; set; }
        public bool? IsDefault { get; set; }

        public bool? IsActive { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Ticket> Tickets { get; set; }
    }
}
