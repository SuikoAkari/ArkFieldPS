using ArkFieldPS.Network;
using ArkFieldPS.Packets.Sc;
using ArkFieldPS.Protocol;

namespace ArkFieldPS.Packets.Cs
{
    public class HandleCsSceneRepatriate
    {

        [Server.Handler(CsMessageId.CsSceneRepatriate)]
        public static void Handle(Player session, CsMessageId cmdId, Packet packet)
        {
            CsSceneRepatriate req = packet.DecodeBody<CsSceneRepatriate>();
            int sceneNumId = session.savedSaveZone.sceneNumId; 
            if (session.curSceneNumId != sceneNumId)
            {
                session.EnterScene(sceneNumId, session.savedSaveZone.position, session.savedSaveZone.rotation, req.PassThroughData);
            }
            else
            {
                ScSceneTeleport t = new()
                {
                    TeleportReason = 2,
                    PassThroughData = req.PassThroughData,
                    Position = session.savedSaveZone.position.ToProto(),
                    Rotation = session.savedSaveZone.rotation.ToProto(),
                    SceneNumId = req.SceneNumId,
                };
                session.curSceneNumId = t.SceneNumId;
                session.Send(ScMessageId.ScSceneTeleport, t);
            }
            // Not correctly fixed, need to find the issue - SuikoAkari
            // No idea how pass_through_data is used, it's not part of the packet sent by the official server
            session.Send(ScMessageId.ScSceneRepatriate, new ScSceneRepatriate()
            {
                SceneNumId = session.savedSaveZone.sceneNumId,
                RepatriateSource = req.RepatriateSource,
            });
        }
       
    }
}
