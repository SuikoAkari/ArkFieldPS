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
                UnlockSystems = {client.unlockedSystems},
                
            };
            if (client.currentDungeon != null)
            {
                unlock.UnlockSystems.Clear();
                unlock.UnlockSystems.Add((int)UnlockSystemType.Jump);
                unlock.UnlockSystems.Add((int)UnlockSystemType.ComboSkill);
                unlock.UnlockSystems.Add((int)UnlockSystemType.NormalSkill);
                unlock.UnlockSystems.Add((int)UnlockSystemType.TeamSkill);
                unlock.UnlockSystems.Add((int)UnlockSystemType.UltimateSkill);
                unlock.UnlockSystems.Add((int)UnlockSystemType.AIBark);
                unlock.UnlockSystems.Add((int)UnlockSystemType.Dash);
                unlock.UnlockSystems.Add((int)UnlockSystemType.Dungeon);
                unlock.UnlockSystems.Add((int)UnlockSystemType.Watch);
            }
            SetData(ScMessageId.ScSyncAllUnlock, unlock);
        }

    }
}
