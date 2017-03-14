namespace TicketingSystem.DB.Database
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class TicketingSystemDBContext : DbContext
    {
        public TicketingSystemDBContext()
            : base("name=TicketingSystemDBContext")
        {
        }

        public virtual DbSet<C__MigrationHistory> C__MigrationHistory { get; set; }
        public virtual DbSet<Attachment> Attachments { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Comment> Comments { get; set; }
        public virtual DbSet<Configuration> Configurations { get; set; }
        public virtual DbSet<EmailParsingRule> EmailParsingRules { get; set; }
        public virtual DbSet<History> Histories { get; set; }
        public virtual DbSet<Impact> Impacts { get; set; }
        public virtual DbSet<Priority> Priorities { get; set; }
        public virtual DbSet<Status> Status { get; set; }
        public virtual DbSet<SubCategory> SubCategories { get; set; }
        public virtual DbSet<Tag> Tags { get; set; }
        public virtual DbSet<Ticket> Tickets { get; set; }
        public virtual DbSet<TicketType> TicketTypes { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>()
                .HasMany(e => e.Tickets)
                .WithOptional(e => e.Category)
                .HasForeignKey(e => e.TicketCategory);

            modelBuilder.Entity<History>()
                .HasMany(e => e.History1)
                .WithOptional(e => e.History2)
                .HasForeignKey(e => e.PreviousHistoryId);

            modelBuilder.Entity<Impact>()
                .HasMany(e => e.Tickets)
                .WithOptional(e => e.Impact)
                .HasForeignKey(e => e.TicketImpact);

            modelBuilder.Entity<Priority>()
                .HasMany(e => e.Tickets)
                .WithOptional(e => e.Priority)
                .HasForeignKey(e => e.TicketPriority);

            modelBuilder.Entity<Status>()
                .HasMany(e => e.Tickets)
                .WithRequired(e => e.Status)
                .HasForeignKey(e => e.TicketStatus)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<SubCategory>()
                .HasMany(e => e.Tickets)
                .WithOptional(e => e.SubCategory)
                .HasForeignKey(e => e.TicketSubCategory);

            modelBuilder.Entity<Ticket>()
                .HasMany(e => e.Attachments)
                .WithRequired(e => e.Ticket1)
                .HasForeignKey(e => e.Ticket)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Ticket>()
                .HasMany(e => e.Comments)
                .WithRequired(e => e._Ticket)
                .HasForeignKey(e => e.Ticket)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TicketType>()
                .HasMany(e => e.Tickets)
                .WithOptional(e => e.TicketType1)
                .HasForeignKey(e => e.TicketType);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Attachments)
                .WithRequired(e => e._UploadedBy)
                .HasForeignKey(e => e.UploadedBy)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Comments)
                .WithRequired(e => e._CreatedBy)
                .HasForeignKey(e => e.CreatedBy)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Comments1)
                .WithRequired(e => e._ModifiedBy)
                .HasForeignKey(e => e.ModifiedBy)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Histories)
                .WithOptional(e => e._ActionTakenBy)
                .HasForeignKey(e => e.ActionTakenBy);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Tickets)
                .WithOptional(e => e._AssignedTechnician)
                .HasForeignKey(e => e.AssignedTechnician);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Tickets1)
                .WithOptional(e => e._ClosedBy)
                .HasForeignKey(e => e.ClosedBy);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Tickets2)
                .WithRequired(e => e._CreatedBy)
                .HasForeignKey(e => e.CreatedBy)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Tickets3)
                .WithRequired(e => e._ModifiedBy)
                .HasForeignKey(e => e.ModifiedBy)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Tickets4)
                .WithRequired(e => e._RequestedBy)
                .HasForeignKey(e => e.RequestedBy)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Tickets5)
                .WithRequired(e => e._RequestedFor)
                .HasForeignKey(e => e.RequestedFor)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Tickets6)
                .WithOptional(e => e._ResolvedBy)
                .HasForeignKey(e => e.ResolvedBy);
        }
    }
}
