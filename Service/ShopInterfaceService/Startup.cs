using AspNetCoreSharedComponent.FileValidations;
using AspNetCoreSharedComponent.ModelValidations;
using AspNetCoreSharedComponent.ServiceDiscoveries;
using DatabaseAccessor.Contexts;
using DatabaseAccessor.Mapping;
using DatabaseAccessor.Repositories;
using DatabaseAccessor.Repositories.Interfaces;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Shared.RequestModels;
using ShopInterfaceService.Validation;

namespace ShopInterfaceService
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
            services.RegisterOcelotService(Configuration);
            services.AddControllers()
                .AddFluentValidation<CreateOrEditInterfaceRequestModel, CreateOrEditInterfaceRequestModelValidator>();
            services.AddSwaggerGen();
            services.AddScoped<ApplicationDbContext>();
            services.AddScoped<IFileStorable, FileStore>();
            services.AddScoped<IShopInterfaceRepository, ShopInterfaceRepository>();
            services.AddSingleton(Mapper.GetInstance());
            services.AddCors(options =>
            {
                options.AddPolicy("Default", builder =>
                {
                    builder.WithOrigins("https://localhost:44349").AllowAnyHeader().AllowAnyMethod();
                });
            });
            services.AddMediatR(typeof(Startup));
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
                        name: "Default",
                        pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
