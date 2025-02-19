using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ArkFieldPS.Resource.ResourceManager;
using ArkFieldPS.Resource;
using MongoDB.Bson.Serialization.IdGenerators;

namespace ArkFieldPS.Game.Spaceship
{
    public class SpaceshipRoom
    {
        [BsonId(IdGenerator = typeof(ObjectIdGenerator))]
        public ObjectId _id { get; set; }
        public string id = "";
        public int level = 1;
        public List<string> stationedCharList = new();
        public ulong owner;
        public SpaceshipRoom()
        {

        }
        public SpaceshipRoom(ulong owner, string id)
        {
            this.owner = owner;
            this.id = id;
        }
        public bool HasCharWorking()
        {
            bool val = false;
            foreach (string chara in stationedCharList)
            {
                SpaceshipChar ch = GetOwner().spaceshipManager.GetChar(chara);
                if (ch != null)
                {
                    if (ch.isWorking)
                    {
                        val = true;
                    }
                }
            }
            return val;
        }
        public int GetType()
        {
            SpaceshipRoomInsTable roomInfo = ResourceManager.spaceshipRoomInsTable[id];
            return roomInfo.roomType;
        }
        public Player GetOwner()
        {
            return Server.clients.Find(c => c.roleId == owner);
        }

        public ScdSpaceshipRoom ToRoomProto()
        {
            SpaceshipRoomInsTable roomInfo = ResourceManager.spaceshipRoomInsTable[id];
            ScdSpaceshipRoom room = new()
            {
                Id = id,
                Level = level,
                Type = roomInfo.roomType,
                HasCharWorking = HasCharWorking(),
                StationedCharList =
                {
                    stationedCharList
                },
                LevelUpConditionFlags =
                {
                    { id+"_level_"+(level+1),true}
                },
                LevelUpConditonValues =
                {
                    { id+"_level_"+(level+1),4}
                },
                AttrsMap =
                        {
                            {0, new ScdSpaceshipRoomAttr()
                            {
                                Value=24.8f,
                                TheoreticalValue=24.8f,
                                BaseAttrs =
                                {
                                    new ScdSpaceshipRoomAttrUnit()
                                    {
                                        Value=20,

                                        Source = new()
                                        {
                                            SourceType=1,
                                        }
                                    }
                                },
                                PercentAttrs =
                                {
                                    new ScdSpaceshipRoomAttrUnit()
                                    {
                                        Value=0.24f,
                                        Source = new()
                                        {
                                            CharId="chr_0004_pelica",
                                            SkillId="spaceship_skill_acc_all_ps_recovery1_2"
                                        }
                                    }
                                }
                            } },
                            {1, new ScdSpaceshipRoomAttr()
                            {
                                Value=12,
                                TheoreticalValue=12,
                                BaseAttrs =
                                {
                                    new ScdSpaceshipRoomAttrUnit()
                                    {
                                        Type=1,
                                        Value=12,
                                        Source = new()
                                        {
                                            SourceType=1
                                        }
                                    }
                                }
                            }
                            }
                        },
            };
            switch (roomInfo.roomType)
            {
                case 0:
                    room.ControlCenter = new()
                    {

                    };
                    break;
                case 1:
                    room.ManufacturingStation = new()
                    {

                    };
                    break;
                case 2:
                    room.GrowCabin = new()
                    {

                    };
                    break;
            }
            return room;
        }
    }
}
