using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace BotTelegram
{
    public class BotService
    {
        private static readonly string BotToken = "6723909841:AAFmguHkiZIcR3JkVr3HvEVNFzjbMMltA_w";
        private readonly TelegramBotClient _botClient;

        public BotService()
        {
            _botClient = new TelegramBotClient(BotToken);
        }

        public async Task StartAsync()
        {
            var cts = new CancellationTokenSource();

            var receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = Array.Empty<UpdateType>() 
            };

            _botClient.StartReceiving(
                HandleUpdateAsync,
                HandleErrorAsync,
                receiverOptions,
                cancellationToken: cts.Token);

            Console.WriteLine("Bot started...");
            await Task.Delay(-1, cts.Token);
        }

        public void Stop()
        {
            
        }

        private async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            if (update.Type == UpdateType.Message && update.Message?.Text != null)
            {
                var message = update.Message;
                var chatId = message.Chat.Id;  
                Console.WriteLine($"Received a text message from {message.Chat.FirstName}: {message.Text.ToLower()}");

                if (message.Text.EndsWith(".xml"))
                {
                    string feedUrl = message.Text;
                    string feedContent = await RssService.GetRssFeedContent(feedUrl);
                    await botClient.SendTextMessageAsync(584569458, feedContent, cancellationToken: cancellationToken);
                }
                
                
            }
            
        }

        private Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            string errorMessage = exception.Message;

            if (exception is ApiRequestException apiRequestException)
            {
                errorMessage = $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}";
            }
            else
            {
                errorMessage = exception.ToString();
            }

            Console.WriteLine(errorMessage);
            return Task.CompletedTask;
        }
    }
}





