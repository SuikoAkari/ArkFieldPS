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
    public class PacketScAdventureBookSync : Packet
    {
        public PacketScAdventureBookSync(Player player) {
            ScAdventureBookSync proto = new ScAdventureBookSync() {
                AdventureBookStage=1,
                DailyActivation=100,
            };
            foreach(var i in ResourceManager.adventureTaskTable)
            {
                if (i.Value.adventureBookStage == 1)                    
                {
                    proto.Tasks.Add(new AdventureTask()
                    {
                        TaskId = i.Value.adventureTaskId,
                        State = 1
                    });
                }
            }
            SetData(ScMessageId.ScAdventureBookSync, proto);
        }
    }
}
