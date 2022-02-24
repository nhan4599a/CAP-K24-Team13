using GUI.Abtractions;
using GUI.Attributes;
using GUI.Clients;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using Refit;
using System;
using System.Net.Http;
using System.Reflection;

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
            IdentityModelEventSource.ShowPII = true;
            services.AddControllersWithViews();
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
            }).AddCookie(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, options =>
            {
                options.Authority = "https://cap-k24-team13-auth.herokuapp.com";
                options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.ClientId = "oidc-client";
                options.ClientSecret = "CapK24Team13";
                options.ResponseType = "code";
                options.UsePkce = true;
                options.ResponseMode = "query";
                options.Scope.Add("product.api");
                options.Scope.Add("interface.api");
                options.Scope.Add("checkout.api");
                options.Scope.Add("order.api");
                options.Scope.Add("rating.api");
                options.Scope.Add("offline_access");
                options.Scope.Add("roles");
                options.GetClaimsFromUserInfoEndpoint = true;
                options.SaveTokens = true;
                options.ClaimActions.MapJsonKey("role", "role", "role");
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    NameClaimType = "name",
                    RoleClaimType = "role"
                };
            });
            services.Configure<RazorViewEngineOptions>(options =>
            {
                var virtualAreaName = typeof(BaseUserController)
                    .GetCustomAttribute<VirtualAreaAttribute>(false)
                    .Name;
                options.ViewLocationFormats.Add($"/Areas/{virtualAreaName}/Views/{{1}}/{{0}}{RazorViewEngine.ViewExtension}");
            });
            services.AddScoped<BaseActionFilter>();
            services.AddRefitClient<IProductClient>()
                .ConfigureHttpClient(ConfigureHttpClient);
            services.AddRefitClient<IShopClient>()
                .ConfigureHttpClient(ConfigureHttpClient);
            services.AddRefitClient<ICategoryClient>()
                .ConfigureHttpClient(ConfigureHttpClient);
            services.AddRefitClient<ICartClient>()
                .ConfigureHttpClient(ConfigureHttpClient);
            services.AddRefitClient<IOrderClient>()
                .ConfigureHttpClient(ConfigureHttpClient);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedHost | ForwardedHeaders.XForwardedProto
            });
            app.Use(async (context, next) =>
            {
                context.Request.Scheme = "https";
                await next();
            });
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.Use(async (context, next) =>
            {
                await next(context);

                var responseCode = context.Response.StatusCode;
                if (responseCode != 200)
                {
                    context.Response.Redirect($"/Error/{responseCode}");
                }
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapAreaControllerRoute(
                        name: "ShopOwner",
                        areaName: "Admin",
                        pattern: "ShopOwner/{controller=Product}/{action=Index}/{id?}");
                
                endpoints.MapControllerRoute(
                        name: "User",
                        pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        private void ConfigureHttpClient(HttpClient client)
        {
            client.BaseAddress = new Uri("http://ec2-52-207-214-39.compute-1.amazonaws.com:3000");
        }
    }
}