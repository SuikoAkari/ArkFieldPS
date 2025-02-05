using BeyondTools.VFS.Crypto;
using EndFieldPS.Network;
using EndFieldPS.Packets.Sc;
using EndFieldPS.Protocol;
using EndFieldPS.Resource;
using Google.Protobuf;
using Google.Protobuf.WellKnownTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;

using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static EndFieldPS.Resource.ResourceManager;
using static System.Net.Mime.MediaTypeNames;

namespace EndFieldPS.Packets.Cs
{
    public class HandleCsSceneLoadFinish
    {
        [Server.Handler(CsMessageId.CsSceneLoadFinish)]
        public static void HandleSceneFinish(Player session, CsMessageId cmdId, Packet packet)
        {
            CsSceneLoadFinish req = packet.DecodeBody<CsSceneLoadFinish>();

            session.Send(new PacketScSelfSceneInfo(session, true));
            session.sceneManager.LoadCurrentTeamEntities();
            session.sceneManager.LoadCurrent();
           /* if (session.curSceneNumId == 98)
            {
                ScObjectEnterView enterView = new()
                {
                    Detail = new()
                    {

                    }
                };
                enterView.Detail.NpcList.Add(new SceneNpc()
                {
                    CommonInfo = new()
                    {
                        Hp=500,

                        SceneNumId=98,
                        Id=34034045,
                        Templateid= "npc_0004_pelica_spaceship_i001",
                        Position=session.position.ToProto(),
                        Rotation=session.rotation.ToProto(),
                        Type= (int)0,

                    },

                });
                session.Send(ScMessageId.ScObjectEnterView, enterView);
            }*/

        }
    }
}
