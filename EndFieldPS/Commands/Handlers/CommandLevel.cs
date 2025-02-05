using EndFieldPS.Commands;
using EndFieldPS.Database;
using EndFieldPS.Packets.Sc;
using EndFieldPS.Resource;

namespace EndFieldPS.Game.Character
{
    public static class CharacterManager
    {
        [Server.Command("level", "Update character/item level", true)]
        public static void HandleLevel(Player sender, string cmd, string[] args, Player target)
        {
            if (args.Length < 1)
            {
                CommandManager.SendMessage(sender, @"Use: 
/level <char_id/item_id> <level> - Update specific character/item level
/level <level>                   - Update all characters and items to level");
                return;
            }

            try
            {
                if (args.Length == 1)
                {
                    if (!int.TryParse(args[0], out int level))
                    {
                        CommandManager.SendMessage(sender, "Invalid level number");
                        return;
                    }
                    UpdateAllLevels(sender, target, level);
                }
                else
                {
                    string id = args[0];
                    if (!int.TryParse(args[1], out int level))
                    {
                        CommandManager.SendMessage(sender, "Invalid level number");
                        return;
                    }
                    UpdateSingleLevel(sender, target, id, level);
                }
            }
            catch (Exception err)
            {
                CommandManager.SendMessage(sender, $"An error occurred: {err}");
            }
        }

        private static void UpdateAllLevels(Player sender, Player target, int level)
        {
            level = Math.Max(1, Math.Min(level, 80));

            int updatedCharCount = 0;
            foreach (var item in ResourceManager.itemTable)
            {
                if (item.Key.StartsWith("chr_"))
                {
                    AddOrUpdateCharacter(target, item.Key, level);
                    updatedCharCount++;
                }
            }

            int updatedItemCount = 0;
            foreach (var item in target.inventoryManager.items)
            {
                if (item.id.StartsWith("wpn_"))
                {
                    item.amount = 0;
                    target.Send(new PacketScItemBagScopeModify(target, item));

                    item.amount = 1;
                    item.level = (ulong)level;
                    target.Send(new PacketScItemBagScopeModify(target, item));

                    updatedItemCount++;
                }
            }

            target.SaveCharacters();
            target.inventoryManager.Save();
            CommandManager.SendMessage(sender, $"Updated {updatedCharCount} characters and {updatedItemCount} weapons to level {level}");
        }

        private static void UpdateSingleLevel(Player sender, Player target, string id, int level)
        {
            level = Math.Max(1, Math.Min(level, 80));

            if (id.StartsWith("chr_"))
            {
                if (!ResourceManager.itemTable.ContainsKey(id))
                {
                    CommandManager.SendMessage(sender, $"Invalid character ID: {id}");
                    return;
                }

                AddOrUpdateCharacter(target, id, level);
                target.SaveCharacters();
                CommandManager.SendMessage(sender, $"Character {id} level updated to {level}");
            }
            else if (id.StartsWith("wpn_"))
            {
                var item = target.inventoryManager.GetItemById(id);
                if (item == null)
                {
                    CommandManager.SendMessage(sender, $"Item not found: {id}");
                    return;
                }

                item.amount = 0;
                target.Send(new PacketScItemBagScopeModify(target, item));

                item.amount = 1;
                item.level = (ulong)level;
                target.Send(new PacketScItemBagScopeModify(target, item));

                target.inventoryManager.Save();
                CommandManager.SendMessage(sender, $"Weapon {id} level updated to {level}");
            }
            else
            {
                CommandManager.SendMessage(sender, $"Invalid ID format: {id} (must start with 'chr_' or 'wpn_')");
            }
        }

        private static Character AddOrUpdateCharacter(Player target, string charId, int level)
        {
            Character existingChar = target.chars.Find(c => c.id == charId);
            if (existingChar != null)
            {
                existingChar.level = level;
                existingChar.breakNode = GetBreakNodeByLevel(level);

                target.Send(new PacketScCharBagDelChar(target, existingChar));
                target.Send(new PacketScCharBagAddChar(target, existingChar));

                return existingChar;
            }

            Character character = new Character(target.roleId, charId, level);
            character.breakNode = GetBreakNodeByLevel(level);
            target.chars.Add(character);

            target.Send(new PacketScCharBagAddChar(target, character));

            return character;
        }

        private static string GetBreakNodeByLevel(int level)
        {
            if (level <= 20) return "";
            if (level <= 40) return "charBreak20";
            if (level <= 60) return "charBreak40";
            if (level <= 70) return "charBreak60";
            return "charBreak70";
        }
    }
}