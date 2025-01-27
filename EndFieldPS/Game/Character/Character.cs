using EndFieldPS.Packets.Sc;
using EndFieldPS.Protocol;
using EndFieldPS.Resource;
using Google.Protobuf.Collections;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static EndFieldPS.Resource.ResourceManager.CharGrowthTable;

namespace EndFieldPS.Game.Character
{
    public class Character
    {
        [BsonId(IdGenerator = typeof(ObjectIdGenerator))]
        public ObjectId _id { get; set; }
        [BsonElement("templateId")]
        public string id;
        public ulong guid;
        public ulong weaponGuid;

        public int level;
        public int xp;
        public ulong owner;
        public double curHp;
        public float ultimateSp = 200;
        public uint potential = 0;
        public string breakNode = "charBreak20";
        public List<string> passiveSkillNodes = new();
        public List<string> attrNodes = new();
        public List<string> factoryNodes = new();
        public Character()
        {
           
        }
        
        public Character(ulong owner, string id) : this(owner, id, 1)
        {

        }

        public void UnlockNode(string nodeId)
        {
            CharTalentNode nodeInfo = ResourceManager.GetTalentNode(id, nodeId);
            if (nodeInfo == null) return;
            //TODO remove cost items
            switch (nodeInfo.nodeType)
            {
                case TalentNodeType.CharBreak:
                    breakNode = nodeId;
                    break;
                case TalentNodeType.EquipBreak:
                    breakNode = nodeId;
                    break;
                case TalentNodeType.Attr:
                    attrNodes.Add(nodeId);
                    break;
                case TalentNodeType.PassiveSkill:
                    passiveSkillNodes.Add(nodeId);
                    break;
                case TalentNodeType.FactorySkill:
                    factoryNodes.Add(nodeId);
                    break;
                default:
                    Logger.PrintWarn($"Unimplemented NodeType {nodeInfo.nodeType}, not unlocked server side.");
                    break;
            }
            GetOwner().Send(new PacketScCharUnlockTalentNode(GetOwner(), this,nodeId));
        }
        public Character(ulong owner,string id, int level) : this()
        {
            this.owner = owner;
            this.id = id;
            this.level = level;
            guid = GetOwner().random.Next();
            this.weaponGuid = GetOwner().inventoryManager.AddWeapon(ResourceManager.charGrowthTable[id].defaultWeaponId, 1).guid;
            this.curHp = ResourceManager.characterTable[id].attributes[level].Attribute.attrs.Find(A => A.attrType == (int)AttributeType.MaxHp)!.attrValue;
        }
        public List<ResourceManager.Attribute> GetAttributes()
        {
            int lev = level - 1 + GetbreakStage();
            return ResourceManager.characterTable[id].attributes[lev].Attribute.attrs;
        }
        public Player GetOwner()
        {
            return Server.clients.Find(c=>c.roleId == this.owner); 
        }
        public SceneCharacter ToSceneProto()
        {
            SceneCharacter proto= new SceneCharacter()
            {
                Level = level,
                
                BattleInfo = new()
                {
                    MsgGeneration = 1,
                    
                    SkillList =
                                {
                                    new ServerSkill()
                                    {
                                        Blackboard = new()
                                        {

                                        },
                                        InstId=GetOwner().random.Next(),
                                        Level=1,
                                        Source=BattleSkillSource.Default,
                                        PotentialLv=1,
                                        SkillId=id+"_NormalSkill",
                                    },
                                    new ServerSkill()
                                    {
                                        Blackboard = new()
                                        {

                                        },
                                        InstId=GetOwner().random.Next(),
                                        Level=1,
                                        Source=BattleSkillSource.Default,
                                        PotentialLv=1,
                                        SkillId=id+"_ComboSkill",
                                    },
                                    new ServerSkill()
                                    {
                                        Blackboard = new()
                                        {

                                        },
                                        InstId=GetOwner().random.Next(),
                                        Level=1,
                                        Source=BattleSkillSource.Default,
                                        PotentialLv=1,
                                        SkillId=id+"_UltimateSkill",
                                    },
                                    new ServerSkill()
                                    {
                                        Blackboard = new()
                                        {

                                        },
                                        InstId=GetOwner().random.Next(),
                                        Level=1,
                                        Source=BattleSkillSource.Default,
                                        PotentialLv=1,
                                        SkillId=id+"_NormalAttack",
                                    }
                                }
                },

                Name = $"{ResourceManager.characterTable[id].engName}",
                
                CommonInfo = new()
                {
                    Hp = curHp,
                    Id = guid,
                    Position = GetOwner().position.ToProto(),
                    Rotation = GetOwner().rotation.ToProto(),
                    SceneNumId = GetOwner().curSceneNumId,
                    Templateid = id,
                    Type = (int)0,
                    
                },
                Attrs =
                {

                }
            };
            GetAttributes().ForEach(attr =>
            {
                proto.Attrs.Add(new AttrInfo()
                {
                    AttrType = attr.attrType,
                    BasicValue = 0,
                    Value = attr.attrValue

                });

            });
            return proto;
        }
        public int GetbreakStage()
        {
            int breakStage = ResourceManager.charBreakNodeTable[breakNode].breakStage;
            return breakStage;
        }
        
        public CharInfo ToProto()
        {
            CharInfo info = new CharInfo()
            {
                Exp = xp,
                Level = level,
                IsDead = curHp < 1,
                
                Objid = guid,
                Templateid = id,
                CharType = CharType.DefaultType,
                OwnTime = 1,
                NormalSkill = id + "_NormalSkill",
                WeaponId = weaponGuid,
                PotentialLevel = potential,
                
                Talent = new()
                {
                    LatestBreakNode= breakNode,
                    LatestPassiveSkillNodes =
                    {
                        passiveSkillNodes
                    },
                    AttrNodes =
                    {
                        attrNodes
                    },
                    
                    LatestFactorySkillNodes =
                    {
                        factoryNodes
                    }
                },
                BattleMgrInfo = new()
                {
                    
                },
                BattleInfo = new()
                {
                    Hp = curHp,
                    Ultimatesp= ultimateSp,
                },
                SkillInfo = new()
                {
                    
                    NormalSkill = id + "_NormalSkill",
                    ComboSkill = id + "_ComboSkill",
                    UltimateSkill = id + "_UltimateSkill",
                    DispNormalAttackSkill = id + "_NormalAttack",
                    LevelInfo =
                    {
                        new SkillLevelInfo()
                        {
                            SkillId=id+"_NormalAttack",
                            SkillLevel=1,
                            SkillMaxLevel=1,
                            SkillEnhancedLevel=1
                        },
                        new SkillLevelInfo()
                        {
                            SkillId=id+"_NormalSkill",
                            SkillLevel=1,
                            SkillMaxLevel=1,
                            SkillEnhancedLevel=1
                        },
                        new SkillLevelInfo()
                        {
                            SkillId=id+"_UltimateSkill",
                            SkillLevel=1,
                            SkillMaxLevel=1,
                            SkillEnhancedLevel=1
                        },
                        new SkillLevelInfo()
                        {
                            SkillId=id+"_ComboSkill",
                            SkillLevel=1,
                            SkillMaxLevel=1,
                            SkillEnhancedLevel=1
                        },

                    }
                }
            };

            return info;
        }
        public (int,int,int) CalculateLevelAndGoldCost(int addedXp)
        {
            int gold = 0;
            int curLevel = this.level;
            while(addedXp >= ResourceManager.charLevelUpTable["" + curLevel].exp)
            {
                gold += ResourceManager.charLevelUpTable["" + curLevel].gold;
                addedXp -= ResourceManager.charLevelUpTable["" + curLevel].exp;
                curLevel++;
                if(curLevel >= 80)
                {
                    curLevel = 80;
                }
            }
            return (curLevel, gold, addedXp);
        }
        public void LevelUp(RepeatedField<ItemInfo> items)
        {
            int addedXp = 0;
            foreach (var item in items)
            {
                addedXp += ResourceManager.expItemDataMap[item.ResId].expGain * item.ResCount;
            }

            (int, int, int) CalculatedValues = CalculateLevelAndGoldCost(xp+addedXp);
            items.Add(new ItemInfo()
            {
                ResId = "item_gold",
                ResCount = CalculatedValues.Item2
            });
            if (GetOwner().inventoryManager.ConsumeItems(items))
            {
                this.level = CalculatedValues.Item1;
                this.xp= CalculatedValues.Item3;
                ScCharLevelUp levelUp = new()
                {
                    CharObjID = guid,
                    

                };
                ScCharSyncLevelExp synclevel = new()
                {
                    Exp = xp,
                    CharObjID = guid,
                    Level = level
                };
                GetOwner().Send(ScMessageId.ScCharSyncLevelExp, synclevel);
                GetOwner().Send(ScMessageId.ScCharLevelUp, levelUp);
                GetOwner().Send(new PacketScSyncWallet(GetOwner()));
            }
        }
    }
}
