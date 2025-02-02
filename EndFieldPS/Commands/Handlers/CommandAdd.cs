using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EndFieldPS.Game.Character;
using EndFieldPS.Game.Inventory;
using EndFieldPS.Packets.Sc;

namespace EndFieldPS.Commands.Handlers;

public static class CommandAdd
{
    [Server.Command("add", "Adds items, weapons or characters", true)]
    public static void Handle(Player sender, string cmd, string[] args, Player target) 
    {
        if(args.Length < 2) 
        {
            CommandManager.SendMessage(sender, "Use: /add (item|weapon|char) (item/weapon/char id) (amount/lvl)");
            return;
        }

        string message = "";
        try 
        {
            switch (args[0]) 
            {
                case "item":
                    Item item=target.inventoryManager.AddItem(args[1], int.Parse(args[2]));
                    message = $"Item {args[1]} was added to {target.nickname}";

                    target.Send(new PacketScItemBagScopeModify(target, item));
                    break;

                case "weapon":
                    Item wep = target.inventoryManager.AddWeapon(args[1], Convert.ToUInt64(args[2]));
                    message = $"Weapon {args[1]} was added to {target.nickname}";

                    target.Send(new PacketScItemBagScopeModify(target, wep));
                    break;

                case "char":
                    int lvl = int.Parse(args[2]);

                    if(lvl < 1 || lvl > 80) 
                    {
                        CommandManager.SendMessage(sender, "Level can't be less than 1 or more than 80");
                        return;
                    }

                    Character character = new Character(target.roleId, args[1], lvl);

                    if(target.chars.Find(c => c.id == character.id) != null) 
                    {
                        CommandManager.SendMessage(sender, "Character already exists");
                        return;
                    }

                    if(lvl <= 20) character.breakNode = "";
                    if(lvl > 20 && lvl <= 40) character.breakNode = "charBreak20";
                    if(lvl > 40 && lvl <= 60) character.breakNode = "charBreak40";
                    if(lvl > 60 && lvl <= 70) character.breakNode = "charBreak60";
                    if(lvl > 70) character.breakNode = "charBreak70";
                    
                    target.chars.Add(character);
                    target.SaveCharacters();
                    
                    message = $"Character {character.id} was added to {target.nickname}.";
                    CommandManager.SendMessage(sender, message);
                    target.Send(new PacketScCharBagAddChar(target, character));
                    Item weapon = target.inventoryManager.items.Find(i => i.guid == character.weaponGuid);
                    if(weapon!=null)target.Send(new PacketScItemBagScopeModify(target, weapon));
                    return;
                
                default:
                    CommandManager.SendMessage(sender, "Unknown argument, use item, weapon or character");
                    return;
            }

            target.inventoryManager.Save();
            CommandManager.SendMessage(sender, $"{message}.");
        }
        catch (Exception err)
        {
            CommandManager.SendMessage(sender, $"An error occurred: {err}");
        }
    }
}
