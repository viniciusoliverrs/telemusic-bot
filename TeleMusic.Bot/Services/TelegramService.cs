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
    public class TelegramService
    {
        private readonly VideoService videoService;
        private readonly YoutubeService youtubeService;
        private readonly string API_KEY = "5180450615:AAHPQIWQcDtnVuHJcGSnOPAZYA9E55r9_ys";
        public TelegramService(VideoService videoService, YoutubeService youtubeService)
        {
            this.videoService = videoService;
            this.youtubeService = youtubeService;
        }
        public async Task Init()
        {
            try
            {
                var botClient = new TelegramBotClient(API_KEY);
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
                TeleMusic.Bot.Data.Video video = null;
                if (update.Type != UpdateType.Message || update.Message!.Type != MessageType.Text)
                    return;
                long chatId = update.Message.Chat.Id;
                var query = update.Message.Text;
                if (videoService.Search(query) == null)
                {
                    video = await youtubeService.GetVideos(query);
                    await videoService.Add(video);
                }
                else
                {
                    video = videoService.Search(query);
                }

                Message sentMessage = await SendTextMessage(video.link, chatId, botClient, cancellationToken);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public async Task<Message> SendTextMessage(string messsage, long chatId, ITelegramBotClient botClient, CancellationToken cancellationToken)
        {
            return await botClient.SendTextMessageAsync(
                    chatId: chatId,
                    text: messsage,
                    cancellationToken: cancellationToken);
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