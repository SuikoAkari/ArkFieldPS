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
    public class PacketScCharBagSetTeamName : Packet
    {

        public PacketScCharBagSetTeamName(Player client)
        {
            ScCharBagSetTeamName proto = new()
            {
                TeamIndex = client.teamIndex,
                TeamName = client.teams[client.teamIndex].name,
                ScopeName = 1,
            };
            SetData(ScMessageId.ScCharBagSetTeamName, proto);
        }
    }
}
