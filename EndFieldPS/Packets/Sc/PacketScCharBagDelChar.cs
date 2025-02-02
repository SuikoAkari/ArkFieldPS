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
    public class PacketScCharBagDelChar : Packet
    {
        public PacketScCharBagDelChar(Player player, Character character)
        {
            ScCharBagDelChar proto = new ScCharBagDelChar()
            {
                CharInstId = character.guid,
                ScopeName = 1,
            };

            SetData(ScMessageId.ScCharBagDelChar, proto);
        }
    }
}
