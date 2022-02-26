﻿using AspNetCoreSharedComponent.ServiceDiscoveries;
using DatabaseAccessor.Contexts;
using DatabaseAccessor.Mapping;
using DatabaseAccessor.Repositories;
using DatabaseAccessor.Repositories.Abstraction;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace CheckoutService
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
            services.AddControllers();
            services.RegisterOcelotService(Configuration);
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
                {
                    options.Authority = "https://cap-k24-team13-auth.herokuapp.com";
                    options.Audience = "checkout";
                });
            services.AddMediatR(typeof(Startup));
            services.AddScoped<ApplicationDbContext>();
            services.AddScoped<IInvoiceRepository, InvoiceRepository>();
            services.AddSingleton(Mapper.GetInstance());
            services.AddSwaggerGen();
            services.AddCors(options =>
            {
                options.AddPolicy("Default", builder =>
                {
                    builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
                });
            });
            services.AddStackExchangeRedisCache(options =>
            {
                options.ConfigurationOptions.Password = Environment.GetEnvironmentVariable("REDIS_PASSWORD");
                options.ConfigurationOptions.ClientName = "localhost:4600";
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseCors("Default");
            app.UseRouting();

            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                        name: "default",
                        pattern: "{controller=Product}/{action=Index}/{id?}");
            });
        }
    }
}