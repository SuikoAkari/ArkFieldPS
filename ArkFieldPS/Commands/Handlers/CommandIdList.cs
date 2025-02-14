using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ArkFieldPS.Resource.ResourceManager;

namespace ArkFieldPS.Commands.Handlers 
{
    public class CommandIdList
    {
        [Server.Command("idlist", "List of all ids", false)]
        public static void Handle(Player sender, string cmd, string[] args, Player target)
        {
            if (args.Length < 1) 
            {
                CommandManager.SendMessage(sender, "Use: /idlist (chars|enemies|scenes)");
                return;
            }

            switch (args[0])
            {
                case "chars":
                    CommandManager.SendMessage(sender, "Character ids:");
                    characterTable.Values.ToList().ForEach(c => {
                        CommandManager.SendMessage(sender, $"{c.charId} ({c.engName})");
                    });
                    break;

                case "enemies":
                    CommandManager.SendMessage(sender, "Enemy ids:");
                    enemyTable.Values.ToList().ForEach(e => {
                        CommandManager.SendMessage(sender, $"{e.enemyId} (templateId: {e.templateId})");
                    });
                    break;

                case "scenes":
                    CommandManager.SendMessage(sender, "Scene ids:");
                    levelDatas.ForEach(s => {
                         CommandManager.SendMessage(sender, $"{s.id} (id: {s.idNum})");
                    });
                    break;

                default:
                    CommandManager.SendMessage(sender, "Category not found");
                    break;
            }
        }
    }
}
