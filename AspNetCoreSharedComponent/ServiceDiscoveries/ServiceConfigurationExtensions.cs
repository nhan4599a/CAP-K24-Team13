using Shared;

namespace AspNetCoreSharedComponent.ServiceDiscoveries
{
    public static class ServiceConfigurationExtensions
    {
        public static ServiceConfiguration GetServiceConfiguration(this IConfiguration configuration)
        {
            if (configuration == null)
                throw new ArgumentNullException(nameof(configuration));
            var serviceConfigurationSection = configuration.GetSection("ServiceConfiguration");
            if (serviceConfigurationSection == null)
                throw new ArgumentException($"{nameof(configuration)} does not have section named \"ServiceConfiguration\"");
            var serviceConfiguration = new ServiceConfiguration();
            serviceConfigurationSection.Bind(serviceConfiguration);
            return serviceConfiguration;
        }
    }
}
