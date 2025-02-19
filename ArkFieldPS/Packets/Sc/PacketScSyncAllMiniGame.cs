using ArkFieldPS.Network;
using ArkFieldPS.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ArkFieldPS.Packets.Sc
{
    public class PacketScSyncAllMiniGame : Packet
    {

        public PacketScSyncAllMiniGame(Player client) {

            ScSyncAllMiniGame proto = new ScSyncAllMiniGame();
            SetData(ScMessageId.ScSyncAllMiniGame, proto);
        }

    }
}
