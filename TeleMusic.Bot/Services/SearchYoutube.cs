using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using Newtonsoft.Json;
using TeleMusic.Bot.ViewModel;

namespace TeleMusic.Bot.Services
{
    public class SearchYoutube
    {
        private string API_KEY = "AIzaSyDa2HU_TDh6f2WQCUQBzHkSEz8Bgj2kXrk";
        public async Task<VideoResponse> GetVideos(string term)
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
            return searchListResponse.Items.Select(s=> new VideoResponse
            {
                VideoId = s.Id.VideoId,
                ChannelTitle = s.Snippet.ChannelTitle,
            }).FirstOrDefault();
        }
        public int GetVideoDuration(string videoId)
        {
            using (var webClient = new WebClient())
            {
                    var url = $"https://www.googleapis.com/youtube/v3/videos?id={String.Format(videoId)}&key={API_KEY}&part=contentDetails";
                    var jsonResponse = webClient.DownloadString(url);
                    dynamic dynamicObject = JsonConvert.DeserializeObject(jsonResponse);
                    string tmp = dynamicObject.items[0].contentDetails.duration;
                    return Convert.ToInt32(System.Xml.XmlConvert.ToTimeSpan(tmp).TotalMinutes);
            }
        }
    }
}