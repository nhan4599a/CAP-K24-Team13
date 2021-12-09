using DatabaseAccessor;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using FluentValidation.AspNetCore;
using FluentValidation;
using ShopProductService.RequestModel;
using ShopProductService.Validation;
using MediatR;
using Shared.RequestModels;

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
            services.AddControllers().AddFluentValidation();
            services.AddScoped<ApplicationDbContext, ApplicationDbContext>();
            services.AddTransient<IValidator<AddOrEditCategoryRequestModel>, AddOrEditCategoryRequestModelValidator>();
            services.AddTransient<IValidator<AddOrEditProductRequestModel>, AddOrEditProductRequestModelValidator>();
            services.AddTransient<IValidator<SearchProductRequestModel>, SearchProductRequestModelValidator>();
            services.AddSwaggerGen();
            services.AddMediatR(typeof(Startup));
            services.AddCors(options =>
            {
                options.AddPolicy("Default", builder =>
                {
                    builder.WithOrigins("https://localhost:44349").AllowAnyMethod().AllowAnyHeader();
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
                        name: "default",
                        pattern: "{controller=Product}/{action=Index}/{id?}");
            });
        }
    }
}