using ArkFieldPS.Network;
using ArkFieldPS.Packets.Sc;
using ArkFieldPS.Protocol;


namespace ArkFieldPS.Packets.Cs
{
    public class HandleCsGetMail
    {

        [Server.Handler(CsMessageId.CsGetMail)]
        public static void Handle(Player session, CsMessageId cmdId, Packet packet)
        {
            CsGetMail req = packet.DecodeBody<CsGetMail>();
            session.Send(new PacketScGetMail(session));
        }
       
    }
}
