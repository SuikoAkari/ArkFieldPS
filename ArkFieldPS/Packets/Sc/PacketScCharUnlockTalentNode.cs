using ArkFieldPS.Game.Character;
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
