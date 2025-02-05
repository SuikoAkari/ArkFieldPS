using EndFieldPS.Database;
using EndFieldPS.Game.Inventory;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization.IdGenerators;
using static EndFieldPS.Resource.ResourceManager;
using EndFieldPS.Resource;

namespace EndFieldPS.Game.Spaceship
{
    public class SpaceshipManager
    {
        public Player owner;
        public List<SpaceshipChar> chars=new();
        public List<SpaceshipRoom> rooms = new();
        public SpaceshipManager(Player o)
        {
            owner = o;
        }
        public SpaceshipChar GetChar(string id)
        {
            return chars.Find(c => c.id == id);
        }
        public void AddNewCharacter(string id)
        {
            if (id.Contains("endmin")) return;
            SpaceshipChar chara = new(owner.roleId, id);
            chars.Add(chara);
        }
        public void Load()
        {
            chars = DatabaseManager.db.LoadSpaceshipChars(owner.roleId);
            rooms = DatabaseManager.db.LoadSpaceshipRooms(owner.roleId);
            foreach (var chara in owner.chars)
            {
                SpaceshipChar c = GetChar(chara.id);
                if (c == null && !chara.id.Contains("endmin"))
                {
                    AddNewCharacter(chara.id);
                }
            }
            if(rooms.Count < 1)
            {
                rooms.Add(new SpaceshipRoom(owner.roleId,"control_center"));
            }
        }

        public void Save()
        {
            foreach(SpaceshipChar spaceshipChar in chars)
            {
                DatabaseManager.db.UpsertSpaceshipChar(spaceshipChar);
            }
            foreach(SpaceshipRoom room in rooms)
            {
                DatabaseManager.db.UpsertSpaceshipRoom(room);
            }
        }

        public void UpdateStationedChars()
        {
            Dictionary<string, string> charAndRoom = new();
            foreach(SpaceshipRoom room in rooms)
            {
                foreach (var c in room.stationedCharList)
                {
                    charAndRoom.Add(c, room.id);
                }
            }
            foreach(SpaceshipChar chara in chars)
            {
                if (charAndRoom.ContainsKey(chara.id))
                {
                    chara.stationedRoomId = charAndRoom[chara.id];
                }
                else
                {
                    chara.stationedRoomId = "";
                }
            }
        }
    }
    public class SpaceshipRoom
    {
        [BsonId(IdGenerator = typeof(ObjectIdGenerator))]
        public ObjectId _id { get; set; }
        public string id="";
        public int level;
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
                SpaceshipChar ch=GetOwner().spaceshipManager.GetChar(chara);
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
            return Server.clients.Find(c=>c.roleId==owner);
        }
        
        public ScdSpaceshipRoom ToRoomProto()
        {
            SpaceshipRoomInsTable roomInfo = ResourceManager.spaceshipRoomInsTable[id];
            ScdSpaceshipRoom room = new()
            {
                Id=id,
                Level=level,
                Type=roomInfo.roomType,
                HasCharWorking= HasCharWorking(),
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
                    room.ManufacturingStation= new()
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
    public class SpaceshipChar
    {
        [BsonId(IdGenerator = typeof(ObjectIdGenerator))]
        public ObjectId _id { get; set; }
        public string id;
        public ulong guid;
        public ulong owner;
        public string stationedRoomId="";
        public float physicalStrength;
        public int favorability;
        public bool isWorking;
        public Vector3f position;
        public Vector3f rotation;
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
                CharId=id,
                Favorability=favorability,
                IsWorking=isWorking,
                PhysicalStrength=physicalStrength,
                StationedRoomId=stationedRoomId,
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
