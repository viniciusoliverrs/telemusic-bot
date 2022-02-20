using System;
using System.Collections.Generic;

namespace TeleMusic.Bot.Data
{
    public class User
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public bool? Dev { get; set; }
        List<Chat> Chats { get; set; }
    }
}