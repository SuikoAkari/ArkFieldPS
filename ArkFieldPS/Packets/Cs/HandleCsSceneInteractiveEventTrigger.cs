using ArkFieldPS.Game.Character;
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

namespace ArkFieldPS.Packets.Cs
{
    public class HandleCsSceneInteractiveEventTrigger
    {

        [Server.Handler(CsMessageId.CsSceneInteractiveEventTrigger)]
        public static void Handle(Player session, CsMessageId cmdId, Packet packet)
        {
            CsSceneInteractiveEventTrigger  req = packet.DecodeBody<CsSceneInteractiveEventTrigger>();
            ScSceneInteractiveEventTrigger rsp = new()
            {
                
            };
            
            EntityInteractive entity = (EntityInteractive)session.sceneManager.GetEntity(req.Id);
            if (entity != null)
            {
                entity.Interact(req.EventName, req.Properties);
            }
            /*ScSceneTriggerClientInteractiveEvent tr = new()
            {
                EventName = req.EventName,
                Id = req.Id,
                SceneNumId = req.SceneNumId,
            };
            session.Send(ScMessageId.ScSceneTriggerClientInteractiveEvent, tr);*/
        }
       
    }
}
