using ArkFieldPS.Game.Inventory;
using ArkFieldPS.Packets.Sc;
using ArkFieldPS.Protocol;
using ArkFieldPS.Resource;
using Google.Protobuf.Collections;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static ArkFieldPS.Resource.ResourceManager;
using static ArkFieldPS.Resource.ResourceManager.CharGrowthTable;
using static ArkFieldPS.Resource.ResourceManager.WeaponUpgradeTemplateTable;

namespace ArkFieldPS.Game.Character
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
        public Dictionary<int,ulong> equipCol = new() { { 0, 0 }, { 1, 0 }, { 2, 0 }, { 3, 0 } };
        public Character()
        {
           
        }
        
        public Character(ulong owner, string id) : this(owner, id, 1)
        {

        }
        public Dictionary<AttributeType, (double baseVal, double val)> CalcAttributes()
        {
            Dictionary<AttributeType, (double baseVal, double val)> attributes = new();
            foreach (var item in GetAttributes())
            {
                attributes.Add((AttributeType)item.attrType, (item.attrValue, item.attrValue));
            }
            Item weapon = GetOwner().inventoryManager.items.Find(w => w.guid == weaponGuid);
            if(weapon != null)
            {
                WeaponBasicTable wTable = ResourceManager.weaponBasicTable[weapon.id];
                WeaponUpgradeTemplateTable template = ResourceManager.weaponUpgradeTemplateTable[wTable.levelTemplateId];
                WeaponCurve curve=template.list.Find(c => c.weaponLv == weapon.level);
                attributes[AttributeType.Atk] = (attributes[AttributeType.Atk].baseVal + curve.baseAtk, attributes[AttributeType.Atk].baseVal + curve.baseAtk);

            }
            
            //Won't be very precise but for now
            foreach (var equip in equipCol)
            {
                Item EquipItem = GetOwner().inventoryManager.items.Find(e => e.guid == equip.Value);
                if (EquipItem != null)
                {
                    foreach (var modifier in EquipItem.GetEquipAttributeModifier())
                    {
                        switch (modifier.modifierType)
                        {
                            case ModifierType.BaseAddition:
                            case ModifierType.Addition:
                                attributes=SetValueDic(attributes, modifier.attrType, GetValueDic(attributes, modifier.attrType) + modifier.attrValue);
                                break;
                            case ModifierType.Multiplier:
                            case ModifierType.BaseMultiplier:
                                attributes=SetValueDic(attributes, modifier.attrType, GetValueDic(attributes, modifier.attrType) * 1 + modifier.attrValue);
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
            attributes[AttributeType.MaxHp] = (attributes[AttributeType.MaxHp].baseVal, attributes[AttributeType.MaxHp].val + attributes[AttributeType.Str].val * 10);
            return attributes;
        }
        public double GetValueDic(Dictionary<AttributeType, (double baseVal, double val)> dic, AttributeType type)
        {

            if (dic.ContainsKey(type))
            {
                return dic[type].val;
            }
            return 0;
        }
        public Dictionary<AttributeType, (double baseVal, double val)> SetValueDic(Dictionary<AttributeType, (double baseVal, double val)> dic,AttributeType type,double value)
        {
            if (dic.ContainsKey(type))
            {
                dic[type] = (dic[type].baseVal,value);
            }
            else
            {
                dic.Add(type, (0, value));
            }
            return dic;
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
            this.curHp = CalcAttributes()[AttributeType.MaxHp].val;
        }
        public int GetSkillMaxLevel()
        {
            if (GetBreakStage() == 0)
            {
                return 1;
            }else if (GetBreakStage() == 1)
            {
                return 3;
            }
            else if (GetBreakStage() == 2)
            {
                return 6;
            }
            else if (GetBreakStage() == 3)
            {
                return 9;
            }
            else if (GetBreakStage() == 4)
            {
                return 12;
            }
            else
            {
                return 12;
            }
        }
        public List<ResourceManager.Attribute> GetAttributes()
        {
            int lev = level - 1 + GetBreakStage();
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
                                        Level=GetSkillMaxLevel(),
                                        Source=BattleSkillSource.Default,
                                        PotentialLv=GetSkillMaxLevel(),
                                        SkillId=id+"_NormalSkill",
                                    },
                                    new ServerSkill()
                                    {
                                        Blackboard = new()
                                        {

                                        },
                                        InstId=GetOwner().random.Next(),
                                        Level=GetSkillMaxLevel(),
                                        Source=BattleSkillSource.Default,
                                        PotentialLv=GetSkillMaxLevel(),
                                        SkillId=id+"_ComboSkill",
                                    },
                                    new ServerSkill()
                                    {
                                        Blackboard = new()
                                        {

                                        },
                                        InstId=GetOwner().random.Next(),
                                        Level=GetSkillMaxLevel(),
                                        Source=BattleSkillSource.Default,
                                        PotentialLv=GetSkillMaxLevel(),
                                        SkillId=id+"_UltimateSkill",
                                    },
                                    new ServerSkill()
                                    {
                                        Blackboard = new()
                                        {

                                        },
                                        InstId=GetOwner().random.Next(),
                                        Level=GetSkillMaxLevel(),
                                        Source=BattleSkillSource.Default,
                                        PotentialLv=GetSkillMaxLevel(),
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
            foreach(var attr in CalcAttributes())
            {
                proto.Attrs.Add(new AttrInfo()
                {
                    AttrType = (int)attr.Key,
                    BasicValue = attr.Value.baseVal,
                    Value = attr.Value.val
                    
                });
            }
            return proto;
        }
        public int GetBreakStage()
        {
            if (ResourceManager.charBreakNodeTable.ContainsKey(breakNode))
            {
                int breakStage = ResourceManager.charBreakNodeTable[breakNode].breakStage;
                return breakStage;
            }
            else
            {
                return 0;
            }

        }
        public bool IsEquipped(ulong equipGuid)
        {
            return equipCol.Values.Contains(equipGuid);
        }
        public Dictionary<int,ulong> GetEquipCol()
        {
            Dictionary<int, ulong> equips = new();
            foreach(var item in equipCol)
            {
                if (item.Value != 0)
                {
                    equips.Add(item.Key,item.Value);
                }
            }
            return equips;
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
                EquipCol =
                {
                    GetEquipCol()
                },
                
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
                            SkillLevel=GetSkillMaxLevel(),
                            SkillMaxLevel=GetSkillMaxLevel(),
                            SkillEnhancedLevel=GetSkillMaxLevel()
                        },
                        new SkillLevelInfo()
                        {
                            SkillId=id+"_NormalSkill",
                            SkillLevel=GetSkillMaxLevel(),
                            SkillMaxLevel=GetSkillMaxLevel(),
                            SkillEnhancedLevel=GetSkillMaxLevel()
                        },
                        new SkillLevelInfo()
                        {
                            SkillId=id+"_UltimateSkill",
                            SkillLevel=GetSkillMaxLevel(),
                            SkillMaxLevel=GetSkillMaxLevel(),
                            SkillEnhancedLevel=GetSkillMaxLevel()
                        },
                        new SkillLevelInfo()
                        {
                            SkillId=id+"_ComboSkill",
                            SkillLevel=GetSkillMaxLevel(),
                            SkillMaxLevel=GetSkillMaxLevel(),
                            SkillEnhancedLevel=GetSkillMaxLevel()
                        },

                    }
                }
            };
            Item wep = GetOwner().inventoryManager.items.Find(w => w.guid == weaponGuid);
            if (wep != null)
            {
                //TODO weapon skills
            }
            foreach (ulong equipGuid in equipCol.Values)
            {
                Item item = GetOwner().inventoryManager.items.Find(i => i.guid == equipGuid);
                if (item != null)
                {
                    string equipSuitId = ResourceManager.GetEquipSuitTableKey(item.id);
                    if (equipSuitId.Length > 0)
                    {
                        if (info.EquipSuit.ContainsKey(equipSuitId))
                        {
                            info.EquipSuit[equipSuitId] += 1;
                        }
                        else
                        {
                            info.EquipSuit.Add(equipSuitId, 1);
                        }
                    }

                }
            }
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
