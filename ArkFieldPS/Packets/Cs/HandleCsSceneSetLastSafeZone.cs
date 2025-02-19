using ArkFieldPS.Game;
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
    public class HandleCsSceneSetLastSafeZone
    {

        [Server.Handler(CsMessageId.CsSceneSetLastSafeZone)]
        public static void Handle(Player session, CsMessageId cmdId, Packet packet)
        {
            CsSceneSetLastSafeZone req = packet.DecodeBody<CsSceneSetLastSafeZone>();
            
            //TODO understand how to work
            /*if (req.SceneNumId != session.curSceneNumId)
            {
                //session.sceneManager.UnloadCurrent(true);
                session.curSceneNumId = req.SceneNumId;
                session.sceneManager.LoadCurrent();
                //session.EnterScene(req.SceneNumId,new Vector3f(req.Position),new Vector3f(req.Rotation));
            }*/
           
        }
       
    }
}
