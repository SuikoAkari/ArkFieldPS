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

                    },
                    GachaPoolRoleDatas =
                    {

                    },
                    GachaPoolCategoryRoleDatas =
                    {
                        
                    }
                },
                WeaponGachaPool = new()
                {
                    
                }
            };
            //TODO: Implement banner config for opentime etc
            foreach (var item in gachaCharPoolTable)
            {
                proto.CharGachaPool.GachaPoolInfos.Add(new ScdGachaPoolInfo()
                {
                    GachaPoolId = item.Value.id,
                    IsClosed=false,
                    CloseTime=0,
                    OpenTime=0,
                    PublicCloseReason=0,
                    
                    
                });
                proto.CharGachaPool.GachaPoolRoleDatas.Add(new ScdGachaPoolRoleData()
                {
                    GachaPoolId=item.Value.id,
                    IsClosed=false,
                    PersonalCloseReason=0,
                    
                });
                proto.CharGachaPool.GachaPoolCategoryRoleDatas.Add(new ScdGachaPoolCategoryRoleData()
                {
                    GachaPoolType=item.Value.type,
                    
                });
            }

            SetData(ScMessageId.ScGachaSync, proto);
        }

    }
}
