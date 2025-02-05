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
    public class PacketScSyncAllRoleScene : Packet
    {

        public PacketScSyncAllRoleScene(Player client) {

            ScSyncAllRoleScene role = new ScSyncAllRoleScene()
            {
                SceneGradeInfo =
                {

                },
                UnlockAreaInfo =
                {

                },
                SubmitEtherCount = 12,
                SubmitEtherLevel = 5,

            };
            foreach (var scene in levelDatas)
            {
                role.SceneGradeInfo.Add(new SceneGradeInfo()
                {
                    Grade=1,
                    LastDownTs= DateTime.UtcNow.ToUnixTimestampMilliseconds(),
                    SceneNumId=scene.idNum,
                    
                });
            }
            foreach (var scene in levelDatas)
            {
                AreaUnlockInfo u = new()
                {
                    SceneId = scene.id,
                    
                    UnlockAreaId =
                    {
                        
                    }
                };
                List<SceneAreaTable> areas =sceneAreaTable.Values.ToList().FindAll(a=>a.sceneId==scene.id);
                foreach (var area in areas)
                {

                    u.UnlockAreaId.Add(area.areaId);
                }
                role.UnlockAreaInfo.Add(u);
            }
            
            SetData(ScMessageId.ScSyncAllRoleScene, role);
        }

    }
}
