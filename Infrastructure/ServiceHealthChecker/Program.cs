using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace ServiceHealthChecker
{
    public class Program
    {
        public static async Task<int> Main(string[] args)
        {
            if (args.Length != 2)
                throw new NotImplementedException("Args structure not supported now");

            HttpClient client = new()
            {
                Timeout = TimeSpan.FromSeconds(10)
            };
            try
            {
                var response = await client.GetAsync(args[0]);
                Console.WriteLine($"Request to {args[0]} returned {response.StatusCode}");
                if (response.IsSuccessStatusCode)
                    return 0;
            }
            catch (Exception)
            {
                var chatId = Environment.GetEnvironmentVariable("TELEGRAM_CHAT_ID");
                var botAccessToken = Environment.GetEnvironmentVariable("TELEGRAM_BOT_ACCESS_TOKEN");
                var telegramApiEndpoint = $"https://api.telegram.org/bot{botAccessToken}/sendMessage?chat_id=${chatId}&text=Service {args[1]} is downed";
                Console.WriteLine(telegramApiEndpoint);
                await client.GetAsync(telegramApiEndpoint);
                return 2;
            }
            return 2;
        }
    }
}