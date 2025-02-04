using EndFieldPS.Commands;
using EndFieldPS.Database;
using EndFieldPS.Packets.Sc;
using EndFieldPS.Resource;
using System.Text;

namespace EndFieldPS.Game.Character
{
    public static class CharacterManager
    {
        [Server.Command("char", "Show character list", true)]
        public static void HandleCharList(Player sender, string cmd, string[] args, Player target)
        {
            if (args.Length < 1 || args[0].ToLower() != "list")
            {
                CommandManager.SendMessage(sender, "Use: /char list - Show all characters");
                return;
            }

            StringBuilder sb = new StringBuilder("Character List:\n");

            var charConfigs = ResourceManager.itemTable
                .Where(item => item.Key.StartsWith("chr_"))
                .OrderBy(item => item.Key);

            foreach (var charConfig in charConfigs)
            {
                var existingChar = target.chars.Find(c => c.id == charConfig.Key);
                string levelInfo = existingChar != null ? $"(Level {existingChar.level})" : "(Not Owned)";
                sb.AppendLine($"{charConfig.Key} {levelInfo}");
            }

            CommandManager.SendMessage(sender, sb.ToString());
        }

        [Server.Command("level", "Update character level", true)]
        public static void HandleLevel(Player sender, string cmd, string[] args, Player target)
        {
            if (args.Length < 1)
            {
                CommandManager.SendMessage(sender, @"Use: 
/level <char_id> <level> - Update specific character level
/level <level>          - Update all characters to level");
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
                    UpdateAllCharacters(sender, target, level);
                }
                else
                {
                    string charId = args[0];
                    if (!int.TryParse(args[1], out int level))
                    {
                        CommandManager.SendMessage(sender, "Invalid level number");
                        return;
                    }
                    UpdateSingleCharacter(sender, target, charId, level);
                }
            }
            catch (Exception err)
            {
                CommandManager.SendMessage(sender, $"An error occurred: {err}");
            }
        }

        private static void UpdateAllCharacters(Player sender, Player target, int level)
        {
            level = Math.Max(1, Math.Min(level, 80));

            int updatedCount = 0;
            foreach (var item in ResourceManager.itemTable)
            {
                if (item.Key.StartsWith("chr_"))
                {
                    AddOrUpdateCharacter(target, item.Key, level);
                    updatedCount++;
                }
            }

            target.SaveCharacters();
            CommandManager.SendMessage(sender, $"Updated {updatedCount} characters to level {level}");
        }

        private static void UpdateSingleCharacter(Player sender, Player target, string charId, int level)
        {
            level = Math.Max(1, Math.Min(level, 80));

            if (!ResourceManager.itemTable.ContainsKey(charId))
            {
                CommandManager.SendMessage(sender, $"Invalid character ID: {charId}");
                return;
            }

            AddOrUpdateCharacter(target, charId, level);
            target.SaveCharacters();
            CommandManager.SendMessage(sender, $"Character {charId} level updated to {level}");
        }

        private static Character AddOrUpdateCharacter(Player target, string charId, int level)
        {
            Character existingChar = target.chars.Find(c => c.id == charId);
            if (existingChar != null)
            {
                existingChar.level = level;
                existingChar.breakNode = GetBreakNodeByLevel(level);

                target.Send(new PacketScCharBagAddChar(target, existingChar));
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