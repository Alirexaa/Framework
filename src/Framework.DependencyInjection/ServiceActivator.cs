using Framework.Abstraction.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace Framework.DependencyInjection
{
    public static class ServiceActivator
    {
        private static IServiceProvider _serviceProvider;
        private static IServiceScope _scope;
        public static void Configure(IServiceProvider serviceProvider)
        {
            ArgumentNullException.ThrowIfNull(serviceProvider, nameof(serviceProvider));
            _serviceProvider = serviceProvider;
        }

        public static IServiceScope CreateScope()
        {
            _scope = _serviceProvider.CreateScope();
            return _scope;
        }

        public static T Get<T>()
        {
            CreateScope();

            return _scope.ServiceProvider.GetService<T>();
        }
    }
}