﻿using AspNetCoreSharedComponent.HttpRequest;
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
using IdentityServer4.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Net;
using System.Net.Mail;
using System.Reflection;

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
            .AddFluentValidation<ExternalSignUpModel, ExternalSignUpModelValidator>()
            .AddFluentValidation<EditUserInformationModel, EditUserInformationModelValidator>();
            services.AddScoped<UserStore<User, Role, ApplicationDbContext, Guid>, ApplicationUserStore>();
            services.AddScoped<UserManager<User>, ApplicationUserManager>();
            services.AddScoped<RoleManager<Role>, ApplicationRoleManager>();
            services.AddScoped<SignInManager<User>, ApplicationSignInManager>();
            services.AddScoped<RoleStore<Role, ApplicationDbContext, Guid>, ApplicationRoleStore>();
            services.AddDbContext<ApplicationDbContext>();
            services.AddAuthentication()
                .AddCookie(options =>
                {
                    options.LoginPath = "/auth/signin";
                    options.LogoutPath = "/auth/signout";
                });
            services.AddTransient<MailConfirmationTokenProvider<User>>();
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
                options.Authentication.CookieLifetime = TimeSpan.FromHours(1);
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
                .AddDeveloperSigningCredential();
            services.AddHostedService<InitializeClientAuthenticationService>();
            services.AddHostedService<InitializeAccountChallengeService>();
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
                context.SetIdentityServerOrigin("https://cap-k24-team13-auth.herokuapp.com/");
                await next();
            });
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseIdentityServer();
            app.UseAuthorization();

            app.Use(async (context, next) =>
            {
                await next(context);

                var responseCode = context.Response.StatusCode;
                if (responseCode >= 400 && !context.Request.IsStatisFileRequest())
                {
                    context.Response.Redirect($"/Error/{responseCode}");
                }
            });

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
