using ArkFieldPS.Network;
using ArkFieldPS.Packets.Sc;
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
    public class HandleCsSpaceshipStationCharChangeWorkState
    {

        [Server.Handler(CsMessageId.CsSpaceshipStationCharChangeWorkState)]
        public static void Handle(Player session, CsMessageId cmdId, Packet packet)
        {
            CsSpaceshipStationCharChangeWorkState req = packet.DecodeBody<CsSpaceshipStationCharChangeWorkState>();
            session.spaceshipManager.GetChar(req.CharId).isWorking = req.GoToWork;
            foreach (var room in session.spaceshipManager.rooms)
            {
                session.Send(new PacketScSpaceshipSyncRoomStation(session,room));
            }
        }
       
    }
}
