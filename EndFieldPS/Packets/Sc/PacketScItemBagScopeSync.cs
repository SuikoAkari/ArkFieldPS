using EndFieldPS.Game.Inventory;
using EndFieldPS.Network;
using EndFieldPS.Protocol;
using EndFieldPS.Resource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace EndFieldPS.Packets.Sc
{
    public class PacketScItemBagScopeSync : Packet
    {
        public PacketScItemBagScopeSync(Player client,ItemValuableDepotType type) {
            
            ScItemBagScopeSync proto = new ScItemBagScopeSync()
            {
                Bag = new()
                {
                    GridLimit = 30,
                    
                },
                FactoryDepot =
                {
                    {"domain_1", 
                        new ScdItemDepot()
                        {
                            
                        } 
                    },
                    {"domain_2",
                        new ScdItemDepot()
                        {

                        }
                    }
                },
                ScopeName = 1,
                Depot = 
                { 

                },
                

            };
            
            //All depots type from 1 to 10
            int i = (int)type;
            if(i > 1)
            {
                proto.FactoryDepot.Clear();
                proto.Bag = null;
            }
            proto.Depot.Add(i, new ScdItemDepot());
            List<Item> items = client.inventoryManager.items.FindAll(item => item.ItemType == (ItemValuableDepotType)i);
            items.ForEach(item =>
            {
                if (item.InstanceType())
                {
                    proto.Depot[i].InstList.Add(item.ToProto());
                }
                else
                {
                    proto.Depot[(int)i].StackableItems.Add(item.id, item.amount);
                }
            });
            
           // Logger.Print(proto.ToString());
            SetData(ScMessageId.ScItemBagScopeSync, proto);
        }

    }
}
