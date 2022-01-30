using AspNetCoreSharedComponent.ServiceDiscoveries;

namespace ApiGateway
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            var ocelotConfiguration = new ConfigurationBuilder().AddJsonFile("ocelot.json").Build();
            services.AddOcelotConsul(ocelotConfiguration);
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseOcelot();
        }
    }
}
