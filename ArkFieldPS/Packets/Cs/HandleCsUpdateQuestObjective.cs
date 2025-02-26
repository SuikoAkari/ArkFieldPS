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
    public class HandleCsUpdateQuestObjective
    {
        
        [Server.Handler(CsMessageId.CsUpdateQuestObjective)]
        public static void Handle(Player session, CsMessageId cmdId, Packet packet)
        {
            CsUpdateQuestObjective req = packet.DecodeBody<CsUpdateQuestObjective>();
            ScQuestObjectivesUpdate u = new()
            {
                QuestId = req.QuestId,
                QuestObjectives =
                {
                    
                }
            };
            session.Send(ScMessageId.ScQuestObjectivesUpdate, u);
        }
       
    }
}
