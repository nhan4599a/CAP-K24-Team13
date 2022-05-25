using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System.Net;

namespace ApiGateway
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
                        serverOptions.Listen(IPAddress.Any, 3000, listenOptions =>
                        {
                            listenOptions.UseHttps("/home/ubuntu/certificate.pfx");
                        });
                    });
                });
    }
}