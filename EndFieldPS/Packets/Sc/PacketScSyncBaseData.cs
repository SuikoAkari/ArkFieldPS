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
    public class PacketScSyncBaseData : Packet
    {

        public PacketScSyncBaseData(EndminPlayer client) {

            ScSyncBaseData proto = new ScSyncBaseData()
            {
                Roleid = client.roleId,
                Level = 1,
                RoleName = "Player",
                Gender = Gender.GenFemale,
                ShortId="1",
            };

            SetData(ScMessageId.ScSyncBaseData, proto);
        }

    }
}
