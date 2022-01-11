using AspNetCoreSharedComponent.ModelBinders.Providers;
using AspNetCoreSharedComponent.ModelValidations;
using AuthServer.Configurations;
using AuthServer.Identities;
using AuthServer.Models;
using AuthServer.Providers;
using AuthServer.Services;
using AuthServer.Validators;
using DatabaseAccessor.Contexts;
using DatabaseAccessor.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Net.Mail;
using System.Reflection;
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
            services.AddControllersWithViews(options =>
            {
                options.ModelBinderProviders.Add(new StringToDateOnlyModelBinderProvider());
            }).AddFluentValidation<SignUpModel, SignUpModelValidator>();
            services.AddScoped<UserStore<User, Role, ApplicationDbContext, Guid>, ApplicationUserStore>();
            services.AddScoped<UserManager<User>, ApplicationUserManager>();
            services.AddScoped<RoleManager<Role>, ApplicationRoleManager>();
            services.AddScoped<SignInManager<User>, ApplicationSignInManager>();
            services.AddScoped<RoleStore<Role, ApplicationDbContext, Guid>, ApplicationRoleStore>();
            services.AddDbContext<ApplicationDbContext>();
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
                {
                    options.LoginPath = "/auth/signin";
                    options.LogoutPath = "/auth/signout";
                });
            services.AddTransient<MailConfirmationTokenProvider<User>>();
            services.AddScoped<SmtpClient>();
            services.AddScoped<IMailService, GmailService>();
            services.AddIdentity<User, Role>(options =>
            {
                options.SignIn.RequireConfirmedEmail = AccountConfig.RequireEmailConfirmation;
                options.SignIn.RequireConfirmedAccount = AccountConfig.RequireEmailConfirmation;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 8;
                if (AccountConfig.RequireEmailConfirmation)
                {
                    options.Tokens.ProviderMap.Add("MailConfirmation",
                        new TokenProviderDescriptor(typeof(MailConfirmationTokenProvider<User>)));
                    options.Tokens.EmailConfirmationTokenProvider = "MailConfirmation";
                }
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10);
            }).AddUserStore<ApplicationUserStore>()
            .AddUserManager<ApplicationUserManager>()
            .AddRoleStore<ApplicationRoleStore>()
            .AddRoleManager<ApplicationRoleManager>()
            .AddSignInManager<ApplicationSignInManager>()
            .AddPasswordValidator<UserPasswordValidator>();

            services.AddIdentityServer(options =>
            {
                options.Events.RaiseSuccessEvents = true;
                options.Events.RaiseFailureEvents = true;
                options.Events.RaiseErrorEvents = true;
                options.Events.RaiseInformationEvents = true;
                options.UserInteraction.LoginUrl = "/auth/signin";
                options.UserInteraction.LogoutUrl = "/auth/signout";
                options.Endpoints.EnableAuthorizeEndpoint = true;
                options.Endpoints.EnableTokenEndpoint = true;
                options.Endpoints.EnableIntrospectionEndpoint = true;
            })
                .AddAspNetIdentity<User>()
                .AddOperationalStore(options =>
                {
                    options.ConfigureDbContext = ApplyOptions;
                })
                .AddConfigurationStore(options =>
                {
                    options.ConfigureDbContext = ApplyOptions;
                })
                .AddSigningCredential(
                    new X509Certificate2(certFilePath, "nhan4599")
                );
            services.AddHostedService<InitializeAuthenticationService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseIdentityServer();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }

        private void ApplyOptions(DbContextOptionsBuilder builder)
        {
            var connectionString =
                "Server=.,4599;Database=ClientAuth;User ID=sa;Password=nhan4599@Nhan;TrustServerCertificate=True";
            var assemblyName = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;
            builder.UseSqlServer(connectionString, sqlOptions => sqlOptions.MigrationsAssembly(assemblyName));
        }
    }
}
