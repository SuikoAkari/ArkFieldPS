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
    public class PacketScSpaceshipSync : Packet
    {

        public PacketScSpaceshipSync(Player client) {
            //TODO de hardcode this and implement a SpaceshipManager
            ScSpaceshipSync proto = new ScSpaceshipSync()
            {
                Rooms =
                {
                    new ScdSpaceshipRoom()
                    {
                        Id="control_center",
                        Type=0,
                        ControlCenter = new()
                        {

                        },
                        Level=1,

                    }
                },

            };

            SetData(ScMessageId.ScSpaceshipSync, proto);
        }

    }
}
