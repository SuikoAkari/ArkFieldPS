using ArkFieldPS.Network;
using ArkFieldPS.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static ArkFieldPS.Resource.ResourceManager;

namespace ArkFieldPS.Packets.Sc
{
    public class PacketScSyncAllBloc : Packet
    {
        
        public PacketScSyncAllBloc(Player client) {

            ScSyncAllBloc allblocks = new()
            {
                Blocs =
                {

                }
            };
            foreach (var bloc in blocDataTable)
            {
                allblocks.Blocs.Add(new BlocInfo()
                {
                    Exp = 0,
                    Level = 1,
                    Blocid = bloc.Value.blocId,
                    
                });
            }
            
            SetData(ScMessageId.ScSyncAllBloc, allblocks);
        }

    }
}
