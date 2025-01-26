using EndFieldPS.Network;
using EndFieldPS.Protocol;

namespace EndFieldPS.Packets.Cs
{
    public class HandleCsSceneRepatriate
    {

        [Server.Handler(CsMessageId.CsSceneRepatriate)]
        public static void Handle(Player session, CsMessageId cmdId, Packet packet)
        {
            CsSceneRepatriate req = packet.DecodeBody<CsSceneRepatriate>();
            
            // No idea how pass_through_data is used, it's not part of the packet sent by the official server
            session.Send(ScMessageId.ScSceneRepatriate, new ScSceneRepatriate()
            {
                SceneNumId = req.SceneNumId,
                RepatriateSource = req.RepatriateSource,
            });
        }
       
    }
}
