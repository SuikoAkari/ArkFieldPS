using ArkFieldPS.Network;
using ArkFieldPS.Protocol;
using ArkFieldPS.Resource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static ArkFieldPS.Resource.ResourceManager;

namespace ArkFieldPS.Packets.Sc
{
    public class PacketScSyncAllUnlock : Packet
    {

        public PacketScSyncAllUnlock(Player client) {

            ScSyncAllUnlock unlock = new()
            {
                UnlockSystems = {client.unlockedSystems},
                
            };
            
            SetData(ScMessageId.ScSyncAllUnlock, unlock);
        }

    }
}
