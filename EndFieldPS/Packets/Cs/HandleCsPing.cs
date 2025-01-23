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
            session.Send(Packet.EncodePacket((int)ScMessageId.ScPing, new ScPing()
            {
                ClientTs = req.ClientTs,
                ServerTs = (ulong)DateTime.UtcNow.Ticks,
            }));

            session.Send(Packet.EncodePacket((int)ScMessageId.ScFactoryHs, new ScFactoryHs()
            {
                Blackboard = new()
                {
                    Power = new()
                    {
                        PowerSaveCurrent=100,
                        PowerSaveMax=200,
                        PowerGenLastSec=2,
                        PowerCostSum=1
                    },
                    
                },
                ChapterId="domain_02",
                FbList =
                {
   
                },
                CtList =
                {
    
                },
                Tms= DateTime.UtcNow.Ticks,
            }));
        }
       
    }
}
