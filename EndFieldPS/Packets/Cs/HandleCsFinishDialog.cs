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
    public class HandleCsFinishDialog
    {
        
        [Server.Handler(CsMessageId.CsFinishDialog)]
        public static void Handle(Player session, CsMessageId cmdId, Packet packet)
        {
            CsFinishDialog req = packet.DecodeBody<CsFinishDialog>();
            session.Send(ScMessageId.ScFinishDialog, new ScFinishDialog()
            {
                DialogId=req.DialogId,
                FinishNums = { req.FinishNums },
                OptionIds = { req.OptionIds },
            });
        }
       
    }
}
