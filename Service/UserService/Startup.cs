using AspNetCoreSharedComponent.Mail;
using AspNetCoreSharedComponent.ModelValidations;
using AspNetCoreSharedComponent.ServiceDiscoveries;
using DatabaseAccessor.Contexts;
using DatabaseAccessor.Mapping;
using DatabaseAccessor.Repositories;
using DatabaseAccessor.Repositories.Abstraction;
using Hangfire;
using Hangfire.SqlServer;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Net;
using System.Net.Mail;
using UserService.Commands;
using UserService.Validations;

namespace UserService
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
            services.AddControllers()
                .AddFluentValidation<GetAllUsersQuery, GetAllUsersQueryValidator>();
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = Configuration["REDIS_CONNECTION_STRING"];
            });
            services.RegisterOcelotService(Configuration);
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
                {
                    options.Authority = "https://cap-k24-team13-auth.herokuapp.com";
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateAudience = false
                    };
                });
            services.AddMediatR(typeof(Startup));
            services.AddDbContext<ApplicationDbContext>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddSingleton(Mapper.GetInstance());
            services.AddCors(options =>
            {
                options.AddPolicy("Default", builder =>
                {
                    builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
                });
            });
            services.AddSingleton(new SmtpClient
            {
                Credentials = new NetworkCredential(Configuration["GMAIL_USERNAME"], Configuration["GMAIL_PASSWORD"]),
                DeliveryMethod = SmtpDeliveryMethod.Network,
                EnableSsl = true,
                Host = "smtp.gmail.com",
                Port = 587
            });
            services.AddSingleton<IMailService, GmailService>();
            services.AddHangfire(config => config
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseSqlServerStorage(Configuration["HANGFIRE_CONNECTION_STRING"], new SqlServerStorageOptions
                {
                    CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                    SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                    QueuePollInterval = TimeSpan.Zero,
                    UseRecommendedIsolationLevel = true,
                    DisableGlobalLocks = true
                }));

            services.AddHangfireServer();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            app.UseCors("Default");
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseHangfireDashboard(options: new DashboardOptions
            {
                Authorization = new[]
                {
                    new HangfireDashboardActionFilter()
                }
            });
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHangfireDashboard();
            });
        }
    }
}