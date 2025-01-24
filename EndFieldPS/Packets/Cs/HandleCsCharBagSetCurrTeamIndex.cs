using BeyondTools.VFS.Crypto;
using EndFieldPS.Network;
using EndFieldPS.Packets.Sc;
using EndFieldPS.Protocol;
using EndFieldPS.Resource;
using Google.Protobuf;
using Google.Protobuf.WellKnownTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;

using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static EndFieldPS.Resource.ResourceManager;
using static System.Net.Mime.MediaTypeNames;

namespace EndFieldPS.Packets.Cs
{
    public class HandleCsCharBagSetCurrTeamIndex
    {
        [Server.Handler(CsMessageId.CsCharBagSetCurrTeamIndex)]
        public static void Handle(Player session, CsMessageId cmdId, Packet packet)
        {
            CsCharBagSetCurrTeamIndex req = packet.DecodeBody<CsCharBagSetCurrTeamIndex>();
            session.teamIndex = req.TeamIndex;
            session.teams[session.teamIndex].leader = req.LeaderId;
            
            session.Send(new PacketScCharBagSetCurrTeamIndex(session));
            session.Send(new PacketScSelfSceneInfo(session, true));
        }
    }
}
