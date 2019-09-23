using Demo.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SSMSPlusCore.Di;
using SSMSPlusCore.Infra.Logging;
using System;
using System.IO;
using System.Reflection;
using System.Windows;

namespace Demo
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static IConfigurationRoot _configuration;
        private ServiceProvider _serviceProvider;

        protected override void OnStartup(StartupEventArgs e)
        {
            try
            {
                _configuration = new ConfigurationBuilder()
                               .SetBasePath(Directory.GetCurrentDirectory())
                               .AddXmlFile(@"_settings.config", optional: false)
                               .Build();

                // create service collection
                var services = new ServiceCollection();
                ConfigureServices(services);

                _serviceProvider = services.BuildServiceProvider();
                ServiceLocator.SetLocatorProvider(_serviceProvider);
            }
            catch (Exception ex)
            {
                _serviceProvider.GetRequiredService<ILogger<App>>().LogCritical(ex, "Critical Error When starting pluging");
                throw;
            }
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            // configure logging
            services.AddLogging(builder =>
            {
                builder.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
                builder.AddFileLogger(() => new FileLoggerOptions
                {
                    Folder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "/log",
                    LogLevel = LogLevel.Information,
                    RetainPolicyFileCount = 30
                });
            });

            services.ConfigureSection<DemoDbSettings>(_configuration.GetSection("DemoDb"));

            // add services:
            services.AddInternalServices();
        }
    }
}
