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
        //Weapon only for now
        public PacketScItemBagScopeSync(EndminPlayer client) {

            ScItemBagScopeSync proto = new ScItemBagScopeSync()
            {
                Bag = new()
                {
                    GridLimit = 30,

                },
                
                ScopeName = 1,
                Depot = 
                { 
                    {(int)ItemValuableDepotType.Weapon,
                        new ScdItemDepot()
                        {
                            InstList =
                            {
                        


                            }
                        } 
                    },
                    {(int)ItemValuableDepotType.SpecialItem,
                        new ScdItemDepot()
                        {
                            
                        }
                    },
                    {(int)ItemValuableDepotType.CommercialItem,
                        new ScdItemDepot()
                        {

                        }
                    },
                    {(int)ItemValuableDepotType.Factory,
                        new ScdItemDepot()
                        {

                        }
                    }

                }

            };
            client.inventoryManager.weapons.ForEach(w => 
            {
                proto.Depot[1].InstList.Add(w.ToProto());
            });

            client.inventoryManager.items.ForEach(i =>
            {
                if (proto.Depot.ContainsKey((int)i.ItemType) && i.ItemType != ItemValuableDepotType.Equip && i.ItemType != ItemValuableDepotType.WeaponGem)
                {
                    proto.Depot[(int)i.ItemType].StackableItems.Add(i.id,i.amount);
                    
                }
            });
            Server.Print(proto.ToString());
            SetData(ScMessageId.ScItemBagScopeSync, proto);
        }

    }
}
