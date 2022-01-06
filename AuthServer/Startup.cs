using AuthServer.Services;
using DatabaseAccessor;
using DatabaseAccessor.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AuthServer
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
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseOpenIddict();
            });
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
                {
                    options.LoginPath = "/auth/signin";
                    options.LogoutPath = "/auth/signout";
                });
            services.AddIdentity<User, IdentityRole>(options =>
            {
                options.SignIn.RequireConfirmedEmail = true;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 8;
                options.Tokens.ProviderMap.Add("MailConfirmation",
                    new TokenProviderDescriptor(typeof(MailConfirmationTokenProvider)));
                options.Tokens.EmailConfirmationTokenProvider = "MailConfirmation";
            }).AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddOpenIddict()
                .AddCore(options =>
                {
                    options.UseEntityFrameworkCore().UseDbContext<ApplicationDbContext>();
                })
                .AddServer(options =>
                {
                    options.AllowClientCredentialsFlow();

                    options.AllowAuthorizationCodeFlow();

                    options.SetTokenEndpointUris("/connect/token");

                    options.SetAuthorizationEndpointUris("/connect/authorize");

                    options.AddEphemeralEncryptionKey().AddEphemeralSigningKey();

                    options.RegisterScopes("api");

                    options.UseAspNetCore().EnableTokenEndpointPassthrough();
                });

            services.AddHostedService<SetupDefaultClientService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
