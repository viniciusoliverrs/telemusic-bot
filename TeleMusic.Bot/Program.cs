using System.Threading.Tasks;
using TeleMusic.Bot.Services;

namespace TeleMusic.Bot
{
    class Program
    {
        static async Task Main(string[] args)
        {
            VideoService videoService = new VideoService();
            YoutubeService youtubeService = new YoutubeService();
            await new TelegramService(videoService, youtubeService).Init();
        }
    }
}
