using Consul;
using Shared;

namespace AspNetCoreSharedComponent
{
    public class ServiceDiscoveryHostedService : IHostedService
    {
        private readonly IConsulClient _consulClient;

        private readonly ServiceConfiguration _serviceConfiguration;

        private readonly string _serviceRegistrationId;

        public ServiceDiscoveryHostedService(IConsulClient consulClient, IConfiguration configuration)
        {
            _consulClient = consulClient;
            _serviceConfiguration = configuration.GetServiceConfiguration();
            _serviceRegistrationId = $"{_serviceConfiguration.ServiceName}-{_serviceConfiguration.ServiceId}";
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var registration = new AgentServiceRegistration
            {
                ID = _serviceRegistrationId,
                Name = _serviceConfiguration.ServiceName,
                Address = _serviceConfiguration.ServiceAddress.Host,
                Port = _serviceConfiguration.ServiceAddress.Port
            };
            await _consulClient.Agent.ServiceDeregister(_serviceRegistrationId, cancellationToken);
            await _consulClient.Agent.ServiceRegister(registration, cancellationToken);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await _consulClient.Agent.ServiceDeregister(_serviceRegistrationId, cancellationToken);
        }
    }
}
