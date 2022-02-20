using System;

namespace TeleMusic.Bot.ViewModel
{
    public class VideoResponse
    {
        public string VideoId { get; set; }
        public string ChannelTitle { get; set; }
        public string ThumbnailsMedium { get; set; }
        public string ThumbnailsHigh { get; set; }
        public string Description { get; set; }
        public string Title { get; set; }
        public DateTime PublishedAt { get; set; }
        public int Duration { get; set; }
    }
}