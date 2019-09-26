using Microsoft.Extensions.DependencyInjection;
using SSMSPlusCore.App;
using SSMSPlusCore.Database;
using SSMSPlusCore.Di;
using SSMSPlusCore.Integration;
using SSMSPlusCore.Integration.Connection;
using SSMSPlusCore.Integration.ObjectExplorer;

namespace SSMSPlusCore
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddSSMSPlusCoreServices(this IServiceCollection services)
        {
            services.AddSingleton<SqliteConnectionFactory>();
            services.AddSingleton<Db>();
            services.AddSingleton<IObjectExplorerInteraction, ObjectExplorerInteraction>();
            services.AddSingleton<IServiceCacheIntegration, ServiceCacheIntegration>();
            services.AddSingleton<DbConnectionProvider>();

            return services;
        }
    }
}
