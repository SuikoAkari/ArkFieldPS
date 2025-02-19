﻿
using ArkFieldPS.Resource.Table;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ArkFieldPS.Resource.ResourceManager;
using static ArkFieldPS.Resource.ResourceManager.LevelScene;
using static ArkFieldPS.Resource.ResourceManager.LevelScene.LevelData;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ArkFieldPS.Resource
{
    public class IdConst
    {
        static System.Int32 ID_SERVER_SET_TYPE_BIT = 62;
        static System.Int32 ID_CLIENT_RUNTIME_SET_TYPE_BIT = 61;
        static System.Int32 ID_ROLE_INDEX_SHIFT = 57;
        public static System.UInt64 LOGIC_ID_SEGMENT = 10000;
        public static System.UInt64 MAX_LOGIC_ID_BOUND = 100000000;
        public static System.Int32 LOCAL_ID_SEGMENT = 10000;
        public static System.Int32 MAX_LOCAL_ID_BOUND = 100000000;
        public static System.Int32 MAX_LEVEL_ID_BOUND = 100000;
        public static System.UInt64 MAX_GLOBAL_ID = 10000000000000;
        public static System.UInt64 MAX_RUNTIME_CLIENT_ID = 2305843009213693952;
    }
    //TODO Move all tables to separated class
    public class ResourceManager
    {
        public static Dictionary<string, SceneAreaTable> sceneAreaTable = new();
        public static StrIdNumTable strIdNumTable = new StrIdNumTable();
        public static Dictionary<string, CharacterTable> characterTable = new();
        public static Dictionary<string, SystemJumpTable> systemJumpTable = new();
        public static Dictionary<string, SettlementBasicDataTable> settlementBasicDataTable = new();
        public static Dictionary<string, BlocMissionTable> blocMissionTable = new();
        public static MissionAreaTable missionAreaTable = new();
        public static Dictionary<string, DialogTextTable> dialogTextTable = new();
        public static Dictionary<string, GameSystemConfigTable> gameSystemConfigTable = new();
        public static Dictionary<string, WikiGroupTable> wikiGroupTable = new();
        public static Dictionary<string, object> blocUnlockTable = new();
        public static Dictionary<string, GameMechanicTable> gameMechanicTable = new();
        public static Dictionary<string, WeaponBasicTable> weaponBasicTable= new();
        public static Dictionary<string, BlocDataTable> blocDataTable = new();
        public static Dictionary<string, ItemTable> itemTable = new();
        public static Dictionary<string, DomainDataTable> domainDataTable = new();
        public static Dictionary<string, CollectionTable> collectionTable = new();
        public static Dictionary<string, GachaCharPoolTable> gachaCharPoolTable = new();
        public static Dictionary<string, CharBreakNodeTable> charBreakNodeTable = new();
        public static Dictionary<string, EnemyAttributeTemplateTable> enemyAttributeTemplateTable = new();
        public static Dictionary<string, CharLevelUpTable> charLevelUpTable = new();
        public static Dictionary<string, ExpItemDataMap> expItemDataMap = new();
        public static Dictionary<string, CharGrowthTable> charGrowthTable = new();
        public static Dictionary<string, WeaponUpgradeTemplateTable> weaponUpgradeTemplateTable = new();
        public static Dictionary<string, GachaCharPoolContentTable> gachaCharPoolContentTable = new();
        public static Dictionary<string, GachaCharPoolTypeTable> gachaCharPoolTypeTable = new();
        public static Dictionary<string, EnemyTable> enemyTable = new();
        public static Dictionary<string, EquipTable> equipTable = new();
        public static Dictionary<string, EquipSuitTable> equipSuitTable = new();
        public static Dictionary<string, SpaceShipCharBehaviourTable> spaceShipCharBehaviourTable = new();
        public static Dictionary<string, SpaceshipRoomInsTable> spaceshipRoomInsTable = new();
        public static Dictionary<string, DungeonTable> dungeonTable = new();
        public static Dictionary<string, LevelGradeTable> levelGradeTable = new();
        public static Dictionary<string, RewardTable> rewardTable = new();  
        public static StrIdNumTable dialogIdTable = new();
        public static Dictionary<string, LevelShortIdTable> levelShortIdTable = new();  
        public static List<LevelScene> levelDatas = new();

        public static int GetSceneNumIdFromLevelData(string name)
        {
            if (levelDatas.Find(a => a.id == name) == null) return 0;
            return levelDatas.Find(a => a.id == name).idNum;
        }
        public static bool missingResources = false;
        public static string ReadJsonFile(string path)
        {
            try
            {
                return File.ReadAllText(path);
            }
            catch(Exception e)
            {
                Logger.PrintError($"Error occured while loading {path} Err: {e.Message}");
                missingResources = true;
                return "";
            }
            
        }
        public static void Init()
        {
            Logger.Print("Loading TableCfg resources");
            sceneAreaTable=JsonConvert.DeserializeObject<Dictionary<string, SceneAreaTable>>(ReadJsonFile("TableCfg/SceneAreaTable.json"));
            strIdNumTable = JsonConvert.DeserializeObject<StrIdNumTable>(ReadJsonFile("TableCfg/StrIdNumTable.json"));
            characterTable = JsonConvert.DeserializeObject<Dictionary<string, CharacterTable>>(ReadJsonFile("TableCfg/CharacterTable.json"));
            systemJumpTable = JsonConvert.DeserializeObject<Dictionary<string, SystemJumpTable>>(ReadJsonFile("TableCfg/SystemJumpTable.json"));
            settlementBasicDataTable = JsonConvert.DeserializeObject<Dictionary<string, SettlementBasicDataTable>>(ReadJsonFile("TableCfg/SettlementBasicDataTable.json"));
            blocMissionTable = JsonConvert.DeserializeObject<Dictionary<string, BlocMissionTable>>(ReadJsonFile("TableCfg/BlocMissionTable.json"));
            dialogTextTable = JsonConvert.DeserializeObject<Dictionary<string, DialogTextTable>>(ReadJsonFile("TableCfg/DialogTextTable.json"));
            gameSystemConfigTable = JsonConvert.DeserializeObject<Dictionary<string, GameSystemConfigTable>>(ReadJsonFile("TableCfg/GameSystemConfigTable.json"));
            wikiGroupTable = JsonConvert.DeserializeObject<Dictionary<string, WikiGroupTable>>(ReadJsonFile("TableCfg/WikiGroupTable.json"));
            dialogIdTable = JsonConvert.DeserializeObject<StrIdNumTable>(ReadJsonFile("Json/GameplayConfig/DialogIdTable.json"));
            blocUnlockTable = JsonConvert.DeserializeObject<Dictionary<string, object>>(ReadJsonFile("TableCfg/BlocUnlockTable.json"));
            gameMechanicTable= JsonConvert.DeserializeObject<Dictionary<string, GameMechanicTable>>(ReadJsonFile("TableCfg/GameMechanicTable.json"));
            weaponBasicTable = JsonConvert.DeserializeObject<Dictionary<string, WeaponBasicTable>>(ReadJsonFile("TableCfg/WeaponBasicTable.json"));
            missionAreaTable = JsonConvert.DeserializeObject<MissionAreaTable>(ReadJsonFile("Json/GameplayConfig/MissionAreaTable.json"));
            blocDataTable = JsonConvert.DeserializeObject<Dictionary<string, BlocDataTable>>(ReadJsonFile("TableCfg/BlocDataTable.json"));
            itemTable = JsonConvert.DeserializeObject<Dictionary<string, ItemTable>>(ReadJsonFile("TableCfg/ItemTable.json"));
            domainDataTable = JsonConvert.DeserializeObject<Dictionary<string, DomainDataTable>>(ReadJsonFile("TableCfg/DomainDataTable.json"));
            collectionTable = JsonConvert.DeserializeObject<Dictionary<string, CollectionTable>>(ReadJsonFile("TableCfg/CollectionTable.json"));
            gachaCharPoolTable = JsonConvert.DeserializeObject<Dictionary<string, GachaCharPoolTable>>(ReadJsonFile("TableCfg/GachaCharPoolTable.json"));
            charBreakNodeTable = JsonConvert.DeserializeObject<Dictionary<string, CharBreakNodeTable>>(ReadJsonFile("TableCfg/CharBreakNodeTable.json"));
            enemyAttributeTemplateTable = JsonConvert.DeserializeObject<Dictionary<string, EnemyAttributeTemplateTable>>(ReadJsonFile("TableCfg/EnemyAttributeTemplateTable.json"));
            charLevelUpTable = JsonConvert.DeserializeObject<Dictionary<string, CharLevelUpTable>>(ReadJsonFile("TableCfg/CharLevelUpTable.json"));
            expItemDataMap = JsonConvert.DeserializeObject<Dictionary<string, ExpItemDataMap>>(ReadJsonFile("TableCfg/ExpItemDataMap.json"));
            charGrowthTable = JsonConvert.DeserializeObject<Dictionary<string, CharGrowthTable>>(ReadJsonFile("TableCfg/CharGrowthTable.json"));
            weaponUpgradeTemplateTable = JsonConvert.DeserializeObject<Dictionary<string, WeaponUpgradeTemplateTable>>(ReadJsonFile("TableCfg/WeaponUpgradeTemplateTable.json"));
            gachaCharPoolContentTable = JsonConvert.DeserializeObject<Dictionary<string, GachaCharPoolContentTable>>(ReadJsonFile("TableCfg/GachaCharPoolContentTable.json"));
            enemyTable = JsonConvert.DeserializeObject<Dictionary<string, EnemyTable>>(ReadJsonFile("TableCfg/EnemyTable.json"));
            gachaCharPoolTypeTable = JsonConvert.DeserializeObject<Dictionary<string, GachaCharPoolTypeTable>>(ReadJsonFile("TableCfg/GachaCharPoolTypeTable.json"));
            equipTable = JsonConvert.DeserializeObject<Dictionary<string, EquipTable>>(ReadJsonFile("TableCfg/EquipTable.json"));
            spaceShipCharBehaviourTable = JsonConvert.DeserializeObject<Dictionary<string, SpaceShipCharBehaviourTable>>(ReadJsonFile("TableCfg/SpaceShipCharBehaviourTable.json"));
            spaceshipRoomInsTable = JsonConvert.DeserializeObject<Dictionary<string, SpaceshipRoomInsTable>>(ReadJsonFile("TableCfg/SpaceshipRoomInsTable.json"));
            dungeonTable = JsonConvert.DeserializeObject<Dictionary<string, DungeonTable>>(ReadJsonFile("TableCfg/DungeonTable.json"));
            equipSuitTable = JsonConvert.DeserializeObject<Dictionary<string, EquipSuitTable>>(ReadJsonFile("TableCfg/EquipSuitTable.json"));
            levelGradeTable = JsonConvert.DeserializeObject<Dictionary<string, LevelGradeTable>>(ReadJsonFile("TableCfg/LevelGradeTable.json"));
            levelShortIdTable = JsonConvert.DeserializeObject<Dictionary<string, LevelShortIdTable>>(ReadJsonFile("DynamicAssets/gamedata/gameplayconfig/jsoncfg/LevelShortIdTable.json"));
            rewardTable = JsonConvert.DeserializeObject<Dictionary<string, RewardTable>>(ReadJsonFile("TableCfg/RewardTable.json"));
            LoadLevelDatas(); 
            if (missingResources)
            {
                Logger.PrintWarn("Missing some resources. The gameserver will probably crash.");
            }
        }
        public static List<int> GetAllShortIds()
        {
            List<int> IDS = new List<int>();
            foreach (LevelShortIdTable table in levelShortIdTable.Values)
            {
                IDS.AddRange(table.ids.Values);
            }
            return IDS;
        }
        public static string GetEquipSuitTableKey(string suitTableId)
        {
            foreach(var item in equipSuitTable)
            {
                if (item.Value.equipList.Contains(suitTableId))
                {
                    return item.Key;    
                }
            }
            return "";
        }
        public static CharGrowthTable.CharTalentNode GetTalentNode(string c, string id)
        {
            return charGrowthTable[c].talentNodeMap[id];
        }
        public static ItemTable GetItemTable(string id)
        {
            return itemTable[id];
        }
        public static LevelScene GetLevelData(int sceneNumId)
        {
           return levelDatas.Find(e => e.idNum == sceneNumId);
        }
        public static List<ulong> GetBitset(List<int> ids)
        {
            Dictionary<string, ulong> stringToIdMap = new Dictionary<string, ulong>(); // Mappa inversa
            HashSet<ulong> result = new HashSet<ulong>();
            int i = 0;
            foreach (int var in ids) // values è l'insieme di stringhe
            {
                ulong bitPos = (ulong)i;
                ulong baseBit = bitPos / 64 * 64;
                ulong bitMask = 1UL << (int)(bitPos % 64);

                if (!result.Contains(baseBit))
                    result.Add(baseBit);

                result.Add(result.Contains(baseBit) ? result.First(x => x == baseBit) | bitMask : bitMask);
                i++;
            }
            return result.ToList();
        }
        public static List<string> CalculateWaypointIdsBitset()
        {
            Logger.Print("getting waypoints");
            string bitset = "";
            List<int> waypoints = GetAllShortIds();
            int maxValue = waypoints.Max();
            int chunkSize = 64;
            Logger.Print("max waypoint id:"+maxValue);
            for (int i = 0; i <= maxValue; i++)
            {
                if (waypoints.Contains(i))
                {
                    bitset += "1";
                }
                else
                {
                    bitset += "0";
                }
            }
            List<string> chunks = new List<string>();

            for (int i = 0; i < bitset.Length; i += chunkSize)
            {
                chunks.Add(bitset.Substring(i, Math.Min(chunkSize, bitset.Length - i)));
            }

            return chunks;
        }
        public static List<string> CalculateDocsIdsBitset()
        {
            string bitset = "";
            int maxValue = strIdNumTable.char_doc_id.dic.Values.Max();
            int chunkSize = 64;
            for (int i = 0; i <= maxValue; i++)
            {
                if (strIdNumTable.char_doc_id.dic.Values.ToList().Contains(i))
                {
                    bitset += "1";
                }
                else
                {
                    bitset += "0";
                }
            }
            List<string> chunks = new List<string>();

            for (int i = 0; i < bitset.Length; i += chunkSize)
            {
                chunks.Add(bitset.Substring(i, Math.Min(chunkSize, bitset.Length - i)));
            }

            return chunks;
        }

        public static List<string> CalculateBitsets(List<int> ids)
        {
            string bitset = "";
            int maxValue = ids.Max();
            int chunkSize = 64;
            for (int i = 0; i <= maxValue; i++)
            {
                if (ids.Contains(i))
                {
                    bitset += "1";
                }
                else
                {
                    bitset += "0";
                }
            }
            List<string> chunks = new List<string>();

            for (int i = 0; i < bitset.Length; i += chunkSize)
            {
                chunks.Add(bitset.Substring(i, Math.Min(chunkSize, bitset.Length - i)));
            }

            return chunks;
        }
        public static List<string> CalculateVoiceIdsBitset()
        {
            string bitset = "";
            int maxValue = strIdNumTable.char_voice_id.dic.Values.Max();
            int chunkSize = 64;
            for (int i = 0; i <= maxValue; i++)
            {
                if (strIdNumTable.char_voice_id.dic.Values.ToList().Contains(i))
                {
                    bitset += "1";
                }
                else
                {
                    bitset += "0";
                }
            }
            List<string> chunks = new List<string>();

            for (int i = 0; i < bitset.Length; i += chunkSize)
            {
                chunks.Add(bitset.Substring(i, Math.Min(chunkSize, bitset.Length - i)));
            }

            return chunks;
        }
        public static List<ulong> ToLongBitsetValue(List<string> binaryString)
        {
            //string binaryString = "1101"; // Numero binario
            List<ulong> bits = new List<ulong>();

            foreach (string bitset in binaryString)
            {
                ulong decimalValue = (ulong)Convert.ToUInt64(bitset, 2); // Converti in decimale
                bits.Add(decimalValue);
            }
            
            return bits;
        }
        public static LevelScene GetLevelData(string sceneId)
        {
            if(levelDatas.Find(e => e.id == sceneId) == null)
            {
                return new LevelScene();
            }
            return levelDatas.Find(e => e.id==sceneId);
        }
        public static string GetDefaultWeapon(int type)
        {
            return weaponBasicTable.Values.ToList().Find(x => x.weaponType == type).weaponId;  
        }
        public static void LoadLevelDatas()
        {
            Logger.Print("Loading LevelData resources");
            string directoryPath = @"Json/LevelData";
            string[] jsonFiles = Directory.GetFiles(directoryPath, "*.json", SearchOption.AllDirectories);
            foreach(string json in jsonFiles)
            {
                if (json.Contains("_lv_data"))
                {
                    continue;
                }
                LevelScene data = JsonConvert.DeserializeObject<LevelScene>(ReadJsonFile(json));
                data.levelData = new();
                int i = 0;
                foreach (string path in data.levelDataPaths)
                {
                    
                    try
                    {

                        LevelData data_lv = JsonConvert.DeserializeObject<LevelData>(File.ReadAllText("Json/" + path));
                        data.levelData.Merge(data_lv);
                        i++;
                    }
                    catch (Exception ex)
                    {
                        //Logger.PrintError(ex.Message);
                        Logger.PrintWarn("Missing levelData natural spawns file for scene " + data.mapIdStr + " path: " + path);
                        
                    }
                }
               
                
                
                levelDatas.Add(data);
               // Print("Loading " + data.id);
            }

            Logger.Print($"Loaded {levelDatas.Count} LevelData");
        }
        public static int GetItemTemplateId(string item_id)
        {
            return strIdNumTable.item_id.dic[item_id];
        }

        public class DungeonTable
        {
            public string dungeonId;
            public string sceneId;

        }
        public class EquipSuitTable
        {
            public List<string> equipList;
        }
        public class BlocMissionTable
        {
            public string missionId;

        }
        public class ExpItemDataMap
        {
            public int expGain;
        }
        public class CharLevelUpTable
        {
            public int exp;
            public int gold;
        }
        public class GameMechanicTable
        {
            public string gameMechanicsId;
            public int difficulty;
            public string firstPassRewardId;
        }
        public class AttributeModifier
        {
            public AttributeType attrType;
            public double attrValue;
            public ModifierType modifierType;
            public int modifyAttributeType;
        }
        public class EquipTable
        {
            public string domainId;
            public string itemId;
            public ulong minWearLv;
            public int partType;
            public string suitID;
            public List<AttributeModifier> displayAttrModifiers; 
            public List<AttributeModifier> attrModifiers;
            public AttributeModifier displayBaseAttrModifier;
        }
        public class WikiGroupTable
        {
            public List<WikiGroup> list;
        }
        public class WikiGroup
        {
            public string groupId;
        }
        public class SpaceShipCharBehaviourTable
        {
            public string charId;
            public string npcId;
        }
        public class SpaceshipRoomInsTable
        {
            public string id;
            public int roomType;
        }
        public class RewardTable
        {
            public string rewardId;

            public List<ItemBundle> itemBundles;
            public class ItemBundle
            {
                public int count;
                public string id;
            }
        }
        public class LevelShortIdTable
        {
            public string sceneName;
            public Dictionary<long, int> ids;
        }
        public class GameSystemConfigTable
        {
            public int unlockSystemType;
            public string systemId;

        }
        public class LevelScene
        {
            public string id;
            public int idNum;
            public string mapIdStr;
            public DefaultState defaultState;

            public Vector3f playerInitPos;
            public Vector3f playerInitRot;
            public List<string> levelDataPaths;
            [JsonIgnore]
            public LevelData levelData;
            public class LevelData
            {
                public string sceneId="";
                public int levelIdNum;
                public List<LevelEnemyData> enemies=new();
                public List<LevelInteractiveData> interactives = new();
                public List<LevelNpcData> npcs = new();
                public List<LevelScriptData> levelScripts = new();
                public List<WorldWayPointSets> worldWayPointSets = new();
                public List<LevelFactoryRegionData> factoryRegions = new();
                public void Merge(LevelData other)
                {
                    this.sceneId = other.sceneId;
                    this.levelIdNum = other.levelIdNum;
                    this.enemies.AddRange(other.enemies);
                    this.interactives.AddRange(other.interactives);
                    this.npcs.AddRange(other.npcs);
                    this.levelScripts.AddRange(other.levelScripts);
                    this.worldWayPointSets.AddRange(other.worldWayPointSets);
                    this.factoryRegions.AddRange(other.factoryRegions);
                }
                
                public class WorldWayPointSets
                {
                    public int id;
                    public Dictionary<string, int> pointIdToIndex = new();
                }
                public class LevelScriptData
                {
                    public ulong scriptId;
                    public List<ParamKeyValue> properties;
                }
                public class LevelFactoryRegionData
                {
                    public ulong levelLogicId;
                    public ulong belongLevelScriptId;
                    public ObjectType entityType;
                    public string entityDataIdKey;
                    public bool defaultHide;
                    public Vector3f position;
                    public Vector3f rotation;
                    public Vector3f scale;
                    public bool forceLoad;
                    public string regionId;
                }
                public class LevelNpcData
                {
                    public ulong levelLogicId;
                    public ulong belongLevelScriptId;
                    public ObjectType entityType;
                    public string entityDataIdKey;
                    public bool defaultHide;
                    public Vector3f position;
                    public Vector3f rotation;
                    public Vector3f scale;
                    public bool forceLoad;
                    public bool doPatrol;
                    public string npcGroupId;
                }
                public class LevelInteractiveData
                {
                    public ulong levelLogicId;
                    public ulong belongLevelScriptId;
                    public ObjectType entityType;
                    public string entityDataIdKey;
                    public bool defaultHide;
                    public Vector3f position;
                    public Vector3f rotation;
                    public Vector3f scale;
                    public bool forceLoad;
                    public int level;
                    public int dependencyGroupId;
                    public List<ParamKeyValue> properties;
                    public Dictionary<InteractiveComponentType, List<ParamKeyValue>> componentProperties = new();
                }
                public class ParamKeyValue
                {
                    public string key;
                    public ParamValue value;

                    public DynamicParameter ToProto()
                    {
                        DynamicParameter param = new()
                        {
                            RealType = (int)value.type,
                            ValueType= (int)value.type,
                            
                        };
                        foreach(var val in value.valueArray)
                        {
                            switch (value.type)
                            {
                                case ParamRealType.LangKey:
                                    param.ValueStringList.Add(val.valueString);
                                    param.ValueType = (int)ParamValueType.String;
                                    break;
                                case ParamRealType.LangKeyList:
                                    param.ValueStringList.Add(val.valueString);
                                    param.ValueType = (int)ParamValueType.StringList;
                                    break;
                                case ParamRealType.String:
                                    param.ValueStringList.Add(val.valueString);
                                    param.ValueType = (int)ParamValueType.String;
                                    break;
                                case ParamRealType.StringList:
                                    param.ValueStringList.Add(val.valueString);
                                    param.ValueType = (int)ParamValueType.StringList;
                                    break;
                                case ParamRealType.Vector3:
                                    param.ValueFloatList.Add(val.ToFloat());
                                    param.ValueType = (int)ParamValueType.FloatList;
                                    break;
                                 case ParamRealType.Float:
                                     param.ValueFloatList.Add(val.ToFloat());
                                    param.ValueType = (int)ParamValueType.Float;
                                    break;
                                case ParamRealType.FloatList:
                                    param.ValueFloatList.Add(val.ToFloat());
                                    param.ValueType = (int)ParamValueType.FloatList;
                                    break;
                                case ParamRealType.Int:
                                     param.ValueIntList.Add(val.valueBit64);
                                    param.ValueType = (int)ParamValueType.Int;
                                    break;
                                case ParamRealType.IntList:
                                    param.ValueIntList.Add(val.valueBit64);
                                    param.ValueType = (int)ParamValueType.IntList;
                                    break;
                                case ParamRealType.Bool:
                                    param.ValueBoolList.Add(false);
                                    param.ValueType = (int)ParamValueType.Bool;
                                    break;
                                case ParamRealType.Vector3List:
                                    param.ValueFloatList.Add(val.ToFloat());
                                    param.ValueType = (int)ParamValueType.FloatList;
                                    break;
                                case ParamRealType.BoolList:
                                    param.ValueBoolList.Add(false);
                                    param.ValueType = (int)ParamValueType.BoolList;
                                    break;
                                case ParamRealType.EntityPtr:
                                    param.ValueIntList.Add(val.valueBit64);
                                    param.ValueType = (int)ParamValueType.Int;
                                    break;
                                case ParamRealType.UInt64:
                                    param.ValueIntList.Add(val.valueBit64);
                                    param.ValueType = (int)ParamValueType.Int;
                                    break;
                                default:
                                    return null;
                                    break;
                            }
                        }
                        
                        return param;
                    }

                    public class ParamValue
                    {
                        public ParamRealType type;
                        public ParamValueAtom[] valueArray;
                    }
                    public class ParamValueAtom
                    {
                        public long valueBit64;
                        public string valueString;

                        public float ToFloat()
                        {
                            int intValueFromBit64 = (int)valueBit64; // Converti long in int
                            float floatValueFromBit64 = BitConverter.ToSingle(BitConverter.GetBytes(intValueFromBit64), 0);
                            return floatValueFromBit64;
                        }
                        public int ToInt()
                        {
                            int intValueFromBit64 = (int)valueBit64; // Converti long in int
                           
                            return intValueFromBit64;
                        }
                    }
                }
                public class LevelEnemyData
                {
                    public ulong levelLogicId;
                    public ulong belongLevelScriptId;
                    public ObjectType entityType;
                    public string entityDataIdKey;
                    public bool defaultHide;
                    public Vector3f position;
                    public Vector3f rotation;
                    public Vector3f scale;
                    public bool forceLoad;
                    public int level;
                    public string overrideAIConfig;
                    public int enemyGroupId;
                    public bool respawnable;

                }
            }
        }
        public class Vector3f
        {
            public float x;
            public float y;
            public float z;
            public Vector3f()
            {

            }
            public Vector3f(Vector v) : this()
            {
                this.x=v.X; this.y=v.Y; this.z=v.Z; 
            }

            public Vector ToProto()
            {
                return new Vector()
                {
                    X=x,
                    Y=y,
                    Z=z,
                };
            }
        }
        public class DefaultState
        {
            public string sourceSceneName;
        }
        public class SettlementBasicDataTable
        {
            public string settlementId;
            public string domainId;
        }
        public class StrIdNumTable
        {
            public StrIdDic skill_group_id;
            public StrIdDic item_id;
            public Dictionary<string, int> dialogStrToNum;
            public StrIdDic chapter_map_id;
            public StrIdDic char_voice_id;
            public StrIdDic char_doc_id;
            public StrIdDic area_id;
            public StrIdDic map_mark_temp_id;
            public StrIdDic wiki_id;
        }
        public class GachaCharPoolTable
        {
            public string id;
            public List<string> upCharIds;
            public int type;

        }
        public class GachaCharPoolTypeTable
        {
            public int type;
            public int hardGuarantee;
            public int softGuarantee;
        }
        public class GachaCharPoolContentTable
        {
            public List<GachaCharPoolItem> list;
        }
        public class GachaCharPoolItem
        {
            public string charId;
            public string isHardGuaranteeItem;
            public int starLevel;
        }
        public class DialogTextTable
        {

        }
        public class SystemJumpTable
        {
            public int bindSystem;
        }
        public class StrIdDic
        {
            public Dictionary<string, int> dic;
        }
        public class SceneAreaTable
        {
            public string areaId;
            public string sceneId;
            public int areaIndex;
        }
        public class EnemyTable
        {
            public string attrTemplateId;
            public string enemyId;
            public string templateId;
        }
        public class ItemTable
        {
            public ItemValuableDepotType valuableTabType;
            public string id;
            public int maxStackCount;
            public bool backpackCanDiscard;
            public string modelKey;
        }
        public class WeaponBasicTable
        {
            public int weaponType;
            public string weaponId;
            public string levelTemplateId;
        }
        public class WeaponUpgradeTemplateTable
        {
            public List<WeaponCurve> list;

            public class WeaponCurve
            {
                public float baseAtk;
                public ulong lvUpExp;
                public ulong lvUpGold;
                public ulong weaponLv;
            }
        }
        public class DomainDataTable
        {
            public string domainId;
            public int sortId;
            public List<string> levelGroup;
            public List<string> settlementGroup;
        }
        public class CollectionTable
        {
            public string prefabId;
        }
        public class CharBreakNodeTable
        {
            public int breakStage;
            public string nodeId;
        }
        public class EnemyAttributeTemplateTable
        {
            public string templateId;
            public List<Attributes> levelDependentAttributes;
            public AttributeList levelIndependentAttributes;
        }
        public class CharGrowthTable
        {
            public Dictionary<string,CharTalentNode> talentNodeMap;
            public string defaultWeaponId;

            public class CharTalentNode
            {
                public string nodeId;
                public TalentNodeType nodeType;
                public List<RequireItem> requiredItem;
            }
        }
        public class RequireItem
        {
            public string id;
            public int count;
        }
        public class CharacterTable
        {
            public List<Attributes> attributes;
            public string charId;
            public int weaponType;
            public string engName;
            public int rarity;

        }
        public class Attributes
        {
            public int breakStage;
            public AttributeList Attribute;
            //Enemy
            public List<Attribute> attrs; 
        }
        public class AttributeList
        {
            public List<Attribute> attrs;
        }
        public class Attribute
        {
            public int attrType;
            public double attrValue;
        }


    }
}
