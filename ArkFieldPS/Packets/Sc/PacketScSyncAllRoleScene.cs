using ArkFieldPS.Network;
using ArkFieldPS.Protocol;
using ArkFieldPS.Resource;
using ArkFieldPS.Resource.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static ArkFieldPS.Resource.ResourceManager;

namespace ArkFieldPS.Packets.Sc
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
                SubmitEtherCount = 0,
                SubmitEtherLevel = 1,

            };
           
            foreach (var scene in ResourceManager.levelGradeTable)
            {
                role.SceneGradeInfo.Add(new SceneGradeInfo()
                {
                    Grade=1,
                    //LastDownTs= DateTime.UtcNow.ToUnixTimestampMilliseconds()/1000,
                    SceneNumId=GetSceneNumIdFromLevelData(scene.Value.name),
                    
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

                  //  u.UnlockAreaId.Add(area.areaId);
                }
                role.UnlockAreaInfo.Add(u);
            }
            
            SetData(ScMessageId.ScSyncAllRoleScene, role);
        }

    }
}
