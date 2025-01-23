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
    public class HandleCsCharLevelUp
    {

        [Server.Handler(CsMessageId.CsCharLevelUp)]
        public static void Handle(Player session, CsMessageId cmdId, Packet packet)
        {
            CsCharLevelUp req = packet.DecodeBody<CsCharLevelUp>();
            


        }
       
    }
}
