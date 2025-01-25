using EndFieldPS.Network;
using EndFieldPS.Packets.Sc;
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
    public class HandleCsSceneRepatriate
    {

        [Server.Handler(CsMessageId.CsSceneRepatriate)]
        public static void Handle(Player session, CsMessageId cmdId, Packet packet)
        {
            CsSceneRepatriate req = packet.DecodeBody<CsSceneRepatriate>();
            //TODO repatriate to actual repatriate position (probably need full level data)
            session.EnterScene(req.SceneNumId);

        }
       
    }
}
