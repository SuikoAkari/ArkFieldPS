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

        public PacketScSyncBaseData(Player client) {

            ScSyncBaseData proto = new ScSyncBaseData()
            {
                Roleid = client.roleId,
                Level = client.level,
                Exp=client.xp,
                RoleName = client.nickname,
                Gender = Gender.GenFemale,
                ShortId="1",
                
            };

            SetData(ScMessageId.ScSyncBaseData, proto);
        }

    }
}
