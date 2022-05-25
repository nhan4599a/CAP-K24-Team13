using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RatingService;
using System.Net;

namespace OrderService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                    webBuilder.UseKestrel(serverOptions =>
                    {
                        serverOptions.UseSystemd();
                        serverOptions.Listen(IPAddress.Any, 3003, listenOptions =>
                        {
                            listenOptions.UseHttps("/home/ubuntu/certificate.pfx");
                        });
                    });
                })
                .ConfigureLogging(builder =>
                {
                    builder.AddConsole();
                });
    }
}