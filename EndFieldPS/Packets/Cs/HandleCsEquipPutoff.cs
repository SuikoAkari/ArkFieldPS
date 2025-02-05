using EndFieldPS.Game.Character;
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
    public class HandleCsEquipPutoff
    {

        [Server.Handler(CsMessageId.CsEquipPutoff)]
        public static void Handle(Player session, CsMessageId cmdId, Packet packet)
        {
            CsEquipPutoff req = packet.DecodeBody<CsEquipPutoff>();
            Character toRemove = session.chars.Find(c => c.guid == req.Charid);
            if (toRemove != null)
            {
                ScEquipPutoff put = new()
                {
                    Charid = req.Charid,
                    Slotid = req.Slotid,
                    
                };
                if (toRemove != null)
                {
                    toRemove.equipCol[req.Slotid] = 0;
                    
                }
                //TODO Improve all this maybe with an internal method in Character
                session.Send(ScMessageId.ScEquipPutoff, put);
            }

        }
       
    }
}
