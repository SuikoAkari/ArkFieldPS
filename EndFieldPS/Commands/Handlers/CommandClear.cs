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
        public static void Handle(string cmd, string[] args, Player target)
        {
            Console.Clear();
            Logger.Print("Console cleared");
        }
    }
}
