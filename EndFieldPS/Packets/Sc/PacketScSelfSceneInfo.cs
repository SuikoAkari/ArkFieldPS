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

        public PacketScSelfSceneInfo(Player session, bool isTeamSpawn=false, SelfInfoReasonType infoReason = SelfInfoReasonType.SlrEnterScene) {
            if (isTeamSpawn)
            {
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
                    SceneGrade = 1,
                    
                    Detail = new()
                    {
                        TeamIndex = session.teamIndex,

                        CharList =
                        {

                        },
                        
                    }
                };
                foreach (var item in ResourceManager.sceneAreaTable)
                {
                    sceneInfo.UnlockArea.Add(item.Value.areaId);
                }
                session.teams[session.teamIndex].members.ForEach(m =>
                {
                    sceneInfo.Detail.CharList.Add(session.chars.Find(c => c.guid == m).ToSceneProto());
                });
                session.sceneManager.LoadCurrentTeamEntities();
                SetData(ScMessageId.ScSelfSceneInfo, sceneInfo);
            }
           
        }

    }
}
