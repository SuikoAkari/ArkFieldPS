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
    public class HandleCsFactoryStatisticRequire
    {
        
        [Server.Handler(CsMessageId.CsFactoryStatisticRequire)]
        public static void Handle(Player session, CsMessageId cmdId, Packet packet)
        {
            CsFactoryStatisticRequire req = packet.DecodeBody<CsFactoryStatisticRequire>();
            ScFactoryStatisticRequire rsp = new()
            {

            };
            
            session.Send(ScMessageId.ScFactoryStatisticRequire, rsp);
           
            //Logger.Print("Server: " + curtimestamp + " client: " + req.ClientTs);
        }
       
    }
}
