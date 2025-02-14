using ArkFieldPS.Game.Entities;
using ArkFieldPS.Network;
using ArkFieldPS.Protocol;
using ArkFieldPS.Resource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ArkFieldPS.Packets.Sc
{
    public class PacketScObjectLeaveView : Packet
    {

        public PacketScObjectLeaveView(Player session, ulong guid) {

            ScObjectLeaveView proto = new()
            {
                
            };
            proto.ObjList.Add(new LeaveObjectInfo()
            {
                ObjId = guid,
                
            });

            SetData(ScMessageId.ScObjectLeaveView, proto);
        }

    }
}
