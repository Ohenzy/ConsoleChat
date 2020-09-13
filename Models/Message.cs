using System;

namespace ConsoleChat.Chat.Models
{
    class Message
    {
        private string message;
        private DateTime dateCreated;

        public Message(string message)
        {
            this.message = message;
            this.dateCreated = DateTime.Now;
        }

        public string GetMessage() { return this.message; }
        public string GetTimeMessage() { return "(" + dateCreated.Hour + ":" + dateCreated.Minute + ":" + dateCreated.Second + ") "; }
    }
}
