using EndFieldPS.Network;
using EndFieldPS.Protocol;
using EndFieldPS.Resource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static EndFieldPS.Resource.ResourceManager;

namespace EndFieldPS.Packets.Sc
{
    public class PacketScSyncAllUnlock : Packet
    {

        public PacketScSyncAllUnlock(Player client) {

            ScSyncAllUnlock unlock = new()
            {

            };
           /* foreach (var item in gameSystemConfigTable)
            {
                unlock.UnlockSystems.Add(item.Value.unlockSystemType);
            }
            foreach (var item in systemJumpTable)
            {
                unlock.UnlockSystems.Add(item.Value.bindSystem);
            }
            foreach (UnlockSystemType unlockType in System.Enum.GetValues(typeof(UnlockSystemType)))
            {
                // unlock.UnlockSystems.Add((int)unlockType);
            }*/
            unlock.UnlockSystems.Add((int)UnlockSystemType.Watch);
            unlock.UnlockSystems.Add((int)UnlockSystemType.Weapon);
            unlock.UnlockSystems.Add((int)UnlockSystemType.Equip);
            unlock.UnlockSystems.Add((int)UnlockSystemType.EquipEnhance);
            unlock.UnlockSystems.Add((int)UnlockSystemType.NormalAttack);
            unlock.UnlockSystems.Add((int)UnlockSystemType.NormalSkill);
            unlock.UnlockSystems.Add((int)UnlockSystemType.UltimateSkill);
            unlock.UnlockSystems.Add((int)UnlockSystemType.TeamSkill);
            unlock.UnlockSystems.Add((int)UnlockSystemType.ComboSkill);
            unlock.UnlockSystems.Add((int)UnlockSystemType.TeamSwitch);
            unlock.UnlockSystems.Add((int)UnlockSystemType.Dash);
            unlock.UnlockSystems.Add((int)UnlockSystemType.Jump);
            unlock.UnlockSystems.Add((int)UnlockSystemType.Friend);
            unlock.UnlockSystems.Add((int)UnlockSystemType.SNS);
            unlock.UnlockSystems.Add((int)UnlockSystemType.Settlement);
            unlock.UnlockSystems.Add((int)UnlockSystemType.Map);
            unlock.UnlockSystems.Add((int)UnlockSystemType.FacZone);
            unlock.UnlockSystems.Add((int)UnlockSystemType.FacHub);
            unlock.UnlockSystems.Add((int)UnlockSystemType.AdventureBook);
            unlock.UnlockSystems.Add((int)UnlockSystemType.FacSystem);
            unlock.UnlockSystems.Add((int)UnlockSystemType.CharUI);
            unlock.UnlockSystems.Add((int)UnlockSystemType.EquipProduce);
            unlock.UnlockSystems.Add((int)UnlockSystemType.EquipTech);
            unlock.UnlockSystems.Add((int)UnlockSystemType.Gacha);
            unlock.UnlockSystems.Add((int)UnlockSystemType.Inventory);
            unlock.UnlockSystems.Add((int)UnlockSystemType.ItemQuickBar);
            unlock.UnlockSystems.Add((int)UnlockSystemType.ItemSubmitRecycle);
            unlock.UnlockSystems.Add((int)UnlockSystemType.ItemUse);
            unlock.UnlockSystems.Add((int)UnlockSystemType.Mail);
            unlock.UnlockSystems.Add((int)UnlockSystemType.ValuableDepot);
            unlock.UnlockSystems.Add((int)UnlockSystemType.Wiki);
            unlock.UnlockSystems.Add((int)UnlockSystemType.AIBark);
            unlock.UnlockSystems.Add((int)UnlockSystemType.AdventureExpAndLv);
            //unlock.UnlockSystems.Add((int)UnlockSystemType.);
            unlock.UnlockSystems.Add((int)UnlockSystemType.CharTeam);
            unlock.UnlockSystems.Add((int)UnlockSystemType.FacMode);
            unlock.UnlockSystems.Add((int)UnlockSystemType.FacOverview);
            unlock.UnlockSystems.Add((int)UnlockSystemType.SpaceshipSystem);
            unlock.UnlockSystems.Add((int)UnlockSystemType.SpaceshipControlCenter);
            unlock.UnlockSystems.Add((int)UnlockSystemType.FacBUS);
            unlock.UnlockSystems.Add((int)UnlockSystemType.PRTS);
            unlock.UnlockSystems.Add((int)UnlockSystemType.Dungeon);
            
            SetData(ScMessageId.ScSyncAllUnlock, unlock);
        }

    }
}
