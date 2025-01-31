using EndFieldPS.Network;
using EndFieldPS.Protocol;
using EndFieldPS.Resource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace EndFieldPS.Packets.Sc
{
    public class PacketScSyncAllGameVar : Packet
    {

        public PacketScSyncAllGameVar(Player client) {

            ScSyncAllGameVar proto = new()
            {
                ServerVars =
                {
                    {(int)ServerGameVarEnum.ServerGameVarDashEnergyLimit,client.maxDashEnergy } //Dash
                }
            };

            SetData(ScMessageId.ScSyncAllGameVar, proto);
        }

    }
}
