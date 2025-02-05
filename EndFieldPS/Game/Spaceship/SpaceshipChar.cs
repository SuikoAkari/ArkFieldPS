using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static EndFieldPS.Resource.ResourceManager;
using MongoDB.Bson.Serialization.IdGenerators;
using EndFieldPS.Resource;

namespace EndFieldPS.Game.Spaceship
{
    public class SpaceshipChar
    {
        [BsonId(IdGenerator = typeof(ObjectIdGenerator))]
        public ObjectId _id { get; set; }
        public string id;
        public ulong guid;
        public ulong owner;
        public string stationedRoomId = "";
        public float physicalStrength;
        public int favorability;
        public bool isWorking;
        public SpaceshipChar()
        {

        }
        public SpaceshipChar(ulong owner, string id)
        {
            this.owner = owner;
            this.id = id;
            this.guid = (ulong)new Random().NextInt64();
            position = GetLevelData(98).playerInitPos;
            rotation = GetLevelData(98).playerInitRot;
        }
        public string GetNpcId()
        {
            return ResourceManager.spaceShipCharBehaviourTable[id].npcId;
        }
        public ScdSpaceshipChar ToSpaceshipChar()
        {
            return new ScdSpaceshipChar()
            {
                CharId = id,
                Favorability = favorability,
                IsWorking = isWorking,
                PhysicalStrength = physicalStrength,
                StationedRoomId = stationedRoomId,
                Skills =
                {
                    new ScdSpaceshipCharSkill()
                    {
                        Index=1,
                        SkillId="spaceship_skill_acc_charmaterial_produce2_1"
                    },
                    new ScdSpaceshipCharSkill()
                    {
                        Index=0,
                        SkillId="spaceship_skill_acc_all_ps_recovery1_2"
                    }

                },
            };
        }

    }
}
