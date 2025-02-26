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
    public class HandleCsSceneMoveStateSet
    {
        
        [Server.Handler(CsMessageId.CsSceneMoveStateSet)]
        public static void Handle(Player session, CsMessageId cmdId, Packet packet)
        {
            CsSceneMoveStateSet req = packet.DecodeBody<CsSceneMoveStateSet>();

            //req.

        }
       
    }
}
