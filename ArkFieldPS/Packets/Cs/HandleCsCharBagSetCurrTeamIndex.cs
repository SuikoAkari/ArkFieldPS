using BeyondTools.VFS.Crypto;
using ArkFieldPS.Network;
using ArkFieldPS.Packets.Sc;
using ArkFieldPS.Protocol;
using ArkFieldPS.Resource;
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
using static ArkFieldPS.Resource.ResourceManager;
using static System.Net.Mime.MediaTypeNames;

namespace ArkFieldPS.Packets.Cs
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
            session.Send(new PacketScSelfSceneInfo(session,SelfInfoReasonType.SlrChangeTeam));
        }
    }
}
