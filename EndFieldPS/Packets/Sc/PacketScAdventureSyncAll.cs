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
    public class PacketScAdventureSyncAll : Packet
    {

        public PacketScAdventureSyncAll(Player session) {

            ScAdventureSyncAll adventure = new()
            {
                Exp = session.xp,
                Level = (int)session.level,

            };

            SetData(ScMessageId.ScAdventureSyncAll, adventure);
        }

    }
}
