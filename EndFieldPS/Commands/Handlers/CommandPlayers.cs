using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EndFieldPS.Commands.Handlers
{
    public static class CommandPlayers
    {
        [Server.Command("players", "list of connected players", false)]
        public static void Handle(Player sender, string cmd, string[] args, Player target)
        {
            CommandManager.SendMessage(sender, "List of connected players:");
            
            if (Server.clients.Count == 0) CommandManager.SendMessage(sender, "└No players on the server");
            foreach (var player in Server.clients)
            {
                string decorator = "│";
                if (player == Server.clients.Last()) decorator = "└";

                CommandManager.SendMessage(sender, $"{decorator}Player: {player.nickname} | UID: {player.accountId}");
            }
        }
    }
}
