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
    public class PacketScCharBagSetCurrTeamIndex : Packet
    {

        public PacketScCharBagSetCurrTeamIndex(Player client) {

            CsCharBagSetCurrTeamIndex proto = new()
            {
                LeaderId = client.teams[client.teamIndex].leader,
                TeamIndex=client.teamIndex,
            };
            SetData(ScMessageId.ScCharBagSetCurrTeamIndex, proto);
        }

    }
}
