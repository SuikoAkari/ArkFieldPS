using ArkFieldPS.Network;
using ArkFieldPS.Protocol;
using Google.Protobuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ArkFieldPS.Packets.Cs
{
    public class HandleCsSceneTeleport
    {

        [Server.Handler(CsMessageId.CsSceneTeleport)]
        public static void Handle(Player session, CsMessageId cmdId, Packet packet)
        {
            CsSceneTeleport req = packet.DecodeBody<CsSceneTeleport>();
            
            if (session.curSceneNumId != req.SceneNumId)
            {
                session.EnterScene(req.SceneNumId, new Resource.ResourceManager.Vector3f(req.Position), new Resource.ResourceManager.Vector3f(req.Rotation));
            }
            else
            {
                ScSceneTeleport t = new()
                {
                    TeleportReason = req.TeleportReason,
                    PassThroughData = req.PassThroughData,
                    Position = req.Position,
                    Rotation = req.Rotation,
                    SceneNumId = req.SceneNumId,
                    

                };
                session.curSceneNumId = t.SceneNumId;
                session.Send(ScMessageId.ScSceneTeleport, t);
            }
            
            

        }
       
    }
}
