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

        [Server.Command("spawn", "Spawn cmd test",true)]
        public static void SpawnCmd(string cmd, string[] args, Player target)
        {
            if (args.Length < 2) return;
            string templateId = args[0];
            int level = int.Parse(args[1]);
            if(level < 1)
            {
                Logger.PrintError("Level can't be less than 1");
                return;
            }
            switch (templateId.Split("_")[0])
            {
                case "eny":
                    if (ResourceManager.enemyTable.ContainsKey(templateId))
                    {
                        EntityMonster mon = new(templateId, level, target.roleId, target.position, target.rotation);
                        target.sceneManager.SpawnEntity(mon);
                    }
                    else
                    {
                        Logger.PrintError("Monster template id not found");
                    }

                    break;
                default:

                    Logger.PrintError("Unsupported template id to spawn: " + templateId.Split("_")[0]);
                    break;
            }
            /*target.Send(ScMessageId.ScSpawnEnemy, new ScSpawnEnemy()
            {
                ClientKey=2,
                EnemyInstIds = { info.Detail.MonsterList[0].CommonInfo.Id }
            });*/
            
        }

        [Server.Command("arise", "Revives/Heals your characters")]
        public static void AriseCmd(string cmd, Player target)
        {
            target.Send(ScMessageId.ScSceneRevival, new ScSceneRevival());
        }
    }
}