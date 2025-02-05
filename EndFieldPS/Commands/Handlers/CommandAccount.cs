using EndFieldPS.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EndFieldPS.Commands.Handlers
{
    public static class CommandAccount
    {
        [Server.Command("account", "account command")]
        public static void Handle(Player sender,string cmd, string[] args, Player target)
        {
            if (sender != null)
            {
                CommandManager.SendMessage(sender, "This command can't be used ingame");
                return;
            }
            if (args.Length < 2)
            {
                CommandManager.SendMessage(sender, "Usage: account create|reset (username)");
                return;
            }
            switch (args[0])
            {
                case "create":
                    DatabaseManager.db.CreateAccount(args[1]);
                    break;
                case "reset":
                    CommandManager.SendMessage(sender, "Reset is not implemented yet");
                    break;
                default:
                    CommandManager.SendMessage(sender, "Example: account create (username)");
                    break;
            }
        }
    }
}
