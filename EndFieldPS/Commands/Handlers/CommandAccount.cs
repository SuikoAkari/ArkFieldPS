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
        public static void Handle(string cmd, string[] args, Player target)
        {
            if (args.Length < 2)
            {
                Logger.Print("Usage: account create|reset (username)");
                return;
            }
            switch (args[0])
            {
                case "create":
                    DatabaseManager.db.CreateAccount(args[1]);
                    break;
                case "reset":
                    Logger.Print("Reset is not implemented yet");
                    break;
                default:
                    Logger.Print("Example: account create (username)");
                    break;
            }
        }
    }
}
