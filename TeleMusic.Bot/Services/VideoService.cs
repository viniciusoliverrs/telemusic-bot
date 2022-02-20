using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TeleMusic.Bot.Data;

namespace TeleMusic.Bot.Services
{
    public class VideoService
    {
        public VideoService() { }
        public Video Search(string q)
        {
            using (var context = new DataContext())
            {
                return context.Videos
                        .Where(v =>
                            v.Title.ToUpper().Contains(q.ToUpper()) ||
                            v.ChannelTitle.ToUpper().Contains(q.ToUpper())
                        ).FirstOrDefault();
            }
        }
        public async Task Add(Video model)
        {
            using (var context = new DataContext())
            {
                await context.Videos.AddAsync(model);
                await context.SaveChangesAsync();
            }
        }
        public async Task PostRange(List<Video> model)
        {
            using (var context = new DataContext())
            {
                await context.Videos.AddRangeAsync(model);
                await context.SaveChangesAsync();
            }
        }
    }
}