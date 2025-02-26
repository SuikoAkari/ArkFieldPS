using ArkFieldPS.Game;
using ArkFieldPS.Game.Entities;
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
using static ArkFieldPS.Resource.ResourceManager;

namespace ArkFieldPS.Packets.Cs
{
    public class HandleCsSceneSetSafeZone
    {

        [Server.Handler(CsMessageId.CsSceneSetSafeZone)]
        public static void Handle(Player session, CsMessageId cmdId, Packet packet)
        {
            CsSceneSetSafeZone req = packet.DecodeBody<CsSceneSetSafeZone>();

            if (req.InZone)
            {
                var entity = session.sceneManager.GetEntity(req.Id) as EntityInteractive;
                if (entity != null)
                {
                    Vector3f position = new();
                    Vector3f rotation = new();
                    foreach(var i in entity.properties)
                    {
                        var val = i.ToProto();
                        switch(i.key)
                        {
                            case "position":
                                position = new()
                                {
                                    x = val.ValueFloatList[0],
                                    y = val.ValueFloatList[1],
                                    z = val.ValueFloatList[2]
                                };
                                break;
                            case "rotation":
                                rotation = new()
                                {
                                    x = val.ValueFloatList[0],
                                    y = val.ValueFloatList[1],
                                    z = val.ValueFloatList[2]
                                };

                                break;
                        }
                    }
                    Logger.Print($"Set safe zone for player {session.accountId} to (sceneNum:{entity.sceneNumId} position:{position.ToProto()} rotation:{rotation.ToProto()})");
                    session.savedSaveZone.sceneNumId = entity.sceneNumId;
                    session.savedSaveZone.position = position;
                    session.savedSaveZone.rotation = rotation;
                }
                else
                    Logger.PrintError($"Cannot find Waypoint entity with ID {req.Id}");
            }

            Logger.Print($"CsSceneSetSafeZone:" + req.ToString());
        }
       
    }
}
