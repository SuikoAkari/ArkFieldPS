using EndFieldPS.Game.Entities;
using EndFieldPS.Network;
using EndFieldPS.Protocol;
using EndFieldPS.Resource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace EndFieldPS.Packets.Sc
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
