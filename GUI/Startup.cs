using GUI.Attributes;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Refit;
using System;
using System.Linq;
using System.Reflection;
using System.Security.Claims;

namespace GUI
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
            services.AddControllersWithViews();
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
            }).AddCookie(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, options =>
            {
                options.Authority = "https://localhost:7265";
                options.ClientId = "oidc-client";
                options.ClientSecret = "CapK24Team13";
                options.ResponseType = "code";
                options.UsePkce = true;
                options.ResponseMode = "query";
                options.Scope.Add("product.read");
                options.Scope.Add("product.write");
                options.SaveTokens = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    NameClaimType = ClaimTypes.Email
                };
            });
            services.AddControllersWithViews();
            services.Configure<RazorViewEngineOptions>(options =>
            {
                var virtualAreaNames = Assembly.GetExecutingAssembly().GetTypes()
                    .Where(type => typeof(Controller).IsAssignableFrom(type)
                        && type.GetCustomAttribute<VirtualAreaAttribute>() != null)
                    .Select(type => type.GetCustomAttribute<VirtualAreaAttribute>(false)!.Name)
                    .Distinct();
                foreach (var virtualArea in virtualAreaNames)
                    options.ViewLocationFormats.Add($"/Areas/{virtualArea}/Views/{{1}}/{{0}}{RazorViewEngine.ViewExtension}");
            });
            services.AddRefitClient<IProductClient>().ConfigureHttpClient(options =>
            {
                options.BaseAddress = new Uri("https://localhost:7157");
            });
            services.AddRefitClient<IShopClient>().ConfigureHttpClient(options =>
            {
                options.BaseAddress = new Uri("https://localhost:7157");
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
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapAreaControllerRoute(
                        name: "Admin",
                        areaName: "Admin",
                        pattern: "Admin/{controller=Product}/{action=Index}/{id?}");
                
                endpoints.MapControllerRoute(
                        name: "User",
                        pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}