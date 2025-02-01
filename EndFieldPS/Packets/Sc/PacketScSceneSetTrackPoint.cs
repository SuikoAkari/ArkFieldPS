using EndFieldPS.Network;
using EndFieldPS.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace EndFieldPS.Packets.Sc
{
    public class PacketScSceneSetTrackPoint : Packet
    {

        public PacketScSceneSetTrackPoint(SceneTrackPoint trackPoint)
        {
            ScSceneSetTrackPoint proto = new()
            {
                TrackPoint = trackPoint
            };
            SetData(ScMessageId.ScSceneSetTrackPoint, proto);
        }
    }
}
