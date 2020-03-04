using Newtonsoft.Json;
using System;
namespace ChatApp.Models
{
    public class Message
    {
        public string Id { get; set; }
        public string Text { get; set; }
        public string User { get; set; }
        public string Time { get; set; }
    }
}
