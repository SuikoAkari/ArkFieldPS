using ArkFieldPS.Game.Character;
using ArkFieldPS.Network;
using ArkFieldPS.Packets.Sc;
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
    public class HandleCsCharPotentialUnlock
    {

        [Server.Handler(CsMessageId.CsCharPotentialUnlock)]
        public static void Handle(Player session, CsMessageId cmdId, Packet packet)
        {
            CsCharPotentialUnlock req = packet.DecodeBody<CsCharPotentialUnlock>();
            
            Character character = session.chars.Find(c => c.guid == req.CharObjId);
            if (character != null)
            {
                character.potential=req.Level;
                //TODO consume Item ID

                ScCharPotentialUnlock unlock = new()
                {
                    CharObjId = req.CharObjId,
                    Level = req.Level,
                };
                session.Send(ScMessageId.ScCharPotentialUnlock, unlock);
            }
           
        }
       
    }
}
