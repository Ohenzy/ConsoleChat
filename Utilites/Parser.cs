using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace ConsoleChat.Chat.Utilites
{
    class Parser
    {
        private int userId;
        private string userName;
        private IPAddress address;
        private string textMessage;

        public Parser(JObject json)
        {
            this.userId = int.Parse(json.GetValue("id").ToString());
            this.userName = json.GetValue("name").ToString();
            this.address = IPAddress.Parse(json.GetValue("ipAddress").ToString());
            this.textMessage = (json.GetValue("textMessage") == null ? "" : json.GetValue("textMessage").ToString());

        }
        public int GetUserId()
        {
            return this.userId;
        }
        public string GetUserName()
        {
            return this.userName;
        }
        public IPAddress GetIPAddress()
        {
            return this.address;
        }
        public string GetTextMessage()
        {
            return this.textMessage;
        }

    }
}
