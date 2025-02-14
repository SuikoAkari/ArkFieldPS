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
    public class PacketScSetName: Packet
    {
        public PacketScSetName(Player player, string nickname) {
            ScSetName proto = new ScSetName() {
                Name = nickname,
                ShortId = player.accountId
            };

            SetData(ScMessageId.ScSetName, proto);
        }
    }
}
