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
    public class HandleCsPing
    {

        [Server.Handler(CsMessageId.CsPing)]
        public static void Handle(Player session, CsMessageId cmdId, Packet packet)
        {
            CsPing req = packet.DecodeBody<CsPing>();
            long curtimestamp = DateTime.UtcNow.ToUnixTimestampMilliseconds();

            session.Send(Packet.EncodePacket((int)ScMessageId.ScPing, new ScPing()
            {
                ClientTs = req.ClientTs,
                ServerTs = (ulong)curtimestamp,
            }));
            //Logger.Print("Server: " + curtimestamp + " client: " + req.ClientTs);
        }
       
    }
}
