[assembly: WebActivator.PostApplicationStartMethod(typeof(TicketingSystem.Service.App_Start.SimpleInjectorWebApiInitializer), "Initialize")]

namespace TicketingSystem.Service.App_Start
{
    using System.Web.Http;
    using SimpleInjector;
    using SimpleInjector.Integration.WebApi;
    using DB.IDBManagers;
    using DB.DBManagers;
    using DB;

    public static class SimpleInjectorWebApiInitializer
    {
        /// <summary>Initialize the container and register it as Web API Dependency Resolver.</summary>
        public static void Initialize()
        {
            var container = new Container();
            container.Options.DefaultScopedLifestyle = new WebApiRequestLifestyle();
            
            InitializeContainer(container);

            container.RegisterWebApiControllers(GlobalConfiguration.Configuration);
       
            container.Verify();
            
            GlobalConfiguration.Configuration.DependencyResolver =
                new SimpleInjectorWebApiDependencyResolver(container);
        }
     
        private static void InitializeContainer(Container container)
        {
            container.Register<IAttachmentManager, AttachmentDBManager>(Lifestyle.Scoped);
            container.Register<ICategoryManager, CategoryDBManager>(Lifestyle.Scoped);
            container.Register<ICommentManager, CommentDBManager>(Lifestyle.Scoped);
            container.Register<IHistoryManager, HistoryDBManager>(Lifestyle.Scoped);
            container.Register<IPriorityManager, PriorityDBManager>(Lifestyle.Scoped);
            container.Register<IStatusManager, StatusDBManager>(Lifestyle.Scoped);
            container.Register<ITicketManager, TicketDBManager>(Lifestyle.Scoped);
            container.Register<ITicketTypeManager, TicketTypeDBManager>(Lifestyle.Scoped);
            container.Register<IUserManager, UserManager>(Lifestyle.Scoped);
            container.Register<IDBManager, DBManager>(Lifestyle.Scoped);
        }
    }
}