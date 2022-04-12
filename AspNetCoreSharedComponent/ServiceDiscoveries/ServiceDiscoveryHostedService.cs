using Consul;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Shared;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace AspNetCoreSharedComponent.ServiceDiscoveries
{
    public class ServiceDiscoveryHostedService : IHostedService
    {
        private readonly IConsulClient _consulClient;

        private readonly ServiceConfiguration _serviceConfiguration;

        private readonly ILogger<ServiceDiscoveryHostedService> _logger;

        private readonly string _serviceRegistrationId;

        private readonly string _healthCheckExecutionPath;

        public ServiceDiscoveryHostedService(IConsulClient consulClient, IConfiguration configuration,
            ILogger<ServiceDiscoveryHostedService> logger, string healthCheckExecutionPath)
        {
            _consulClient = consulClient;
            _logger = logger;
            _serviceConfiguration = configuration.GetServiceConfiguration();
            _serviceRegistrationId = $"{_serviceConfiguration.ServiceName}-{_serviceConfiguration.ServiceId}";
            _healthCheckExecutionPath = healthCheckExecutionPath;
        }

        public ServiceDiscoveryHostedService(IConsulClient consulClient, IConfiguration configuration,
            ILogger<ServiceDiscoveryHostedService> logger) :
            this(consulClient, configuration, logger, string.Empty)
        { }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var registration = new AgentServiceRegistration
            {
                ID = _serviceRegistrationId,
                Name = _serviceConfiguration.ServiceName,
                Address = _serviceConfiguration.ServiceAddress.Host,
                Port = _serviceConfiguration.ServiceAddress.Port
            };
            if (!string.IsNullOrWhiteSpace(_healthCheckExecutionPath) && File.Exists(_healthCheckExecutionPath))
            {
                registration.Check = new()
                {
                    Name = $"{_serviceRegistrationId} health check",
                    Notes = $"Health check for {_serviceRegistrationId}",
                    Args = new string[] 
                    { 
                        "/usr/bin/dotnet",
                        _healthCheckExecutionPath,
                        $"{_serviceConfiguration.ServiceAddress}health",
                        _serviceConfiguration.ServiceName
                    },
                    Interval = TimeSpan.FromMinutes(10),
                    Timeout = TimeSpan.FromSeconds(10),
                    Status = HealthStatus.Passing
                };
            }
            try
            {
                await _consulClient.Agent.ServiceDeregister(_serviceRegistrationId, cancellationToken);
                await _consulClient.Agent.ServiceRegister(registration, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to register service {0}, inner exception is: {1}", registration.Name, ex.Message);
            }
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await _consulClient.Agent.ServiceDeregister(_serviceRegistrationId, cancellationToken);
        }
    }
}
