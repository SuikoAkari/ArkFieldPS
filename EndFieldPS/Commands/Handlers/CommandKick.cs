using EndFieldPS.Resource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EndFieldPS.Commands.Handlers
{
    public static class CommandKick
    {
        [Server.Command("kick", "kick target", true)]
        public static void Handle(string cmd, string[] args, Player target)
        {
            target.Kick(CODE.ErrKickSessionEnd, "Kicked");
            Logger.Print("Kicked " + target.accountId);
        }
    }
}
