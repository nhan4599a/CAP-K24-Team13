using AuthServer.Configurations;
using AuthServer.Identities;
using AuthServer.Providers;
using DatabaseAccessor.Contexts;
using DatabaseAccessor.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.Security.Cryptography.X509Certificates;

namespace AuthServer
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }

        public IConfiguration Configuration { get; }

        public IWebHostEnvironment Environment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var certFilePath = Path.Combine(Environment.ContentRootPath, "auth-server.pfx");
            services.AddControllersWithViews();
            services.AddScoped<UserStore<User, Role, ApplicationDbContext, Guid>, ApplicationUserStore>();
            services.AddScoped<UserManager<User>, ApplicationUserManager>();
            services.AddScoped<RoleManager<Role>, ApplicationRoleManager>();
            services.AddScoped<SignInManager<User>, ApplicationSignInManager>();
            services.AddScoped<RoleStore<Role, ApplicationDbContext, Guid>, ApplicationRoleStore>();
            services.AddDbContext<ApplicationDbContext>();
            services.AddDbContext<ClientAuthenticationDbContext>();
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
                {
                    options.LoginPath = "/auth/signin";
                    options.LogoutPath = "/auth/signout";
                });
            services.AddTransient<MailConfirmationTokenProvider<User>>();
            services.AddIdentity<User, Role>(options =>
            {
                options.SignIn.RequireConfirmedEmail = true;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 8;
                options.Tokens.ProviderMap.Add("MailConfirmation",
                    new TokenProviderDescriptor(typeof(MailConfirmationTokenProvider<User>)));
                options.Tokens.EmailConfirmationTokenProvider = "MailConfirmation";
            }).AddUserStore<ApplicationUserStore>()
            .AddUserManager<ApplicationUserManager>()
            .AddRoleStore<ApplicationRoleStore>()
            .AddRoleManager<ApplicationRoleManager>()
            .AddSignInManager<ApplicationSignInManager>();

            services.AddIdentityServer()
                .AddInMemoryClients(ClientAuthConfig.Clients)
                .AddInMemoryIdentityResources(ClientAuthConfig.IdentityResources)
                .AddInMemoryApiResources(ClientAuthConfig.ApiResources)
                .AddInMemoryApiScopes(ClientAuthConfig.ApiScopes)
                .AddInMemoryPersistedGrants()
                .AddAspNetIdentity<User>()
                .AddSigningCredential(
                    new X509Certificate2(certFilePath, "nhan4599")
                );
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();
            app.UseStaticFiles();
            app.UseIdentityServer();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
