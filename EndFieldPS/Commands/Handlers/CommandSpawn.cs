using EndFieldPS.Game.Entities;
using EndFieldPS.Resource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EndFieldPS.Commands.Handlers
{
    public static class CommandSpawn
    {
        [Server.Command("spawn", "Spawn cmd test", true)]
        public static void Handle(Player sender, string cmd, string[] args, Player target)
        {
            if (args.Length < 2)
            {
                CommandManager.SendMessage(sender, "Example: spawn (id) (level)");

                return;
            }
            string templateId = args[0];
            int level = int.Parse(args[1]);
            if (level < 1)
            {
                CommandManager.SendMessage(sender, "Level can't be less than 1");
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
                        CommandManager.SendMessage(sender, "Monster template id not found");
                    }

                    break;
                default:

                    CommandManager.SendMessage(sender, "Unsupported template id to spawn: " + templateId.Split("_")[0]);
                    break;
            }
            /*target.Send(ScMessageId.ScSpawnEnemy, new ScSpawnEnemy()
            {
                ClientKey=2,
                EnemyInstIds = { info.Detail.MonsterList[0].CommonInfo.Id }
            });*/

        }
    }
}
