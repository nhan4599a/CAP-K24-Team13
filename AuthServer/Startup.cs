using AspNetCoreSharedComponent.Middleware;
using AspNetCoreSharedComponent.ModelBinders.Providers;
using AspNetCoreSharedComponent.ModelValidations;
using AuthServer.Configurations;
using AuthServer.Factories;
using AuthServer.Identities;
using AuthServer.Models;
using AuthServer.Providers;
using AuthServer.Services;
using AuthServer.Validators;
using DatabaseAccessor.Contexts;
using DatabaseAccessor.Models;
using DatabaseAccessor.Repositories;
using DatabaseAccessor.Repositories.Abstraction;
using IdentityServer4.Extensions;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Models;
using System;
using System.Net;
using System.Net.Mail;
using System.Reflection;
using static IdentityServer4.IdentityServerConstants;

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
            services.AddControllersWithViews(options =>
            {
                options.ModelBinderProviders.Add(new StringToDateOnlyModelBinderProvider());
            }).AddFluentValidation<UserSignUpModel, SignUpModelValidator>()
            .AddFluentValidation<EditUserInformationModel, EditUserInformationModelValidator>();
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = Configuration["REDIS_CONNECTION_STRING"];
            });
            services.AddDataProtection()
                .PersistKeysToDbContext<ApplicationDbContext>();
            services.AddSession();
            services.AddScoped<UserStore<User, Role, ApplicationDbContext, Guid>, ApplicationUserStore>();
            services.AddScoped<UserManager<User>, ApplicationUserManager>();
            services.AddScoped<RoleManager<Role>, ApplicationRoleManager>();
            services.AddScoped<SignInManager<User>, ApplicationSignInManager>();
            services.AddScoped<RoleStore<Role, ApplicationDbContext, Guid>, ApplicationRoleStore>();
            services.AddScoped<IReportRepository, ReportRepository>();
            services.AddDbContext<ApplicationDbContext>();
            services.AddAuthentication()
                .AddCookie(options =>
                {
                    options.LoginPath = "/auth/signin";
                    options.LogoutPath = "/auth/signout";
                });
            services.AddAuthorization(options =>
            {
                options.AddPolicy(LocalApi.PolicyName, policy =>
                {
                    policy.AddAuthenticationSchemes(LocalApi.AuthenticationScheme);
                    policy.RequireAuthenticatedUser();
                    policy.RequireRole(Roles.ADMIN);
                });
            });
            services.AddTransient<MailConfirmationTokenProvider<User>>();
            services.AddTransient<ResetPasswordTokenProvider<User>>();
            services.AddSingleton(new SmtpClient
            {
                Credentials = new NetworkCredential(Configuration["GMAIL_USERNAME"], Configuration["GMAIL_PASSWORD"]),
                DeliveryMethod = SmtpDeliveryMethod.Network,
                EnableSsl = true,
                Host = "smtp.gmail.com",
                Port = 587
            });
            services.AddSingleton<IMailService, GmailService>();
            services.AddScoped<IUserClaimsPrincipalFactory<User>, ApplicationUserClaimsPrincipleFactory>();
            services.AddIdentity<User, Role>(options =>
            {
                options.SignIn.RequireConfirmedEmail = AccountConfig.RequireEmailConfirmation;
                options.SignIn.RequireConfirmedAccount = AccountConfig.RequireEmailConfirmation;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequiredLength = 8;
                if (AccountConfig.RequireEmailConfirmation)
                {
                    options.Tokens.ProviderMap.Add("MailConfirmation",
                        new TokenProviderDescriptor(typeof(MailConfirmationTokenProvider<User>)));
                    options.Tokens.EmailConfirmationTokenProvider = "MailConfirmation";
                }
                options.Tokens.ProviderMap.Add("ResetPassword",
                    new TokenProviderDescriptor(typeof(ResetPasswordTokenProvider<User>)));
                options.Tokens.PasswordResetTokenProvider = "ResetPassword";
                options.Lockout.MaxFailedAccessAttempts = AccountConfig.MaxFailedAccessAttempts;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(AccountConfig.LockOutTime);
            }).AddUserStore<ApplicationUserStore>()
            .AddUserManager<ApplicationUserManager>()
            .AddRoleStore<ApplicationRoleStore>()
            .AddRoleManager<ApplicationRoleManager>()
            .AddSignInManager<ApplicationSignInManager>()
            .AddPasswordValidator<UserPasswordValidator>()
            .AddEntityFrameworkStores<ApplicationDbContext>();

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
                options.Authentication.CookieLifetime = TimeSpan.FromHours(2);
                options.Authentication.CookieSlidingExpiration = false;
                options.Authentication.RequireAuthenticatedUserForSignOutMessage = true;
            })
                .AddAspNetIdentity<User>()
                .AddOperationalStore(options =>
                {
                    options.ConfigureDbContext = ApplyOptions;
                    options.TokenCleanupInterval = 7200;
                })
                .AddConfigurationStore(options =>
                {
                    options.ConfigureDbContext = ApplyOptions;
                })
                .AddDeveloperSigningCredential();
            services.AddLocalApiAuthentication();
            services.AddHostedService<InitializeClientAuthenticationService>();
            services.AddHostedService<InitializeAccountChallengeService>();
            services.ConfigureApplicationCookie(options =>
            {
                options.ExpireTimeSpan = TimeSpan.FromHours(2);
                options.SlidingExpiration = false;
            });
            services.AddMediatR(typeof(Startup));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedHost | ForwardedHeaders.XForwardedProto
            });
            app.Use(async (context, next) =>
            {
                context.SetIdentityServerOrigin("https://cap-k24-team13-auth.herokuapp.com/");
                await next();
            });
            app.UseHsts();

            app.UseIdentityServer();

            app.UseStaticFiles();
            app.UseRouting();

            app.UseGlogalExceptionHandlerMiddleware();

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseSession();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }

        private void ApplyOptions(DbContextOptionsBuilder builder)
        {
            var connectionString = Configuration["CLIENT_AUTH_CONNECTION_STRING"];
            var assemblyName = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;
            builder.UseSqlServer(connectionString, sqlOptions => sqlOptions.MigrationsAssembly(assemblyName));
        }
    }
}
