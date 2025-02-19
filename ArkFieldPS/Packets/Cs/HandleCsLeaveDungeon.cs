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
    public class HandleCsLeaveDungeon
    {
        
        [Server.Handler(CsMessageId.CsLeaveDungeon)]
        public static void Handle(Player session, CsMessageId cmdId, Packet packet)
        {
            CsLeaveDungeon req = packet.DecodeBody<CsLeaveDungeon>();

            session.LeaveDungeon(req);

        }
       
    }
}
