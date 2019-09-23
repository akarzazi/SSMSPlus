namespace SSMSPlusCore.Di
{
    using System;
    using Microsoft.Extensions.DependencyInjection;

    public class ServiceLocator
    {
        private static IServiceProvider _serviceProvider;

        public static void SetLocatorProvider(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public static T GetRequiredService<T>()
        {
            return _serviceProvider.GetRequiredService<T>();
        }

        public static IServiceProvider GeTServiceProvider()
        {
            return _serviceProvider;
        }
    }
}
