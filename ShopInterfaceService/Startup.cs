using DatabaseAccessor;
using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ShopInterfaceService.RequestModel;
using ShopInterfaceService.Service;
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
            services.AddControllers();
            services.AddSwaggerGen();
            services.AddScoped<ApplicationDbContext, ApplicationDbContext>();
            services.AddTransient<IShopService, ShopService>();
            services.AddTransient<IValidator<AddOrEditShopInterfaceRequestModel>, AddOrEditShopInterfaceRequestModelValidator>();
            services.AddCors(options =>
            {
                options.AddPolicy("Default", builder =>
                {
                    builder.WithOrigins("https://localhost:44349").AllowAnyHeader().AllowAnyMethod();
                });
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
                        name: "Default",
                        pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
