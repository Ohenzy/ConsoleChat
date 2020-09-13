using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ConsoleChat.Chat.Models
{
    class User
    {
        private int id;
        private string name;
        private IPEndPoint ipAddress;
        private List<Message> messages;

        public User(int id, string name, IPEndPoint ipAddress)
        {
            this.id = id;
            this.name = name;
            this.ipAddress = ipAddress;
            messages = new List<Message>();
        }

        public string AddMessage(string message)
        {
            Message currMessage = new Message(message);
            messages.Add(currMessage);
            return currMessage.GetTimeMessage() + this.name + ": " + message;
        }
        public int GetId() { return id; }
        public string GetName() { return name; }
        public IPEndPoint GeIPEndPoint() { return this.ipAddress; }
    }
}
