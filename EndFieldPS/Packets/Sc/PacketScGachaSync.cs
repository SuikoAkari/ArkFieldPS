using EndFieldPS.Game.Gacha;
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
                (int fiveStarPity, int sixStarPity, GachaTransaction? lastSixStar, bool isFiftyFiftyLost)
                PityInfo = client.gachaManager.GetCurrentPity(item.Value.id);
                proto.CharGachaPool.GachaPoolInfos.Add(new ScdGachaPoolInfo()
                {
                    GachaPoolId = item.Value.id,
                    IsClosed=false,
                    CloseTime= DateTime.UtcNow.AddDays(20).ToUnixTimestampMilliseconds()/1000,
                    OpenTime= DateTime.UtcNow.ToUnixTimestampMilliseconds() / 1000,
                    PublicCloseReason=0,
                    
                    
                });
                proto.CharGachaPool.GachaPoolRoleDatas.Add(new ScdGachaPoolRoleData()
                {
                    GachaPoolId=item.Value.id,
                    IsClosed=false,
                    PersonalCloseReason=0,
                    SoftGuaranteeProgress=PityInfo.sixStarPity,
                    TotalPullCount = PityInfo.sixStarPity,
                    Star5SoftGuaranteeProgress = PityInfo.fiveStarPity,
                    HardGuaranteeProgress = PityInfo.sixStarPity,
                    
                });
                proto.CharGachaPool.GachaPoolCategoryRoleDatas.Add(new ScdGachaPoolCategoryRoleData()
                {
                    GachaPoolType=item.Value.type,
                    TotalPullCount = PityInfo.sixStarPity,
                    Star5SoftGuaranteeProgress = PityInfo.fiveStarPity,
                    SoftGuaranteeProgress = PityInfo.sixStarPity,
                    
                });
            }

            SetData(ScMessageId.ScGachaSync, proto);
        }

    }
}
