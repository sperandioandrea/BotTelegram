using System;
using System.Threading.Tasks;


namespace BotTelegram
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            BotService botService = new BotService();
            await botService.StartAsync();

            Console.WriteLine("Bot started...");
            Console.ReadLine();
        }
    }
}
