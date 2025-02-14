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
    public class HandleCsSpaceshipStationChar
    {

        [Server.Handler(CsMessageId.CsSpaceshipStationChar)]
        public static void Handle(Player session, CsMessageId cmdId, Packet packet)
        {
            CsSpaceshipStationChar req = packet.DecodeBody<CsSpaceshipStationChar>();
            session.spaceshipManager.rooms.Find(r=>r.id==req.RoomId).stationedCharList=req.StationedCharList.ToList();
            session.spaceshipManager.UpdateStationedChars();
            session.Send(new PacketScSpaceshipSyncRoomStation(session, session.spaceshipManager.rooms.Find(r => r.id == req.RoomId)));
            //Logger.Print("Server: " + curtimestamp + " client: " + req.ClientTs);
        }
       
    }
}
