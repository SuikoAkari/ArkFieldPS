using ArkFieldPS.Game.Character;
using ArkFieldPS.Game.Entities;
using ArkFieldPS.Network;
using ArkFieldPS.Packets.Sc;
using ArkFieldPS.Protocol;
using ArkFieldPS.Resource;
using Google.Protobuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static ArkFieldPS.Resource.ResourceManager;

namespace ArkFieldPS.Packets.Cs
{
    public class HandleCsSceneInteractSpInteractive
    {

        [Server.Handler(CsMessageId.CsSceneInteractSpInteractive)]
        public static void Handle(Player session, CsMessageId cmdId, Packet packet)
        {
            CsSceneInteractSpInteractive req = packet.DecodeBody<CsSceneInteractSpInteractive>();
            Entity entity = session.sceneManager.GetEntity(req.ObjId);
            if (entity != null)
            {
                LevelScene scene = ResourceManager.GetLevelData(entity.sceneNumId);
                switch (req.OpType)
                {
                    case SpInteractiveOpType.CommonActive:
                        session.bitsetManager.AddValue(Resource.BitsetType.InteractiveActive, ResourceManager.levelShortIdTable[scene.mapIdStr].ids[(long)entity.guid]);
                        break;
                    default:
                        break;
                }
                session.Send(new PacketScSyncAllBitset(session));
                ScSceneInteractSpInteractive rsp = new()
                {
                    ObjId = req.ObjId,
                };
                session.Send(ScMessageId.ScSceneInteractSpInteractive, rsp);
            }
            
        }
       
    }
}
