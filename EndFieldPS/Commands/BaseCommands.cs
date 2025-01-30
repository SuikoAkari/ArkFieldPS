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
        public static void SceneCmd(string cmd, string[] args, Player target)
        {
            if (args.Length < 1) return;
            int sceneNumId = int.Parse(args[0]);
            target.EnterScene(sceneNumId);

        }
        [Server.Command("target", "Set a target uid", false)]
        public static void TargetCmd(string cmd, string[] args, Player target)
        {
            if (args.Length < 1)
            {
                Logger.PrintError("Use: /target (uid)");
                return;
            }
            string id = args[0];
            Player player = Server.clients.Find(c=>c.accountId == id);
            if (player == null)
            {
                Logger.PrintError("Only online players can be set as target");
                return;
            }
            CommandManager.targetId = id;
            Logger.Print("Set Target player to "+id);
        }

        

    }
}