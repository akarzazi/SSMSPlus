using Microsoft.Extensions.DependencyInjection;

using SSMSPlusDb.DbUpdate;

namespace SSMSPlusDb
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddSSMSPlusDbServices(this IServiceCollection services)
        {
            services.AddSingleton<DbUpdater>();

            return services;
        }
    }
}
