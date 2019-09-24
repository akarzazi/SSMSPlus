using EnvDTE80;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.Shell;
using SSMSPlus.DbUpdate;
using SSMSPlus.Services;
using SSMSPlusCore;
using SSMSPlusCore.App;
using SSMSPlusCore.Di;
using SSMSPlusCore.Infra.Logging;
using SSMSPlusCore.Integration;
using SSMSPlusCore.Settings;
using SSMSPlusCore.Utils;
using SSMSPlusDocument;
using SSMSPlusHistory;
using SSMSPlusPreferences;
using SSMSPlusSearch;
using System;
using System.ComponentModel.Design;
using System.IO;
using System.Reflection;
using System.Windows;
using Task = System.Threading.Tasks.Task;

namespace SSMSPlus
{
    /// <summary>
    /// Command handler
    /// </summary>
    internal sealed class Command1
    {
        private static DTE2 _dte;
        private static AsyncPackage _asyncPackage;
        private static OleMenuCommandService _commandService;

        private Microsoft.Extensions.DependencyInjection.ServiceProvider _serviceProvider;
        private static IConfigurationRoot _configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="Command1"/> class.
        /// Adds our command handlers for menu (commands must exist in the command table file)
        /// </summary>
        /// <param name="package">Owner package, not null.</param>
        /// <param name="commandService">Command service to add command to, not null.</param>
        private Command1(AsyncPackage package)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            try
            {
                _configuration = new ConfigurationBuilder()
                                .SetBasePath(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location))
                                .AddXmlFile(@"_settings.config", optional: false)
                                .Build();

                // create service collection
                var services = new ServiceCollection();
                ConfigureServices(services);

                _serviceProvider = services.BuildServiceProvider();
                ServiceLocator.SetLocatorProvider(_serviceProvider);

                _serviceProvider.GetRequiredService<DbUpdate.DbUpdater>().UpdateDb();
                _serviceProvider.GetRequiredService<HistoryPlugin>().Register();
                _serviceProvider.GetRequiredService<SearchPlugin>().Register();
                _serviceProvider.GetRequiredService<DocumentPlugin>().Register();
                _serviceProvider.GetRequiredService<PreferencesUI>().Register();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.GetFullStackTraceWithMessage(), "Could not Load SSMSPlus");
                _serviceProvider.GetRequiredService<ILogger<Command1>>().LogCritical(ex, "Critical Error when starting pluging");

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
                    Folder = Path.Combine(new SSMSWorkingDirProvider().GetWorkingDir(), "log"),
                    LogLevel = LogLevel.Information,
                    RetainPolicyFileCount = 30
                });
            });

            services.AddSingleton<PackageProvider>((_) => new PackageProvider(_dte, _asyncPackage, _commandService));

            // add services:
            AddInternalServices(services);
        }

        public static IServiceCollection AddInternalServices(IServiceCollection services)
        {
            services.ConfigureSection<DistributionSettings>(_configuration.GetSection("Distribution"));

            services.AddSingleton<IWorkingDirProvider, SSMSWorkingDirProvider>();
            services.AddSingleton<IVersionProvider, VersionProvider>();
            services.AddSingleton<DbUpdater>();
            services.AddSSMSPlusCoreServices();
            services.AddSSMSPlusHistoryServices();
            services.AddSSMSPlusSearchServices();
            services.AddSSMSPlusDocumentServices();
            services.AddSSMSPlusPreferencesServices();
            return services;
        }

        /// <summary>
        /// Gets the instance of the command.
        /// </summary>
        public static Command1 Instance
        {
            get;
            private set;
        }

        /// <summary>
        /// Initializes the singleton instance of the command.
        /// </summary>
        /// <param name="package">Owner package, not null.</param>
        public static async Task InitializeAsync(AsyncPackage package, DTE2 dte)
        {
            // Switch to the main thread - the call to AddCommand in Command1's constructor requires
            // the UI thread.
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync(package.DisposalToken);

            _dte = dte;
            _asyncPackage = package;
            _commandService = await package.GetServiceAsync(typeof(IMenuCommandService)) as OleMenuCommandService;

            Instance = new Command1(package);
        }
    }
}
