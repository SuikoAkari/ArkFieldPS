using EndFieldPS.Network;
using EndFieldPS.Protocol;
using Google.Protobuf;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static EndFieldPS.Resource.ResourceManager;

namespace EndFieldPS.Packets.Cs
{
    public class HandlCsMoveObjectMove
    {

        [Server.Handler(CsMessageId.CsMoveObjectMove)]
        public static void Handle(EndminPlayer session, CsMessageId cmdId, Packet packet)
        {
            CsMoveObjectMove req = packet.DecodeBody<CsMoveObjectMove>();
            foreach (var moveInfo in req.MoveInfo)
            {
                if (moveInfo.Objid == session.teams[session.teamIndex].leader)
                {
                    session.position=new Vector3f(moveInfo.MotionInfo.Position);
                    session.rotation = new Vector3f(moveInfo.MotionInfo.Rotation);
                }
            }
            ScMoveObjectMove proto = new()
            {
                MoveInfo =
                {
                    req.MoveInfo,
                },
                ServerNotify=false,
            };

            session.Send(ScMessageId.ScMoveObjectMove, proto);
        }
       
    }
}
