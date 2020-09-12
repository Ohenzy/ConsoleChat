using ConsoleChat.Chat.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ConsoleChat
{
    class ServerChat
    {
        private long index;
        private List<User> users;
        private List<Message> messages;

        private Socket serverSoceket;

        public ServerChat()
        {
            index = 0;
            users = new List<User>();
            messages = new List<Message>();
            serverSoceket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            JObject dataPoint = JObject.Parse(File.ReadAllText("C:\\Users\\Ohenzy\\CSharpProjects\\ConsoleChat\\resource\\properties.json"));
            IPEndPoint point = new IPEndPoint(IPAddress.Parse(dataPoint.GetValue("addres").ToString()), int.Parse(dataPoint.GetValue("port").ToString()));
            serverSoceket.Bind(point);
            serverSoceket.Listen(10);
        }
        public void start()
        {
            Console.WriteLine("Сервер запущен");
            while (true)
            {
                Socket clientSocket = serverSoceket.Accept();
                StringBuilder jsonData = new StringBuilder();
                byte[] data = new byte[256];

                do
                {
                    jsonData.Append(Encoding.UTF8.GetString(data, 0, clientSocket.Receive(data)));
                } while (clientSocket.Available > 0);

                JObject jsonObj = new JObject(jsonData.ToString());
                long userId = long.Parse(jsonObj.GetValue("id").ToString());

                if (!this.existUser(userId))
                    this.ConnectUser(jsonObj.GetValue("name").ToString(), clientSocket);
                else
                    this.SendMessageAll(jsonObj);

                
                    


            }
        }

        private bool existUser(long userId)
        {
            bool exists = false;
            foreach (User user in users)
                if (user.GetId().Equals(userId))
                    exists = true;

            return exists;
        }
        private void ConnectUser(string name, Socket userSocket)
        {
            users.Add(new User(++index, name, userSocket));
            JObject json = new JObject();
            json.Add("id", index);
            userSocket.Send(Encoding.UTF8.GetBytes(json.ToString()));
            userSocket.Shutdown(SocketShutdown.Both);
            userSocket.Close();

            string msg = name + " подключился к чату";
            Console.WriteLine(msg);
            this.SendServerMessage(msg);
        }

        private bool DisconnectUser(long userId)
        {
            User user = this.getUserById(userId);
            return (user == null ? false : users.Remove(user));
        }
        private int GetCountUsers()
        {
            return users.Count;
        }

        private List<User> GetUsers()
        {
            return users;
        }

        private void SendMessageAll(JObject json)
        {

            
        }
        private void SendServerMessage(string serverMessage)
        {
            foreach (User user in users)
            {
                Socket socket = user.GetUserSocket();
                socket.Listen(10);

            }
                

        }
        private User getUserById(long userId)
        {
            foreach (User user in users)
                if(user.GetId().Equals(userId))
                    return user;

            return null;
        }
    }
}
