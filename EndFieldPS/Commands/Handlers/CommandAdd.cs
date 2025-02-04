using EndFieldPS.Database;
using EndFieldPS.Game.Character;
using EndFieldPS.Packets.Sc;
using EndFieldPS.Protocol;
using EndFieldPS.Resource;
using System.Text;

namespace EndFieldPS.Commands.Handlers
{
    public static class CommandAddAll
    {
        [Server.Command("addall", "Adds all items and weapons", true)]
        public static void Handle(Player sender, string cmd, string[] args, Player target)
        {
            int amount = 1;
            int level = 80;

            if (args.Length > 0)
            {
                int.TryParse(args[0], out amount);
                amount = Math.Max(1, Math.Min(amount, 1000000));
            }

            try
            {
                int addedCount = 0;
                StringBuilder result = new StringBuilder();

                foreach (var item in ResourceManager.itemTable)
                {
                    try
                    {
                        if (item.Key.StartsWith("wpn_"))
                        {
                            var weapon = target.inventoryManager.AddWeapon(item.Key, (ulong)level);
                            target.Send(new PacketScItemBagScopeModify(target, weapon));
                            addedCount++;
                        }
                        else if (!item.Key.StartsWith("chr_"))
                        {
                            var newItem = target.inventoryManager.AddItem(item.Key, amount);
                            target.Send(new PacketScItemBagScopeModify(target, newItem));
                            addedCount++;
                        }
                    }
                    catch (Exception itemErr)
                    {
                        result.AppendLine($"Failed to add {item.Key}: {itemErr.Message}");
                    }
                }

                target.inventoryManager.Save();

                target.unlockedSystems.Clear();
                target.UnlockImportantSystems();
                target.maxDashEnergy = 250;
                foreach (var system in target.unlockedSystems)
                {
                    ScUnlockSystem unlocked = new()
                    {
                        UnlockSystemType = system,
                    };
                    target.Send(ScMessageId.ScUnlockSystem, unlocked);
                }

                result.Insert(0, $"Successfully added {addedCount} items to {target.nickname}.\n");
                CommandManager.SendMessage(sender, result.ToString());
            }
            catch (Exception err)
            {
                CommandManager.SendMessage(sender, $"An error occurred: {err}");
            }
        }
    }
}