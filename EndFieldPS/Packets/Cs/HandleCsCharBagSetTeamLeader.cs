using EndFieldPS.Network;
using EndFieldPS.Packets.Sc;
using EndFieldPS.Protocol;
using Google.Protobuf;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Parameters;
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
    public class HandleCsCharBagSetTeamLeader
    {

        [Server.Handler(CsMessageId.CsCharBagSetTeamLeader)]
        public static void Handle(Player session, CsMessageId cmdId, Packet packet)
        {
            CsCharBagSetTeamLeader req = packet.DecodeBody<CsCharBagSetTeamLeader>();

            session.teams[req.TeamIndex].leader=req.Leaderid;
            ScCharBagSetTeamLeader rsp = new()
            {
                Leaderid = req.Leaderid,
                TeamIndex = req.TeamIndex,
                ScopeName = 1,
                TeamType = req.TeamType,
            };
            session.Send(ScMessageId.ScCharBagSetTeamLeader, rsp);
            
        }
       
    }
}
