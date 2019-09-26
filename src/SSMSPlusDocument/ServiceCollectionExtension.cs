namespace SSMSPlusDocument
{
    using Microsoft.Extensions.DependencyInjection;
    using SSMSPlusDocument.UI;

    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddSSMSPlusDocumentServices(this IServiceCollection services)
        {
            services.AddSingleton<DocumentUi>();
            services.AddSingleton<DocumentPlugin>();
            
            services.AddTransient<ExportDocumentsControlVM>();
            return services;
        }
    }
}
