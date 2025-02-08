using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EndFieldPS.Packets.Sc;

namespace EndFieldPS.Commands.Handlers
{

    public static class CommandNickname
    {
        [Server.Command("nickname", "Changes nickname", true)]
        public static void Handle(Player sender, string cmd, string[] args, Player target)
        {
            if (args.Length < 1)
            {
                CommandManager.SendMessage(sender, "Use: /nickname (your new nickname)");
                return;
            }

            for (int i=0; i < args.Length; i++) 
            {
                args[i] = Uri.UnescapeDataString(args[i]);
            }

            target.nickname = string.Join(" ", args);
            target.Save();
            target.Send(new PacketScSetName(target, target.nickname));
            CommandManager.SendMessage(sender, $"Nickname was changed to {target.nickname}");
        }
    }
}
