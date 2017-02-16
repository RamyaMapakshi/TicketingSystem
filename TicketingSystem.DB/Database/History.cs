namespace TicketingSystem.DB.Database
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("History")]
    public partial class History
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public History()
        {
            History1 = new HashSet<History>();
        }

        public int ID { get; set; }

        [StringLength(50)]
        public string ActionType { get; set; }

        public string Action { get; set; }
        public string Status { get; set; }

        public int? ActionTakenBy { get; set; }

        public DateTime? ActionDateTime { get; set; }

        public int? TicketId { get; set; }

        public int? PreviousHistoryId { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<History> History1 { get; set; }

        public virtual History History2 { get; set; }

        public virtual Ticket Ticket { get; set; }

        public virtual User _ActionTakenBy { get; set; }
    }
}
