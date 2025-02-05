using EndFieldPS.Network;
using EndFieldPS.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace EndFieldPS.Packets.Sc
{
    public class PacketScSyncGameMode : Packet
    {

        public PacketScSyncGameMode(Player client, string gamemode) {

            ScSyncGameMode proto = new ScSyncGameMode()
            {
                ModeId=gamemode,
                
            };

            SetData(ScMessageId.ScSyncGameMode, proto);
        }

    }
}
