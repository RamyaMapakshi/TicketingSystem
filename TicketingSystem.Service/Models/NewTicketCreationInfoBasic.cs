using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketingSystem.DB.ViewModel;

namespace TicketingSystem.Service.Models
{
    public class NewTicketCreationInfoBasic
    {
        public User RequestedBy { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string EmailsToNotify { get; set; }
        public List<Attachment> Attachments { get; set; }
        public NewTicketCreationInfoBasic()
        {
            this.Attachments = new List<Attachment>();
        }
        public Ticket CreateTicketFromBasicTicketInfo()
        {
            return new Ticket()
            {
                ID = 0,
                Created = DateTime.Now,
                CreatedBy = this.RequestedBy,
                IsActive = true,
                IsTicketGeneratedViaEmail = false,
                Modified = DateTime.Now,
                ModifiedBy = this.RequestedBy,
                RequestedBy = this.RequestedBy,
                RequestedFor = this.RequestedBy,
                Attachments = this.Attachments,
                Title = this.Title,
                EmailsToNotify = this.EmailsToNotify,
                Description = this.Description
            };
        }
    }
}
