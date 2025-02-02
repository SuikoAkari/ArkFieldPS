using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EndFieldPS.Database;
using EndFieldPS.Game.Character;
using EndFieldPS.Packets.Sc;

namespace EndFieldPS.Commands.Handlers;

public static class CommandRemove
{
        [Server.Command("remove", "Removes character", true)]
        public static void Handle(Player sender, string cmd, string[] args, Player target)
        {
            if(args.Length < 1) 
            {
                CommandManager.SendMessage(sender, "Use: /remove (char id)");
                return;
            }
            
            Character character = target.chars.Find(c => c.id == args[0]);
            if(character == null) 
            {
                CommandManager.SendMessage(sender, "Character not found");
                return;
            }

            target.chars.Remove(character);
            DatabaseManager.db.DeleteCharacter(character);
            target.SaveCharacters();
            target.Send(new PacketScCharBagDelChar(target, character));
            CommandManager.SendMessage(sender, $"Character {character.id} removed.");
        }
}
