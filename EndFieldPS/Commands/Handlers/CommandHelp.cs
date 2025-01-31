using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EndFieldPS.Commands.Handlers
{
    public static class CommandHelp
    {
        [Server.Command("help", "Show list of commands", false)]
        public static void Handle(Player sender, string cmd, string[] args, Player target)
        {
            CommandManager.SendMessage(sender, "List of possible commands: ");
            foreach (var command in CommandManager.s_notifyReqGroup)
            {
                CommandManager.SendMessage(sender, $"/{command.Key} - {command.Value.Item1.desc} (Require Target: {command.Value.Item1.requiredTarget})");
            }

        }
    }
}
