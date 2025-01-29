using EndFieldPS.Network;
using EndFieldPS.Packets.Sc;
using EndFieldPS.Protocol;


namespace EndFieldPS.Packets.Cs
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
