using EndFieldPS.Database;
using EndFieldPS.Game.Entities;
using EndFieldPS.Protocol;
using EndFieldPS.Resource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static EndFieldPS.Resource.ResourceManager;

namespace EndFieldPS.Commands
{
    public static class BaseCommands
    {


        [Server.Command("scene", "Change scene",true)]
        public static void SceneCmd(Player sender, string cmd, string[] args, Player target)
        {
            if (args.Length < 1) return;
            int sceneNumId = int.Parse(args[0]);
            target.EnterScene(sceneNumId);
            CommandManager.SendMessage(sender, "Changing scene");

        }
        [Server.Command("target", "Set a target uid", false)]
        public static void TargetCmd(Player sender, string cmd, string[] args, Player target)
        {
            if (sender != null)
            {
                CommandManager.SendMessage(sender, "This command can't be used ingame");
                return;
            }
            if (args.Length < 1)
            {
                CommandManager.SendMessage(sender,"Use: /target (uid)");
                return;
            }
            string id = args[0];
            Player player = Server.clients.Find(c=>c.accountId == id);
            if (player == null)
            {
                CommandManager.SendMessage(sender, "Only online players can be set as target");
                return;
            }
            CommandManager.targetId = id;
            CommandManager.SendMessage(sender, "Set Target player to " +id);
        }

        

    }
}