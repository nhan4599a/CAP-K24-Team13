using AuthServer.Services;
using DatabaseAccessor;
using DatabaseAccessor.Contexts;
using DatabaseAccessor.Identities;
using DatabaseAccessor.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
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
            var password = Configuration.GetValue<string>("CLIENT_AUTH_PASSWORD");
            var clientConnectionString = string.Format(Configuration.GetConnectionString("ClientAuthentication"), password);
            services.AddControllersWithViews();
            services.AddScoped<UserStore<User, Role, ApplicationDbContext, Guid>, ApplicationUserStore>();
            services.AddScoped<UserManager<User>, ApplicationUserManager>();
            services.AddScoped<RoleManager<Role>, ApplicationRoleManager>();
            services.AddScoped<SignInManager<User>, ApplicationSignInManager>();
            services.AddScoped<RoleStore<Role, ApplicationDbContext, Guid>, ApplicationRoleStore>();
            services.AddDbContext<ApplicationDbContext>();
            services.AddDbContext<ClientAuthenticationDbContext>(options =>
            {
                options.UseSqlServer(clientConnectionString).UseOpenIddict();
            });
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
                {
                    options.LoginPath = "/auth/signin";
                    options.LogoutPath = "/auth/signout";
                });
            services.AddIdentity<User, Role>(options =>
            {
                options.SignIn.RequireConfirmedEmail = true;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 8;
                options.Tokens.ProviderMap.Add("MailConfirmation",
                    new TokenProviderDescriptor(typeof(MailConfirmationTokenProvider)));
                options.Tokens.EmailConfirmationTokenProvider = "MailConfirmation";
            }).AddUserStore<ApplicationUserStore>()
            .AddUserManager<ApplicationUserManager>()
            .AddRoleStore<ApplicationRoleStore>()
            .AddRoleManager<ApplicationRoleManager>()
            .AddSignInManager<ApplicationSignInManager>();

            services.AddOpenIddict()
                .AddCore(options =>
                {
                    options.UseEntityFrameworkCore().UseDbContext<ClientAuthenticationDbContext>();
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
