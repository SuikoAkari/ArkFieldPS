using EndFieldPS.Game.Character;
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
    public class PacketScCharBagAddChar : Packet
    {

        public PacketScCharBagAddChar(Player client,Character chara) {

            ScCharBagAddChar proto = new ScCharBagAddChar()
            {
                Char = chara.ToProto(),
                ScopeName=1,
            };

            SetData(ScMessageId.ScCharBagAddChar, proto);
        }

    }
}
