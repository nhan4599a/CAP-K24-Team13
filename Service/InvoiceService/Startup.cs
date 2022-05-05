using AspNetCoreSharedComponent.ModelValidations;
using AspNetCoreSharedComponent.ServiceDiscoveries;
using DatabaseAccessor.Contexts;
using DatabaseAccessor.Mapping;
using DatabaseAccessor.Repositories;
using DatabaseAccessor.Repositories.Abstraction;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using InvoiceService.Commands;
using InvoiceService.Validations;
using System;

namespace InvoiceService
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            IdentityModelEventSource.ShowPII = true;
            services.AddControllers()
                .AddFluentValidation<FindInvoiceQuery, FindInvoiceQueryValidator>();
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = Configuration["REDIS_CONNECTION_STRING"];
            });
            services.RegisterOcelotService(Configuration, Configuration["HEALTH_CHECK_EXECUTION_PATH"]);
            services.AddDbContext<ApplicationDbContext>();
            services.AddTransient<IInvoiceRepository, InvoiceRepository>();
            services.AddSingleton(Mapper.GetInstance());
            services.AddCors(options =>
            {
                options.AddPolicy("Default", builder =>
                {
                    builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
                });
            });
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
               .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
               {
                   options.Authority = "https://cap-k24-team13-auth.herokuapp.com";
                   options.TokenValidationParameters = new TokenValidationParameters
                   {
                       ValidateAudience = false,
                       RoleClaimType = "role"
                   };
               });
            services.AddMediatR(typeof(Startup));

            services.AddHealthChecks()
                .AddDbContextCheck<ApplicationDbContext>(customTestQuery: async (dbContext, cancellationToken) =>
                {
                    dbContext.Database.SetCommandTimeout(TimeSpan.FromSeconds(10));
                    return await dbContext.Database.CanConnectAsync(cancellationToken);
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            app.UseCors("Default");
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHealthChecks("/health", new HealthCheckOptions
                {
                    ResultStatusCodes =
                    {
                        [HealthStatus.Healthy] = StatusCodes.Status200OK,
                        [HealthStatus.Degraded] = StatusCodes.Status200OK,
                        [HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable
                    }
                });
                endpoints.MapControllers();
            });
        }
    }
}
