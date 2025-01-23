using EndFieldPS.Network;
using EndFieldPS.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static EndFieldPS.Resource.ResourceManager;

namespace EndFieldPS.Packets.Sc
{
    public class PacketScGachaSync : Packet
    {

        public PacketScGachaSync(Player client) {

            ScGachaSync proto = new ScGachaSync()
            {
                CharGachaPool = new()
                {
                    GachaPoolInfos =
                    {

                    }
                }
            };
            //TODO: Implement banner config for opentime etc
            foreach (var item in gachaCharPoolTable)
            {
                proto.CharGachaPool.GachaPoolInfos.Add(new ScdGachaPoolInfo()
                {
                    GachaPoolId = item.Value.id,
                    
                });
            }

            SetData(ScMessageId.ScGachaSync, proto);
        }

    }
}
