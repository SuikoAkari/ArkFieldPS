using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EndFieldPS.Commands.Handlers
{
    public static class CommandClear
    {
        [Server.Command("clear", "clears the console", false)]
        public static void Handle(Player sender,string cmd, string[] args, Player target)
        {
            if (sender != null)
            {
                CommandManager.SendMessage(sender, "This command can't be used ingame");
                return;
            }
            Console.Clear();
            Logger.Print("Console cleared");
        }
    }
}
