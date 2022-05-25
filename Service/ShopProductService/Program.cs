using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Net;

namespace ShopProductService
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
                        serverOptions.Listen(IPAddress.Any, 3005, listenOptions =>
                        {
                            listenOptions.UseHttps("/home/ubuntu/certificate.crt");
                        });
                    });
                })
                .ConfigureLogging(buider =>
                {
                    buider.AddConsole();
                });
    }
}