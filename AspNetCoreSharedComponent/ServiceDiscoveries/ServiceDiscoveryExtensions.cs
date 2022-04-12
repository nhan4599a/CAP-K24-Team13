using Ocelot.DependencyInjection;
using Ocelot.Provider.Consul;
using Ocelot.Middleware;
using Consul;
using Shared;
using Microsoft.Extensions.DependencyInjection;
using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace AspNetCoreSharedComponent.ServiceDiscoveries
{
    public static class ServiceDiscoveryExtensions
    {
        public static IServiceCollection AddOcelotConsul(this IServiceCollection services, IConfiguration configuration)
        {
            if (configuration == null)
                throw new ArgumentNullException(nameof(configuration));
            services.AddOcelot(configuration).AddConsul();
            return services;
        }

        public static IApplicationBuilder UseOcelot(this IApplicationBuilder app)
        {
            OcelotMiddlewareExtensions.UseOcelot(app).Wait();
            return app;
        }

        public static IServiceCollection RegisterOcelotService(this IServiceCollection services, IConfiguration configuration,
            string healtchCheckExecutionPath)
        {
            var serviceConfiguration = configuration.GetServiceConfiguration();
            services.AddSingleton<IHostedService, ServiceDiscoveryHostedService>(sv => 
                new ServiceDiscoveryHostedService(sv.GetRequiredService<IConsulClient>(), sv.GetRequiredService<IConfiguration>(),
                    sv.GetRequiredService<ILogger<ServiceDiscoveryHostedService>>(), healtchCheckExecutionPath));
            services.AddSingleton<IConsulClient>(CreateConsulClient(serviceConfiguration));
            return services;
        }

        private static ConsulClient CreateConsulClient(ServiceConfiguration serviceConfiguration)
        {
            return new ConsulClient(config =>
            {
                config.Address = serviceConfiguration.ServiceDiscoveryAddress;
            });
        }
    }
}