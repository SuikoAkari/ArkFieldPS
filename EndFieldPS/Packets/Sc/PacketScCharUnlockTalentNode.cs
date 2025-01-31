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
    public class PacketScCharUnlockTalentNode : Packet
    {

        public PacketScCharUnlockTalentNode(Player client, Character character, string nodeId) {

            ScCharUnlockTalentNode proto = new ScCharUnlockTalentNode()
            {
                CharObjId=character.guid,
                NodeId= nodeId,
            };

            SetData(ScMessageId.ScCharUnlockTalentNode, proto);
        }

    }
}
