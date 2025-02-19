using ArkFieldPS.Game.Character;
using ArkFieldPS.Game.Inventory;
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
