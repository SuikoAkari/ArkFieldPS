using ArkFieldPS.Resource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static ArkFieldPS.Resource.ResourceManager;
using ArkFieldPS.Network;
using ArkFieldPS.Protocol;

namespace ArkFieldPS.Packets.Sc
{
    public class PacketScAdventureBookSync : Packet
    {
        public PacketScAdventureBookSync(Player player) {
            ScAdventureBookSync proto = new ScAdventureBookSync() {
                AdventureBookStage=1,
                DailyActivation=100,
            };

            foreach(var i in adventureTaskTable)
            {
                if (i.Value.AdventureBookStage == 1)                    
                {
                    proto.Tasks.Add(new AdventureTask()
                    {
                        TaskId = i.Value.AdventureTaskId,
                        State = 1
                    });
                }
            }
            SetData(ScMessageId.ScAdventureBookSync, proto);
        }
    }
}