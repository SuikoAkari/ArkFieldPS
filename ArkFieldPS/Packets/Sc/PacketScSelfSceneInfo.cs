using ArkFieldPS.Game.Entities;
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
                    
                        
                },
                LevelScripts =
                {

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

            //Levelscripts here?
            ResourceManager.GetLevelData(session.curSceneNumId).levelData.levelScripts.ForEach(l =>
            {
                LevelScriptInfo script = new LevelScriptInfo()
                {
                    ScriptId = l.scriptId,
                    IsDone = false,
                    State = 1,

                };
                int i = 0;
                foreach (var item in l.properties)
                {
                    DynamicParameter p=item.ToProto();
                    if (p != null)
                    script.Properties.Add(l.GetPropertyId(item.key,script.Properties.Keys.ToList()), p);
                }
                sceneInfo.LevelScripts.Add(script);
            });
            SetData(ScMessageId.ScSelfSceneInfo, sceneInfo);
            
           
        }

    }
}
