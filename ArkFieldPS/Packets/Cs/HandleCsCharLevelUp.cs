using ArkFieldPS.Game.Character;
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
    public class HandleCsCharLevelUp
    {

        [Server.Handler(CsMessageId.CsCharLevelUp)]
        public static void Handle(Player session, CsMessageId cmdId, Packet packet)
        {
            CsCharLevelUp req = packet.DecodeBody<CsCharLevelUp>();

            Character character = session.chars.Find(c=>c.guid==req.CharObjID);
            if(character!=null)
            character.LevelUp(req.Items);

        }
       
    }
}
