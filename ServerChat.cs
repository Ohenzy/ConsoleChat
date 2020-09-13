using ConsoleChat.Chat.Models;
using ConsoleChat.Chat.Utilites;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace ConsoleChat
{
    class ServerChat
    {
        private int index = 0;
        private List<User> users;
        private int serverPort;
        private Socket serverSoceket;
        private Socket connectedSocket;
        private CommandHandler commandHandler;
        private bool isRun;

        public ServerChat()
        {
            this.serverPort = 11414;
            this.users = new List<User>();
            this.commandHandler = new CommandHandler();
            this.serverSoceket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            this.serverSoceket.Bind(new IPEndPoint(Dns.GetHostByName(Dns.GetHostName()).AddressList[1], this.serverPort));
            this.serverSoceket.Listen(10);
            this.isRun = true;
        }
        public void Start()
        {
            Console.WriteLine("Сервер запущен");
            while (this.isRun)
            {
                Console.Write(".");
                Parser value = this.AcceptMessage();
                if (this.commandHandler.findCommand(this, value))
                    continue;
                User user = this.findUserById(value.GetUserId());
                if (user == null)
                    this.ConnectUser(value);
                else
                    this.SendMessageAll(user.AddMessage(value.GetTextMessage()));                
            }
            Console.WriteLine("Сервер остановленн");
        }
        public Parser AcceptMessage()
        {
            this.connectedSocket = this.serverSoceket.Accept();
            StringBuilder jsonData = new StringBuilder();
            byte[] data = new byte[256];
            do
            {
                jsonData.Append(Encoding.UTF8.GetString(data, 0, this.connectedSocket.Receive(data)));
            } while (this.connectedSocket.Available > 0);

            return new Parser(JObject.Parse(jsonData.ToString()));
        }
        public void ConnectUser(Parser value)
        {
            this.index++;
            this.connectedSocket.Send(Encoding.UTF8.GetBytes(this.index.ToString()));
            this.connectedSocket.Shutdown(SocketShutdown.Both);
            this.connectedSocket.Close();
            this.SendMessageAll(value.GetUserName() + " подключился к чату");
            users.Add(new User(this.index, value.GetUserName(), new IPEndPoint(value.GetIPAddress(), serverPort + index)));
            Console.WriteLine(value.GetUserName() + " подключенн");
        }

        public bool DisconnectUser(User user)
        {
            return (user == null ? false : users.Remove(user));
        }
        public void SendMessageAll(string serverMessage)
        {
            foreach (User user in users)
            {
                int countTry = 0;
                tryConnect:
                if (countTry > 10)
                    continue;
                try
                {
                    Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    socket.Connect(user.GeIPEndPoint());
                    socket.Send(Encoding.UTF8.GetBytes(serverMessage));
                    socket.Shutdown(SocketShutdown.Both);
                    socket.Close();
                } catch
                {
                    Thread.Sleep(500);
                    countTry++;
                    goto tryConnect;
                }
            }
        }
        public User findUserById(int userId)
        {
            foreach (User user in users)
                if(user.GetId().Equals(userId))
                    return user;
            return null;
        }

        public void Shutdown()
        {
            this.serverSoceket.Shutdown(SocketShutdown.Both);
            this.serverSoceket.Close();
            this.isRun = false;
        }
    }
}
