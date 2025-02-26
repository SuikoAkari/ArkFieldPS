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
    public class HandleCsBitsetRemove
    {

        [Server.Handler(CsMessageId.CsBitsetRemove)]
        public static void Handle(Player session, CsMessageId cmdId, Packet packet)
        {
            CsBitsetRemove req = packet.DecodeBody<CsBitsetRemove>();
            foreach (var item in req.Value)
            {
                session.bitsetManager.RemoveValue((BitsetType)req.Type, (int)item);
            }
            session.Send(new PacketScBitsetRemove(session,req.Type,req.Value.ToList()));    

        }
       
    }
}
