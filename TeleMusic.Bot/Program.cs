using System;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TeleMusic.Bot.Services;

namespace TeleMusic.Bot
{
    class Program
    {
        static async Task Main(string[] args)
        {
            await new TeleBot().Init();
        }
    }
}
