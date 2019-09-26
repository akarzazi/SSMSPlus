using Microsoft.Extensions.DependencyInjection;
using SSMSPlusPreferences.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSMSPlusPreferences
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddSSMSPlusPreferencesServices(this IServiceCollection services)
        {
            services.AddSingleton<PreferencesUI>();
            services.AddSingleton<PreferencesWindowVM>();
            return services;
        }
    }
}
