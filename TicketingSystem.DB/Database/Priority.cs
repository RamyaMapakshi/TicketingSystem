namespace TicketingSystem.DB.Database
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Priority")]
    public partial class Priority
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Priority()
        {
            Tickets = new HashSet<Ticket>();
        }

        public int ID { get; set; }

        [Required]
        [StringLength(50)]
        public string Title { get; set; }

        public string Description { get; set; }

        public int? DaysDue { get; set; }

        [StringLength(50)]
        public string Color { get; set; }

        public bool? IsDefault { get; set; }
        public bool? IsActive { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Ticket> Tickets { get; set; }
    }
}
