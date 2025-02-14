using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using static EndFieldPS.Resource.ResourceManager;
using Newtonsoft.Json;
using EndFieldPS.Game.Character;
using EndFieldPS.Database;
using EndFieldPS.Game.Inventory;

namespace EndFieldPS.Commands.Handlers
{
    public class CommandCharInfo
    {
        [Server.Command("charinfo", "Information about characters", true)]
        public static void Handle(Player sender, string cmd, string[] args, Player target)
        {
            // maybe we could use localization here
            // string lang = "EN";
            // Dictionary<string, string> locTable = JsonConvert.DeserializeObject<Dictionary<string, string>>(ReadJsonFile($"TableCfg/I18nTextTable_{lang}.json"));

            if (args.Length < 1)
            {
                CommandManager.SendMessage(sender, "Use: /charinfo (id)");
                CommandManager.SendMessage(sender, "\nAll characters:");
                foreach (Character chara in target.chars)
                {
                    //locTable.TryGetValue(characterTable.Values.ToList().Find(x => x.charId == chara.id).name.id, out string aName);
                    string aName = characterTable.Values.ToList().Find(x => x.charId == chara.id).engName;
                    CommandManager.SendMessage(sender, $"{aName} ({chara.id})");
                }
                return;
            }

            //locTable.TryGetValue(characterTable.Values.ToList().Find(x => x.charId == args[0]).name.id, out string name);
            string name = characterTable.Values.ToList().Find(x => x.charId == args[0]).engName;

            Character character = target.GetCharacter(args[0]);
            Item weapon = DatabaseManager.db.LoadInventoryItems(target.roleId).Find(w => w.guid == character.weaponGuid);

            string responce = @$"
            // {name} ({character.id})
            │   guid: {character.guid}
            │   level: {character.level}
            │   curHp: {character.curHp}
            │   potential: {character.potential}
            │   breakNode: {character.breakNode}
            │   
            ├   // Weapon ({weapon.id})
            │   guid: {character.weaponGuid}
            │   level: {weapon.level}
            │   weapon breakThroughLvl: {weapon.breakthroughLv}
            └   weapon potential: {weapon.refineLv}";
            CommandManager.SendMessage(sender, responce);
        }
    }
}
