﻿namespace Demo
{
    using Demo.Services;
    using Microsoft.Extensions.DependencyInjection;
    using SSMSPlusCore;
    using SSMSPlusCore.App;
    using SSMSPlusCore.Integration;
    using SSMSPlusCore.Integration.ObjectExplorer;
    using SSMSPlusCore.Settings;
    using SSMSPlusDb;
    using SSMSPlusDocument;
    using SSMSPlusHistory;
    using SSMSPlusPreferences;
    using SSMSPlusSearch;
    using SSMSPlusSearch.Services;

    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddInternalServices(this IServiceCollection services)
        {
            services.AddSSMSPlusDbServices();
            services.AddSSMSPlusCoreServices();
            services.AddSSMSPlusHistoryServices();
            services.AddSSMSPlusSearchServices();
            services.AddSSMSPlusDocumentServices();
            services.AddSSMSPlusPreferencesServices();

            services.AddSingleton<IVersionProvider, DemoVersionProvider>();
            services.AddSingleton<IWorkingDirProvider, DemoWorkingDirProvider>();
            services.AddSingleton<IObjectExplorerInteraction, DemoObjectExplorerInteraction>();
            services.AddSingleton<IServiceCacheIntegration, DemoServiceCacheIntegration>();
            
            // Comment out this line to use the real indexer
            services.AddSingleton<IDbIndexer, DemoDbIndexer>(); 

            services.AddSingleton<DistributionSettings>((_) => new DistributionSettings() { ContributeText = "Contribute", ContributeUrl = "Http://Demo" });
            return services;
        }
    }
}
