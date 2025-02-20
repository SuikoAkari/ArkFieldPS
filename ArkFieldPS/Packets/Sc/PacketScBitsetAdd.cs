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
    public class PacketScBitsetAdd : Packet
    {

        public PacketScBitsetAdd(Player client, int type, List<uint> values) {

            ScBitsetAdd proto = new()
            {
                Type = type,
                Value =
                {
                    values
                }
            };
            
            SetData(ScMessageId.ScBitsetAdd, proto);
        }

    }
}
