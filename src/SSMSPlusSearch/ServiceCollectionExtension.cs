using Microsoft.Extensions.DependencyInjection;
using SSMSPlusSearch.Repositories;
using SSMSPlusSearch.Services;
using SSMSPlusSearch.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSMSPlusSearch
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddSSMSPlusSearchServices(this IServiceCollection services)
        {
            services.AddSingleton<SearchPlugin>();
            services.AddSingleton<SearchUi>();
            services.AddSingleton<IDbIndexer, DbIndexer>();
            services.AddSingleton<SchemaSearchRepository>();
            services.AddTransient<SchemaSearchControlVM>();
            return services;
        }
    }
}
