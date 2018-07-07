using Autofac;
using vts.Data.Context;
using vts.Data.Contracts;
using vts.Shared.Services;

namespace Vts.Core.Tests
{
    public class IocHelper
    {
        public static IContainer DefaultContainerAutofac()
        {
            DbHelper connectionString = DefaultDbHelper.GetDefaultDbTestingHelper();
            var result = InitializeAutofacContainer(connectionString.VtsConnectionstring);
            IContainer r = result.Build();
            return r;
        }

        public static ContainerBuilder InitializeAutofacContainer(string vtssqlconnectionstring)
        {
            ContainerBuilder containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterType<ContextConnection>()
                .AsSelf()
                .WithParameter("vtsConnectionString", vtssqlconnectionstring);

            foreach (var defaultService in new UnifiedDefaultServices().Services.ServiceList)
            {
                containerBuilder.RegisterType(defaultService.Implementation).As(defaultService.Contract);
            }
            foreach (var defaultService in new DefaultRepositoryServices().Services.ServiceList)
            {
                containerBuilder.RegisterType(defaultService.Implementation).As(defaultService.Contract);
            }
            return containerBuilder;
        }
    }
}