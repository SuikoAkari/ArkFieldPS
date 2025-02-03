using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static EndFieldPS.Resource.ResourceManager;
using EndFieldPS.Packets.Sc;
using MongoDB.Bson;

namespace EndFieldPS.Commands.Handlers
{
    public class CommandTeleport
    {
        [Server.Command("tp", "Teleports player", true)]
        public static void Handle(Player sender, string cmd, string[] args, Player target) 
        {
            if (args.Length < 3)
            {
                CommandManager.SendMessage(sender, "Use: /tp (x) (y) (z)\nYou can use ~ to use current player coordinate");
                CommandManager.SendMessage(sender, $"\nCurrent player position: {target.position.ToJson()}");
                return;
            }

            for (int i=0; i < args.Length; i++) 
            {
                args[i] = Uri.UnescapeDataString(args[i]);
            }

            float x, y, z;

            x = args[0] == "~" ? target.position.x : float.Parse(args[0]);
            y = args[1] == "~" ? target.position.y : float.Parse(args[1]);
            z = args[2] == "~" ? target.position.z : float.Parse(args[2]);

            Vector3f position = new Vector3f(new Vector()
            {
                X = x,
                Y = y,
                Z = z
            });

            target.position = position;
            target.Send(new PacketScEnterSceneNotify(target, target.curSceneNumId, position));
            CommandManager.SendMessage(sender, $"Player teleported to {target.position.ToJson()}");
        }
    }
}
