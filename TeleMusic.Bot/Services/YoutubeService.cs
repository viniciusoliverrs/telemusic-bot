using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using Newtonsoft.Json;

namespace TeleMusic.Bot.Services
{
    public class YoutubeService
    {
        private string API_KEY = "AIzaSyDa2HU_TDh6f2WQCUQBzHkSEz8Bgj2kXrk";
        public YoutubeService() { }
        public async Task<TeleMusic.Bot.Data.Video> GetVideos(string term)
        {

            var youtubeService = new YouTubeService(new BaseClientService.Initializer()
            {
                ApiKey = API_KEY,
                ApplicationName = this.GetType().ToString()
            });
            var searchListRequest = youtubeService.Search.List("snippet");
            searchListRequest.Q = term; // Replace with your search term.
            searchListRequest.MaxResults = 1;
            var searchListResponse = await searchListRequest.ExecuteAsync();
            return searchListResponse.Items.Select(s => new TeleMusic.Bot.Data.Video
            {
                VideoId = s.Id.VideoId,
                ChannelTitle = s.Snippet.ChannelTitle,
                Title = s.Snippet.Title,
                PublishedAt = (DateTime)s.Snippet.PublishedAt,
                ThumbnailsMedium = s.Snippet.Thumbnails.Medium.Url,
                ThumbnailsHigh = s.Snippet.Thumbnails.High.Url,
                Description = s.Snippet.Description,
                Duration = GetVideoDuration(s.Id.VideoId),
                link = $"https://www.youtube.com/watch?v={s.Id.VideoId}&ab_channel={s.Snippet.ChannelTitle}"
            }).FirstOrDefault();
        }
        public int GetVideoDuration(string videoId)
        {
            using (var webClient = new WebClient())
            {
                if (videoId == null) return 0;
                var url = $"https://www.googleapis.com/youtube/v3/videos?id={String.Format(videoId)}&key={API_KEY}&part=contentDetails";
                var jsonResponse = webClient.DownloadString(url);
                dynamic dynamicObject = JsonConvert.DeserializeObject(jsonResponse);
                string tmp = dynamicObject.items[0].contentDetails.duration;
                return Convert.ToInt32(System.Xml.XmlConvert.ToTimeSpan(tmp).TotalMinutes);
            }
        }
    }
}