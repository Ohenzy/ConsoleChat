using ConsoleChat.Chat.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleChat.Chat.Utilites
{
    class CommandHandler
    {
        public bool findCommand(ServerChat server, Parser value)
        {
            bool isCommand = false;

            if (value.GetTextMessage().Equals("qqq"))
            {
                User user = server.findUserById((value.GetUserId()));
                if (server.DisconnectUser(user))
                    Console.WriteLine(user.GetName() + " отключен");
                isCommand = true;
            }
            else if (value.GetTextMessage().Equals("sqqq"))
            {
                server.Shutdown();
                isCommand = true;
            }

            return isCommand;
        }
    }
}
