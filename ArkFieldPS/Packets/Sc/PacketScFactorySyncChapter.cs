using ArkFieldPS.Game.Inventory;
using ArkFieldPS.Network;
using ArkFieldPS.Protocol;
using ArkFieldPS.Resource;
using ArkFieldPS.Resource.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static ArkFieldPS.Resource.ResourceManager;

namespace ArkFieldPS.Packets.Sc
{
    public class PacketScFactorySyncChapter : Packet
    {

        public PacketScFactorySyncChapter(Player client, string chapterId) {

            string json = File.ReadAllText("ScFactorySyncChapter.json");
            //ScFactorySyncChapter chapter = Newtonsoft.Json.JsonConvert.DeserializeObject<ScFactorySyncChapter>(json);
            ScFactorySyncChapter chapter = new()
            {
                ChapterId = chapterId,

                Tms = DateTime.UtcNow.ToUnixTimestampMilliseconds(),

                Nodes =
                {
                   new ScdFacNode()
                   {
                       NodeId=1,
                       NodeType=1,
                       TemplateId="__inventory__",
                       Transform = new()
                       {
                           Position=new(),
                           Direction=new(),
                           MapId=0,

                       },
                       InstKey="",
                       IsDeactive=true,
                       Power = new()
                       {

                       },
                       ComponentPos={
                           {11, 1}
                       },
                       Components =
                       {
                           new ScdFacCom()
                           {
                               ComponentId=1,
                               ComponentType=4,
                               Inventory = new()
                               {
                                   Items =
                                   {
                                       //client.inventoryManager.GetInventoryChapter(chapterId)
                                   }
                               },
                               
                           }
                       }
                   }
                },
                Blackboard = new()
                {
                    
                    Power = new()
                    {
                        PowerGen = 0,
                        PowerSaveMax=0,
                        PowerSaveCurrent=0,
                        PowerCost=0
                    },
                    InventoryNodeId=1
                },
                Statistic = new()
                {
                    LastDay = new()
                    {
                        
                    },
                    Other = new()
                    {
                        
                    }
                },
                PinBoard = new()
                {
                    Cards =
                    {
                        
                    },
                    
                },
                Maps =
                {
                    
                },
                Scenes =
                {
                },
                
                
            };
            foreach (var item in levelDatas)
            {
                
            }
            domainDataTable[chapterId].levelGroup.ForEach(levelGroup =>
            {
                chapter.Maps.Add(new ScdFactorySyncMap()
                {
                    MapId = GetSceneNumIdFromLevelData(levelGroup),
                    
                    Wires =
                    {
                    },

                });
                LevelGradeInfo sceneGrade = ResourceManager.levelGradeTable[levelGroup].grades[0];
                chapter.Blackboard.Power.PowerGen += sceneGrade.bandwidth;

                var scene = new ScdFactorySyncScene()
                {
                    SceneId = GetSceneNumIdFromLevelData(levelGroup),
                    
                    Bandwidth = new()
                    {
                        Current = 0,
                        Max = sceneGrade.bandwidth,
                        TravelPoleMax = sceneGrade.travelPoleLimit,
                        
                        BattleCurrent = 0,
                        BattleMax = sceneGrade.battleBuildingLimit,
                    },
                    Settlements =
                    {
                        
                    },

                    Panels =
                    {
                        
                    }
                };
                chapter.Scenes.Add(scene);


            });

            SetData(ScMessageId.ScFactorySyncChapter, chapter);
        }

    }
}
