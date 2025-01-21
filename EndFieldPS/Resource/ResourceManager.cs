
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static EndFieldPS.Resource.ResourceManager;

namespace EndFieldPS.Resource
{
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
        public static StrIdNumTable dialogIdTable = new();
        public static List<LevelData> levelDatas = new();


        public static int GetSceneNumIdFromLevelData(string name)
        {
            if (levelDatas.Find(a => a.id == name) == null) return 0;
            return levelDatas.Find(a => a.id == name).idNum;
        }
        public static void Init()
        {
            Server.Print("Loading resources");
            sceneAreaTable=JsonConvert.DeserializeObject<Dictionary<string, SceneAreaTable>>(File.ReadAllText("Excel/SceneAreaTable.json"));
            strIdNumTable = JsonConvert.DeserializeObject<StrIdNumTable>(File.ReadAllText("Excel/StrIdNumTable.json"));
            characterTable = JsonConvert.DeserializeObject<Dictionary<string, CharacterTable>>(File.ReadAllText("Excel/CharacterTable.json"));
            systemJumpTable = JsonConvert.DeserializeObject<Dictionary<string, SystemJumpTable>>(File.ReadAllText("Excel/SystemJumpTable.json"));
            settlementBasicDataTable = JsonConvert.DeserializeObject<Dictionary<string, SettlementBasicDataTable>>(File.ReadAllText("Excel/SettlementBasicDataTable.json"));
            blocMissionTable = JsonConvert.DeserializeObject<Dictionary<string, BlocMissionTable>>(File.ReadAllText("Excel/BlocMissionTable.json"));
            dialogTextTable = JsonConvert.DeserializeObject<Dictionary<string, DialogTextTable>>(File.ReadAllText("Excel/DialogTextTable.json"));
            gameSystemConfigTable = JsonConvert.DeserializeObject<Dictionary<string, GameSystemConfigTable>>(File.ReadAllText("Excel/GameSystemConfigTable.json"));
            wikiGroupTable = JsonConvert.DeserializeObject<Dictionary<string, WikiGroupTable>>(File.ReadAllText("Excel/WikiGroupTable.json"));
            dialogIdTable = JsonConvert.DeserializeObject<StrIdNumTable>(File.ReadAllText("Json/GameplayConfig/DialogIdTable.json"));
            blocUnlockTable = JsonConvert.DeserializeObject<Dictionary<string, object>>(File.ReadAllText("Excel/BlocUnlockTable.json"));
            gameMechanicTable= JsonConvert.DeserializeObject<Dictionary<string, GameMechanicTable>>(File.ReadAllText("Excel/GameMechanicTable.json"));
            weaponBasicTable = JsonConvert.DeserializeObject<Dictionary<string, WeaponBasicTable>>(File.ReadAllText("Excel/WeaponBasicTable.json"));
            missionAreaTable = JsonConvert.DeserializeObject<MissionAreaTable>(File.ReadAllText("Json/GameplayConfig/MissionAreaTable.json"));
            blocDataTable = JsonConvert.DeserializeObject<Dictionary<string, BlocDataTable>>(File.ReadAllText("Excel/BlocDataTable.json"));
            LoadLevelDatas();
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
            string directoryPath = @"Json/LevelData"; // Percorso della directory principale
            string[] jsonFiles = Directory.GetFiles(directoryPath, "*.json", SearchOption.AllDirectories);
            foreach(string json in jsonFiles)
            {
                LevelData data = JsonConvert.DeserializeObject<LevelData>(File.ReadAllText(json));
                levelDatas.Add(data);
                Print("Loading " + data.id);
            }

            Print($"Loaded {levelDatas.Count} LevelData");
        }
        public static int GetItemTemplateId(string item_id)
        {
            return strIdNumTable.item_id.dic[item_id];
        }
        public class MissionAreaTable
        {
            public Dictionary<string, Dictionary<string, object>> m_areas;
        }
        public class BlocDataTable
        {
            public string blocId;
        }
        public class BlocMissionTable
        {
            public string missionId;

        }
        public class GameMechanicTable
        {
            public string gameMechanicsId;
            public int difficulty;
            public string firstPassRewardId;
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
        public struct Vector3f
        {
            public float x;
            public float y;
            public float z;

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
        }
        public class StrIdNumTable
        {
            public StrIdDic skill_group_id;
            public StrIdDic item_id;
            public Dictionary<string, int> dialogStrToNum;
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
        public class WeaponBasicTable
        {
            public int weaponType;
            public string weaponId;
        }
        public class CharacterTable
        {
            public List<Attributes> attributes;
            public string charId;
            public int weaponType;
            public string engName;

        }
        public class Attributes
        {
            public int breakStage;
            public AttributeList Attribute;
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
        public static void Print(string text)
        {
            Logger.Log(text);
            Console.WriteLine($"[{Server.ColoredText("ResourceManager", "03fcce")}] " + text);
        }


    }
}
