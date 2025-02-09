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
    public class PacketScSelfSceneInfo : Packet
    {

        public PacketScSelfSceneInfo(Player session, SelfInfoReasonType infoReason = SelfInfoReasonType.SlrEnterScene) {

            ScSelfSceneInfo sceneInfo = new()
            {
                SceneId = session.sceneManager.GetSceneGuid(session.curSceneNumId),
                SceneNumId = session.curSceneNumId,
                SelfInfoReason = (int)infoReason,
                    
                TeamInfo = new()
                {
                    CurLeaderId = session.teams[session.teamIndex].leader,
                    TeamIndex = session.teamIndex,
                    TeamType = CharBagTeamType.Main

                },
                SceneGrade = 4,
                
                Detail = new()
                {
                    TeamIndex = session.teamIndex,

                        
                }
            };
            if (session.currentDungeon != null)
            {
                sceneInfo.Dungeon = new()
                {
                    DungeonId = session.currentDungeon.table.dungeonId,
                };
            }
            else
            {
                sceneInfo.Empty = new()
                {
                    
                };
            }
            foreach (var item in ResourceManager.sceneAreaTable)
            {
                if(session.curSceneNumId==ResourceManager.GetSceneNumIdFromLevelData(item.Value.sceneId))
                    sceneInfo.UnlockArea.Add(item.Value.areaId);
            }
            session.teams[session.teamIndex].members.ForEach(m =>
            {
                sceneInfo.Detail.CharList.Add(session.chars.Find(c => c.guid == m).ToSceneProto());
            });
               
                
            SetData(ScMessageId.ScSelfSceneInfo, sceneInfo);
            
           
        }

    }
}
