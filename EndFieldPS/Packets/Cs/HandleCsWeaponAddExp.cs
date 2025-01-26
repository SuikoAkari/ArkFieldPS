using EndFieldPS.Game.Character;
using EndFieldPS.Game.Inventory;
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
    public class HandleCsWeaponAddExp
    {

        [Server.Handler(CsMessageId.CsWeaponAddExp)]
        public static void Handle(Player session, CsMessageId cmdId, Packet packet)
        {
            CsWeaponAddExp req = packet.DecodeBody<CsWeaponAddExp>();
            
            Item item = session.inventoryManager.items.Find(c=>c.guid==req.Weaponid);
            if(item != null)
            item.LevelUp(req.CostItemId2Count,req.CostWeaponIds);

        }
       
    }
}
