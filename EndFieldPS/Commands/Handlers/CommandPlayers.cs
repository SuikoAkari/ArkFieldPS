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
        public static void Handle(string cmd, string[] args, Player target)
        {
            Logger.Print("List of connected players:");
            
            if (Server.clients.Count == 0) Logger.Print("└No players on the server");
            foreach (var player in Server.clients)
            {
                string decorator = "│";
                if (player == Server.clients.Last()) decorator = "└";

                Logger.Print($"{decorator}Player: {player.nickname} | UID: {player.accountId}");
            }
        }
    }
}
