using ArkFieldPS.Network;
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
            /*ScFactoryHsSync s = new()
            {
                Blackboard = new()
                {
                    InventoryNodeId = 0,
                    Power = new()
                    {
                        
                    }
                },
                CcList =
                {

                },
                Tms = curtimestamp / 1000,
                ChapterId = session.GetCurrentChapter()
            };
            
            session.Send(ScMessageId.ScFactoryHsSync,s);*/

            //Logger.Print("Server: " + curtimestamp + " client: " + req.ClientTs);
        }
       
    }
}
