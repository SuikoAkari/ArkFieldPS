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
    public class PacketScBitsetRemove : Packet
    {

        public PacketScBitsetRemove(Player client, int type, List<uint> values) {

            ScBitsetRemove proto = new()
            {
                Type = type,
                Value =
                {
                    values
                }
            };
            
            SetData(ScMessageId.ScBitsetRemove, proto);
        }

    }
}
