﻿using AspNetCoreSharedComponent.JSON;
using AspNetCoreSharedComponent.ServiceDiscoveries;
using DatabaseAccessor.Contexts;
using DatabaseAccessor.Mapping;
using DatabaseAccessor.Repositories;
using DatabaseAccessor.Repositories.Abstraction;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Shared.Models;

namespace StatisticService
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
            services.AddControllers()
                .AddJsonPropertyToStringSerializer<StatisticDateResult>();
            services.RegisterOcelotService(Configuration);
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
                {
                    options.Authority = "https://cap-k24-team13-auth.herokuapp.com";
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateAudience = false
                    };
                });
            services.AddMediatR(typeof(Startup));
            services.AddDbContext<ApplicationDbContext>();
            services.AddScoped<IInvoiceRepository, InvoiceRepository>();
            services.AddSingleton(LoggerFactory.Create(builder =>
            {
                builder.AddConsole();
            }));
            services.AddSingleton(Mapper.GetInstance());
            services.AddCors(options =>
            {
                options.AddPolicy("Default", builder =>
                {
                    builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            app.UseHsts();

            app.UseCors("Default");
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}