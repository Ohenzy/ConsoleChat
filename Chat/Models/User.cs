using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace ConsoleChat.Chat.Models
{
    class User
    {
        private long id;
        private string name;
        private Socket userSocket;
        private List<Message> messages;

        public User(long id, string name, Socket userSocket)
        {
            this.id = id;
            this.name = name;
            this.userSocket = userSocket;
            messages = new List<Message>();
        }

        public User AddMessage(string message)
        {
            messages.Add(new Message(message));
            return this;
        }
        public long GetId() { return id; }
        public string GetName() { return name; }
        public Socket GetUserSocket() { return this.userSocket; }
    }
}
