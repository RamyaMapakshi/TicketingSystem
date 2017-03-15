using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketingSystem.DB.Database;
using TicketingSystem.DB.IDBManagers;
using TicketingSystem.DB.ViewModel;

namespace TicketingSystem.DB.DBManagers
{
    public class ConfigurationDBManager : IConfigurationDBManager
    {

        public List<ViewModel.Configuration> GetAllConfigurations()
        {
            List<ViewModel.Configuration> configurations = new List<ViewModel.Configuration>();
            using (TicketingSystemDBContext context = new TicketingSystemDBContext())
            {
                foreach (var config in context.Configurations)
                {
                    configurations.Add(ConvertToViewModelObject(config));
                }
                return configurations;
            }
        }

        public ViewModel.Configuration GetConfigurationByKey(string key)
        {
            using (TicketingSystemDBContext context = new TicketingSystemDBContext())
            {
                return ConvertToViewModelObject(context.Configurations.FirstOrDefault(x => x.Key == key));
            }
        }

        public bool UpdateConfiguration(ViewModel.Configuration config)
        {
            if (config == null)
            {
                return false;
            }
            using (TicketingSystemDBContext context = new TicketingSystemDBContext())
            {
                context.Configurations.Add(new Database.Configuration()
                {
                    ID = config.ID,
                    IsActive = config.IsActive,
                    Key = config.Key,
                    Value = config.Value,
                });
                context.SaveChanges();
            }
            return true;
        }

        private ViewModel.Configuration ConvertToViewModelObject(Database.Configuration config)
        {
            if (config == null)
            {
                return null;
            }
            return new ViewModel.Configuration()
            {
                ID = config.ID,
                IsActive = config.IsActive,
                Key = config.Key,
                Value = config.Value,
            };
        }
    }
}
