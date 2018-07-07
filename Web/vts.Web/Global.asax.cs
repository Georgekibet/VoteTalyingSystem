using System.Configuration;
using System.Reflection;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using vts.Core.Contracts;
using vts.Data.Context;
using vts.Data.Contracts;
using Vts.WebLib.DefaultContracts;

namespace vts.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        public static IContainer BaseContainer { get; private set; }
        private readonly string _serverConnectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        protected void Application_Start()
        {
            var builder = new ContainerBuilder();
            builder.RegisterApiControllers(typeof(MvcApplication).Assembly);
            builder.RegisterControllers(typeof(MvcApplication).Assembly);
            builder.RegisterModule(new AutofacWebTypesModule());
            BaseContainer = builder.Build();
            

            AreaRegistration.RegisterAllAreas();

            InitializeServices();
            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            
           
            DependencyResolver.SetResolver(new AutofacDependencyResolver(BaseContainer)); //Set the MVC DependencyResolver
            GlobalConfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver(BaseContainer); //Set the WebApi DependencyResolver
            GlobalConfiguration.Configuration.EnsureInitialized();
        }

        private void InitializeServices()
        {
            ContainerBuilder containerBuilder = new ContainerBuilder();
            foreach (var defaultService in new DefaultRepositoryServices().Services.ServiceList)
            {
                containerBuilder.RegisterType(defaultService.Implementation).As(defaultService.Contract);
            }

            foreach (var service in new DefaultImportServices().Services.ServiceList)
            {
                containerBuilder.RegisterType(service.Implementation).As(service.Contract);
            }

            foreach (var defaultService in new ViewModelBuilderDefaultContracts().Services.ServiceList)
            {
                containerBuilder.RegisterType(defaultService.Implementation).As(defaultService.Contract);
            }


            containerBuilder.RegisterType<ContextConnection>()
               .AsSelf()
               .WithParameter("vtsConnectionString", _serverConnectionString);
            containerBuilder.Update(BaseContainer);

            
        }
    }
}