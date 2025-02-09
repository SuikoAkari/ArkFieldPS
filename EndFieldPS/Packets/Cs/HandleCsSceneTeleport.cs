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
    public class HandleCsSceneTeleport
    {

        [Server.Handler(CsMessageId.CsSceneTeleport)]
        public static void Handle(Player session, CsMessageId cmdId, Packet packet)
        {
            CsSceneTeleport req = packet.DecodeBody<CsSceneTeleport>();
            session.curSceneNumId=req.SceneNumId;
            ScSceneTeleport t = new()
            {
                TeleportReason = req.TeleportReason,
                PassThroughData = req.PassThroughData,
                Position = req.Position,
                Rotation = req.Rotation,
                SceneNumId = req.SceneNumId,
                TpUuid = 20000000,

            };
            session.Send(ScMessageId.ScSceneTeleport, t);
        }
       
    }
}
