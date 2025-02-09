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

            session.Send(new PacketScSelfSceneInfo(session, SelfInfoReasonType.SlrEnterScene));
            session.sceneManager.LoadCurrentTeamEntities();
            session.sceneManager.LoadCurrent();
            if (session.curSceneNumId == 98)
            {
                session.Send(new PacketScSyncGameMode(session, "spaceship"));
            }
            else
            {
                
                if (session.currentDungeon != null && session.curSceneNumId==GetSceneNumIdFromLevelData(session.currentDungeon.table.sceneId))
                {
                    session.Send(new PacketScSyncGameMode(session, "dungeon_race"));
                }
                else
                {
                    session.currentDungeon = null;
                    session.Send(new PacketScSyncGameMode(session, ""));
                }
                    
            }
        }
    }
}
