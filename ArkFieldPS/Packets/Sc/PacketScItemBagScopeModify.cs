using ArkFieldPS.Game.Inventory;
using ArkFieldPS.Network;
using ArkFieldPS.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ArkFieldPS.Packets.Sc
{
    public class PacketScItemBagScopeModify : Packet
    {

        public PacketScItemBagScopeModify(Player client, Item item) {

            ScItemBagScopeModify proto = new ScItemBagScopeModify()
            {
                Depot =
                {
                    {(int)item.ItemType,new ScdItemDepotModify(){
                        
                    }
                    }
                },
                ScopeName=1,
                
            };
            if (item.InstanceType())
            {
                if (item.amount <= 0)
                {
                    proto.Depot[(int)item.ItemType].DelInstList.Add(item.guid);
                }
                else
                {
                    proto.Depot[(int)item.ItemType].InstList.Add(item.ToProto());
                }
                
            }
            else
            {
                proto.Depot[(int)item.ItemType].Items.Add(item.id, item.amount);
                
            }

            SetData(ScMessageId.ScItemBagScopeModify, proto);
        }

    }
}
