using ArkFieldPS.Game.Character;
using ArkFieldPS.Network;
using ArkFieldPS.Packets.Sc;
using ArkFieldPS.Protocol;
using ArkFieldPS.Resource;
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
    public class HandleCsBitsetAdd
    {

        [Server.Handler(CsMessageId.CsBitsetAdd)]
        public static void Handle(Player session, CsMessageId cmdId, Packet packet)
        {
            CsBitsetAdd req = packet.DecodeBody<CsBitsetAdd>();
            foreach (var item in req.Value)
            {
                session.bitsetManager.AddValue((BitsetType)req.Type, (int)item);
            }
            session.Send(new PacketScBitsetAdd(session,req.Type,req.Value.ToList()));    

        }
       
    }
}
