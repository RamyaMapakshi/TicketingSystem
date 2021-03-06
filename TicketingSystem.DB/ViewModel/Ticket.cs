﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketingSystem.DB.ViewModel
{
    public class Ticket
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public User RequestedBy { get; set; }
        public User RequestedFor { get; set; }

        public Status Status { get; set; }

        public Category Category { get; set; }
        public Impact Impact { get; set; }
        public SubCategory SubCategory { get; set; }
        public string Description { get; set; }
        public string Tags { get; set; }

        public Priority Priority { get; set; }

        public DateTime? DueDate { get; set; }

        public TicketType Type { get; set; }

        public string EmailsToNotify { get; set; }
        public string Notes { get; set; }
        public User AssignedTechnician { get; set; }

        public bool? IsEscalated { get; set; }

        public DateTime? ExpectedCompletionDate { get; set; }

        public User ClosedBy { get; set; }

        public User ResolvedBy { get; set; }

        public DateTime? ClosedDate { get; set; }

        public DateTime? ResolvedDate { get; set; }

        public bool? IsTicketGeneratedViaEmail { get; set; }

        public bool? IsActive { get; set; }

        public DateTime Created { get; set; }

        public DateTime Modified { get; set; }

        public User CreatedBy { get; set; }

        public User ModifiedBy { get; set; }

        public bool? IsDuplicate { get; set; }

        public int? DuplicateTicketID { get; set; }

        public List<Attachment> Attachments { get; set; }
        public List<Comment> Comments { get; set; }
        public List<History> Histories { get; set; }
        public Ticket()
        {
            this.Attachments = new List<Attachment>();
            this.Comments = new List<Comment>();
            this.Histories = new List<History>();
        }
    }
}
