using ArkFieldPS.Network;
using ArkFieldPS.Protocol;
using ArkFieldPS.Resource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ArkFieldPS.Packets.Sc
{
    public class PacketScSyncAllGameVar : Packet
    {

        public PacketScSyncAllGameVar(Player client) {

            ScSyncAllGameVar proto = new()
            {
                ServerVars =
                {
                    {(int)ServerGameVarEnum.ServerGameVarDashEnergyLimit,client.maxDashEnergy }, //Dash
                }
            };
            foreach(var id in ResourceManager.strIdNumTable.client_game_var_string_id.dic)
            {
                proto.ClientVars.Add(id.Value, 1);
            }
            SetData(ScMessageId.ScSyncAllGameVar, proto);
        }

    }
}
