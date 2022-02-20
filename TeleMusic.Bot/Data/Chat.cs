using System;

namespace TeleMusic.Bot.Data
{
    public class Chat
    {
        public Guid Id { get; set; }
        public string ChatId { get; set; }
        public string Message { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }
    }
}