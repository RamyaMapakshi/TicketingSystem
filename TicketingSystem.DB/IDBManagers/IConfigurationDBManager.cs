using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketingSystem.DB.IDBManagers
{
    public interface IConfigurationDBManager
    {
        List<ViewModel.Configuration> GetAllConfigurations();
        ViewModel.Configuration GetConfigurationByKey(string key);
        bool UpdateConfiguration(ViewModel.Configuration config);
    }
}
