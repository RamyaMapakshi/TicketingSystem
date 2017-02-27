using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketingSystem.DB.Database;

namespace TicketingSystem.DB.DBManagers
{
    public class TagDBManager
    {
        public List<ViewModel.Tag> GetAllTags()
        {
            List<ViewModel.Tag> tags = new List<ViewModel.Tag>();
            using (TicketingSystemDBContext context = new TicketingSystemDBContext())
            {
                foreach (var tag in context.Tags)
                {
                    tags.Add(ConvertToViewModelObject(tag));
                }
            }
            return tags;
        }
        public int CreateTag(ViewModel.Tag tag)
        {
            using (TicketingSystemDBContext context = new TicketingSystemDBContext())
            {
                Database.Tags t = new Database.Tags()
                {
                    ID = tag.ID,
                    TagName = tag.TagName,
                    Details = tag.Details
                };
                context.Tags.Add(t);
                context.SaveChanges();
                return t.ID;
            }
        }
        private ViewModel.Tag ConvertToViewModelObject(Database.Tags tag)
        {
            return new ViewModel.Tag()
            {
                ID = tag.ID,
                TagName = tag.TagName,
                Details = tag.Details
            };
        }
    }
}
