using EndFieldPS.Network;
using EndFieldPS.Protocol;
using Google.Protobuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace EndFieldPS.Packets.Cs
{
    public class HandleCsMergeMsg
    {

        [Server.Handler(CsMessageId.CsMergeMsg)]
        public static void Handle(Player session, CsMessageId cmdId, Packet p)
        {
            CsMergeMsg req = p.DecodeBody<CsMergeMsg>();



            byte[] allBytes = req.Msg.ToByteArray();
            while (allBytes.Length > 3) {

                byte headLength = Packet.GetByte(allBytes, 0);
                ushort bodyLength = Packet.GetUInt16(allBytes, 1);

                byte[] head = allBytes.AsSpan().Slice(3, headLength).ToArray();
                byte[] body = allBytes.AsSpan().Slice(3+ headLength, bodyLength).ToArray();
                Packet packet = new()
                {
                    finishedBody = body,
                    csHead = CSHead.Parser.ParseFrom(head),
                    cmdId = CSHead.Parser.ParseFrom(head).Msgid,

                };
                if (Server.config.logOptions.packets)
                {
                    Logger.Print("CmdId: " + (CsMessageId)packet.csHead.Msgid);
                    Logger.Print(BitConverter.ToString(packet.finishedBody).Replace("-", string.Empty).ToLower());
                }

                try
                {
                    NotifyManager.Notify(session, (CsMessageId)packet.cmdId, packet);
                }
                catch (Exception e)
                {
                    Logger.PrintError("Error while notify packet: " + e.Message);
                }
                allBytes = allBytes.AsSpan().Slice(3 + headLength + bodyLength).ToArray();
            }

        }
       
    }
}
