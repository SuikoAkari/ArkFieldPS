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
    public class PacketScFactoryOpRet : Packet
    {

        public PacketScFactoryOpRet(Player client,CsFactoryOp op) {

            ScFactoryOpRet proto = new ScFactoryOpRet()
            {
                RetCode=0,
            };
            
            SetData(ScMessageId.ScFactoryOpRet, proto);
        }

    }
}
