using EndFieldPS.Network;
using EndFieldPS.Packets.Sc;
using EndFieldPS.Protocol;

namespace EndFieldPS.Packets.Cs
{
    public class HandleCsSceneRepatriate
    {

        [Server.Handler(CsMessageId.CsSceneRepatriate)]
        public static void Handle(Player session, CsMessageId cmdId, Packet packet)
        {
            CsSceneRepatriate req = packet.DecodeBody<CsSceneRepatriate>();
            // Not correctly fixed, need to find the issue - SuikoAkari
            // No idea how pass_through_data is used, it's not part of the packet sent by the official server
            /*session.Send(ScMessageId.ScSceneRepatriate, new ScSceneRepatriate()
            {
                SceneNumId = req.SceneNumId,
                RepatriateSource = req.RepatriateSource,
            });
            //session.Send(new PacketScSyncCharBagInfo(session));
            session.EnterScene(req.SceneNumId);*/
        }
       
    }
}
