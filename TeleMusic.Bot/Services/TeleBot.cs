using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace TeleMusic.Bot.Services
{
    public class TeleBot
    {
        public async Task Init()
        {
            try
            {
                var botClient = new TelegramBotClient("5180450615:AAHPQIWQcDtnVuHJcGSnOPAZYA9E55r9_ys");
                using var cts = new CancellationTokenSource();
                var receiverOptions = new ReceiverOptions
                {
                    AllowedUpdates = { } // receive all update types
                };
                botClient.StartReceiving(
                    HandleUpdateAsync,
                    HandleErrorAsync,
                    receiverOptions,
                    cancellationToken: cts.Token);

                var me = await botClient.GetMeAsync();
                Console.WriteLine($"Start listening for @{me.Username}");
                Console.ReadLine();
                cts.Cancel();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            try
            {
                if (update.Type != UpdateType.Message || update.Message!.Type != MessageType.Text)
                    return;

                long chatId = update.Message.Chat.Id;
                var video = await new SearchYoutube().GetVideos(update.Message.Text);

                Message sentMessage = await botClient.SendTextMessageAsync(
                    chatId: chatId,
                    text: $"https://www.youtube.com/watch?v={video.VideoId}&ab_channel={video.ChannelTitle}",
                    cancellationToken: cancellationToken);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            var ErrorMessage = exception switch
            {
                ApiRequestException apiRequestException
                    => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => exception.ToString()
            };

            Console.WriteLine(ErrorMessage);
            return Task.CompletedTask;
        }
    }
}