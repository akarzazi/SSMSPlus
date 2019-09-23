
namespace SSMSPlusHistory
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.Extensions.DependencyInjection;
    using SSMSPlusHistory.Repositories;
    using SSMSPlusHistory.Services;
    using SSMSPlusHistory.UI.VM;

    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddSSMSPlusHistoryServices(this IServiceCollection services)
        {
            services.AddSingleton<QueryTracker>();    
            services.AddSingleton<QueryItemRepository>();
            services.AddSingleton<HistoryPlugin>();

            services.AddSingleton<HistoryUi>();
            services.AddSingleton<HistoryControlVM>();
          
            return services;
        }
    }
}
