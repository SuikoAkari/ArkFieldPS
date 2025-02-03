
using EndFieldPS.Resource.Table;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static EndFieldPS.Resource.ResourceManager;

namespace EndFieldPS.Resource
{
    public class IdConst
    {
        static System.Int32 ID_SERVER_SET_TYPE_BIT = 62;
        static System.Int32 ID_CLIENT_RUNTIME_SET_TYPE_BIT = 61;
        static System.Int32 ID_ROLE_INDEX_SHIFT = 57;
        static System.UInt64 LOGIC_ID_SEGMENT = 10000;
        static System.UInt64 MAX_LOGIC_ID_BOUND = 100000000;
        static System.Int32 LOCAL_ID_SEGMENT = 10000;
        static System.Int32 MAX_LOCAL_ID_BOUND = 100000000;
        static System.Int32 MAX_LEVEL_ID_BOUND = 100000;
        static System.UInt64 MAX_GLOBAL_ID = 10000000000000;
        static System.UInt64 MAX_RUNTIME_CLIENT_ID = 2305843009213693952;
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
        public static StrIdNumTable dialogIdTable = new();

        public static List<LevelData> levelDatas = new();

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
            LoadLevelDatas();
            if (missingResources)
            {
                Logger.PrintWarn("Missing some resources. The gameserver will probably crash.");
            }
        }
        public static CharGrowthTable.CharTalentNode GetTalentNode(string c, string id)
        {
            return charGrowthTable[c].talentNodeMap[id];
        }
        public static ItemTable GetItemTable(string id)
        {
            return itemTable[id];
        }
        public static LevelData GetLevelData(int sceneNumId)
        {
           return levelDatas.Find(e => e.idNum == sceneNumId);
        }
        public static LevelData GetLevelData(string sceneId)
        {
            if(levelDatas.Find(e => e.id == sceneId) == null)
            {
                return new LevelData();
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
                LevelData data = JsonConvert.DeserializeObject<LevelData>(ReadJsonFile(json));
                levelDatas.Add(data);
               // Print("Loading " + data.id);
            }

            Logger.Print($"Loaded {levelDatas.Count} LevelData");
        }
        public static int GetItemTemplateId(string item_id)
        {
            return strIdNumTable.item_id.dic[item_id];
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
        }
        public class WikiGroupTable
        {
            public List<WikiGroup> list;
        }
        public class WikiGroup
        {
            public string groupId;
        }
        public class GameSystemConfigTable
        {
            public int unlockSystemType;
            public string systemId;

        }
        public class LevelData
        {
            public string id;
            public int idNum;
            public string mapIdStr;
            public DefaultState defaultState;

            public Vector3f playerInitPos;
            public Vector3f playerInitRot;
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
