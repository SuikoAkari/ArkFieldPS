using ArkFieldPS.Game.Character;
using ArkFieldPS.Network;
using ArkFieldPS.Packets.Sc;
using ArkFieldPS.Protocol;
using Google.Protobuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ArkFieldPS.Packets.Cs
{
    public class HandleCsCharUnlockTalentNode
    {
        [Server.Handler(CsMessageId.CsCharUnlockTalentNode)]
        public static void Handle(Player session, CsMessageId cmdId, Packet packet)
        {
            CsCharUnlockTalentNode req = packet.DecodeBody<CsCharUnlockTalentNode>();
            
            Character character = session.chars.Find(c=>c.guid==req.CharObjId);
            if (character != null)
            {
                character.UnlockNode(req.NodeId);             
            }
        }
       
    }
}
