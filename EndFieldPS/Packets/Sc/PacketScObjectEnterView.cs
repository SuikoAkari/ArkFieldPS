using EndFieldPS.Game.Entities;
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
    public class PacketScObjectEnterView : Packet
    {

        public PacketScObjectEnterView(Player session, List<Entity> entities) {

            ScObjectEnterView proto = new()
            {
                Detail = new()
                {
                    
                },
                
            };
            foreach (Entity entity in entities)
            {
                if (entity is EntityMonster)
                {
                    EntityMonster monster = (EntityMonster)entity;
                    proto.Detail.MonsterList.Add(monster.ToProto());
                }
                else if (entity is EntityNpc)
                {
                    EntityNpc npc = (EntityNpc)entity;
                    proto.Detail.NpcList.Add(npc.ToProto());
                }
                else if (entity is EntityInteractive)
                {
                    EntityInteractive interact = (EntityInteractive)entity;
                    proto.Detail.InteractiveList.Add(interact.ToProto());
                }
                
            }
            

            SetData(ScMessageId.ScObjectEnterView, proto);
        }

    }
}
