using AspNetCoreSharedComponent.FileValidations;
using AspNetCoreSharedComponent.ServiceDiscoveries;
using DatabaseAccessor;
using DatabaseAccessor.Mapping;
using DatabaseAccessor.Repositories;
using DatabaseAccessor.Repositories.Interfaces;
using FluentValidation;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Shared.RequestModels;
using ShopProductService.Validations;
using System;

namespace ShopProductService
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
            services.AddControllers(options =>
            {
                options.ModelBinderProviders.Add(new IntToBoolModelBinderProvider());
            }).AddFluentValidation();
            services.RegisterOcelotService(Configuration);
            services.AddMediatR(typeof(Startup));
            services.AddScoped<ApplicationDbContext>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IFileStorable, FileStore>();
            services.AddTransient<IValidator<CreateOrEditCategoryRequestModel>, AddOrEditCategoryRequestModelValidator>();
            services.AddTransient<IValidator<CreateOrEditProductRequestModel>, AddOrEditProductRequestModelValidator>();
            services.AddTransient<IValidator<SearchProductRequestModel>, SearchProductRequestModelValidator>();
            services.AddSingleton(Mapper.GetInstance());
            services.AddSwaggerGen();
            services.AddCors(options =>
            {
                options.AddPolicy("Default", builder =>
                {
                    builder.WithOrigins("https://localhost:44349").AllowAnyMethod().AllowAnyHeader();
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
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseCors("Default");
            app.UseRouting();
            
            app.UseSwagger();
            app.UseSwaggerUI();

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