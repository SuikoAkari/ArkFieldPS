using EndFieldPS.Network;
using EndFieldPS.Protocol;
using Google.Protobuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace EndFieldPS.Packets.Cs
{
    public class HandleCsCharBagSetTeamName
    {

        [Server.Handler(CsMessageId.CsCharBagSetTeamName)]

        public static void Handle(Player session, CsMessageId cmdId, Packet packet)
        {
            CsCharBagSetTeamName req = packet.DecodeBody<CsCharBagSetTeamName>();

            session.teams[req.TeamIndex].name=req.TeamName;

            ScCharBagSetTeamName rsp = new()
            {
                TeamIndex = req.TeamIndex,
                TeamName = req.TeamName,
                ScopeName = 1,
            };
            session.Send(ScMessageId.ScCharBagSetTeamName, rsp);
        }
    }
}
