using ArkFieldPS.Game.Inventory;
using ArkFieldPS.Network;
using ArkFieldPS.Protocol;
using ArkFieldPS.Resource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ArkFieldPS.Packets.Sc
{
    public class PacketScItemBagScopeSync : Packet
    {
        public PacketScItemBagScopeSync(Player client,ItemValuableDepotType type) {
            
            ScItemBagScopeSync proto = new ScItemBagScopeSync()
            {
                Bag = new()
                {
                    GridLimit = 30,
                    Grids =
                    {
                        new ScdItemGrid()
                        {
                            GridIndex=0,
                            Count=1,
                            Id="item_port_power_pole_2",
                            Inst = new()
                            {
                                InstId=300000000000,

                            },

                        },
                        new ScdItemGrid()
                        {
                            GridIndex=1,
                            Count=1,
                            Id="item_port_travel_pole_1",
                            Inst = new()
                            {
                                InstId=300000000001,

                            },

                        },
                        new ScdItemGrid()
                        {
                            GridIndex=2,
                            Count=1,
                            Id="item_port_grinder_1",
                            Inst = new()
                            {
                                InstId=300000000002,

                            },

                        },
                        new ScdItemGrid()
                        {
                            GridIndex=3,
                            Count=1,
                            Id="item_port_sp_hub_1",
                            Inst = new()
                            {
                                InstId=300000000003,

                            },
                        }
                    }
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

                    if (proto.Depot[(int)i].StackableItems.ContainsKey(item.id))
                    {
                        proto.Depot[(int)i].StackableItems[item.id]+= item.amount;
                    }
                    else
                    {
                        proto.Depot[(int)i].StackableItems.Add(item.id, item.amount);
                    }
                    
            
                }
            });
            
           // Logger.Print(proto.ToString());
            SetData(ScMessageId.ScItemBagScopeSync, proto);
        }

    }
}
