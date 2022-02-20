using System.Threading.Tasks;
using TeleMusic.Bot.Services;

namespace TeleMusic.Bot
{
    class Program
    {
        static async Task Main(string[] args)
        {
            await new TelegramService(new VideoService(), new YoutubeService()).Init();
        }
    }
}
