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
    public class HandleCsSceneSetTrackPoint
    {

        [Server.Handler(CsMessageId.CsSceneSetTrackPoint)]
        public static void Handle(Player session, CsMessageId cmdId, Packet packet)
        {
            CsSceneSetTrackPoint req = packet.DecodeBody<CsSceneSetTrackPoint>();

            session.Send(new PacketScSceneSetTrackPoint(req.TrackPoint));
        }
    }
}
