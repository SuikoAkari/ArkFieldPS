using EndFieldPS.Protocol;
using Org.BouncyCastle.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EndFieldPS.Commands
{
    public static class BaseCommands
    {

        [Server.Command("scene", "Change scene")]
        public static void SceneCmd(string cmd, string[] args)
        {
            if (args.Length < 1) return;
            int sceneNumId = int.Parse(args[0]);

            foreach (var item in Server.clients)
            {
                item.EnterScene(sceneNumId);
            }
        }
        
    }
}