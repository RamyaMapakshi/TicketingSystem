namespace TicketingSystem.DB.Database
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Ticket")]
    public partial class Ticket
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Ticket()
        {
            Attachments = new HashSet<Attachment>();
            Comments = new HashSet<Comment>();
            Histories = new HashSet<History>();
        }

        public int ID { get; set; }

        public int RequestedBy { get; set; }

        public int RequestedFor { get; set; }

        public string Title { get; set; }

        public int TicketStatus { get; set; }

        public int? TicketCategory { get; set; }

        public int? TicketImpact { get; set; }

        public int? TicketSubCategory { get; set; }

        public string Description { get; set; }

        public string Tags { get; set; }

        public int? TicketPriority { get; set; }

        public string Notes { get; set; }

        public string EmailsToNotify { get; set; }

        public DateTime? DueDate { get; set; }

        public int? TicketType { get; set; }

        public int? AssignedTechnician { get; set; }

        public bool? IsEscalated { get; set; }

        public DateTime? ExpectedCompletionDate { get; set; }

        public int? ClosedBy { get; set; }

        public int? ResolvedBy { get; set; }

        public DateTime? ClosedDate { get; set; }

        public DateTime? ResolvedDate { get; set; }

        public bool? IsTicketGeneratedViaEmail { get; set; }

        public bool? IsActive { get; set; }

        public DateTime Created { get; set; }

        public DateTime Modified { get; set; }

        public int CreatedBy { get; set; }

        public int ModifiedBy { get; set; }

        public bool? IsDuplicate { get; set; }

        public int? DuplicateTicketID { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Attachment> Attachments { get; set; }

        public virtual Category Category { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Comment> Comments { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<History> Histories { get; set; }

        public virtual Impact Impact { get; set; }

        public virtual Priority Priority { get; set; }

        public virtual Status Status { get; set; }

        public virtual SubCategory SubCategory { get; set; }

        public virtual TicketType TicketType1 { get; set; }

        public virtual User _AssignedTechnician { get; set; }

        public virtual User _ClosedBy { get; set; }

        public virtual User _CreatedBy { get; set; }

        public virtual User _ModifiedBy { get; set; }

        public virtual User _RequestedBy { get; set; }

        public virtual User _RequestedFor { get; set; }

        public virtual User _ResolvedBy { get; set; }
    }
}
