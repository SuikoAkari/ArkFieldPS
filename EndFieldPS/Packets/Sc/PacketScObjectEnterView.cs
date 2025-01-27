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

        public PacketScObjectEnterView(Player session, Entity entity) {

            ScObjectEnterView proto = new()
            {
                Detail = new()
                {
                    
                },
                
            };
            if (entity is EntityMonster)
            {
                EntityMonster monster = (EntityMonster) entity;
                proto.Detail.MonsterList.Add(monster.ToProto());
            }

            SetData(ScMessageId.ScObjectEnterView, proto);
        }

    }
}
