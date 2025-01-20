using EndFieldPS.Network;
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
    public class HandleCsPing
    {

        [Server.Handler(CsMessageId.CsPing)]
        public static void Handle(Client session, CsMessageId cmdId, Packet packet)
        {
            CsPing req = packet.DecodeBody<CsPing>();
            session.Send(Packet.EncodePacket((int)ScMessageId.ScPing, new ScPing()
            {
                ClientTs = req.ClientTs,
                ServerTs = (ulong)DateTime.UtcNow.Ticks,
            }));

            /*session.Send(Packet.EncodePacket((int)ScMessageId.ScFactoryHs, new ScFactoryHs()
            {
                Blackboard = new()
                {
                    Power = new()
                    {
                        
                    },
                    
                },
                
                Tms= DateTime.UtcNow.Ticks,
            }));*/
        }
       
    }
}
