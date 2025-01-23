using EndFieldPS.Network;
using EndFieldPS.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace EndFieldPS.Packets.Sc
{
    public class PacketScSyncWallet : Packet
    {

        public PacketScSyncWallet(Player client) {

            ScSyncWallet proto = new ScSyncWallet()
            {
                MoneyList =
                {
                    new MoneyInfo()
                    {
                        Id="item_diamond",
                        Amount=(ulong)client.inventoryManager.item_diamond_amt,
                    },
                    new MoneyInfo()
                    {
                        Id="item_gold",
                        Amount=(ulong)client.inventoryManager.item_gold_amt,
                    }
                }
                
            };

            SetData(ScMessageId.ScSyncWallet, proto);
        }

    }
}
