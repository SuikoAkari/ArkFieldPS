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
