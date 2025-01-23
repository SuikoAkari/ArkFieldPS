using EndFieldPS.Game.Inventory;
using EndFieldPS.Network;
using EndFieldPS.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static EndFieldPS.Resource.ResourceManager;

namespace EndFieldPS.Packets.Sc
{
    public class PacketScFactorySyncChapter : Packet
    {

        public PacketScFactorySyncChapter(Player client, string chapterId) {

            
            ScFactorySyncChapter chapter = new()
            {
                ChapterId = chapterId,
                
                Tms=DateTime.UtcNow.Ticks,
                
                Nodes =
                {
                    
                },
                Blackboard = new()
                {
                    Power = new()
                    {
                        PowerGen = 100,
                        PowerSaveMax=200,
                        PowerSaveCurrent=100,
                    },
                    InventoryNodeId=0
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
                    
                }
                
            };
            
            domainDataTable[chapterId].levelGroup.ForEach(levelGroup =>
            {
                var scene = new ScdFactorySyncScene()
                {
                    SceneId = GetSceneNumIdFromLevelData(levelGroup),
                    
                    Bandwidth = new()
                    {
                        Current = 100,
                        Max = 200,
                        TravelPoleCurrent = 1,
                        TravelPoleMax = 10,
                        
                        BattleCurrent = 0,
                        BattleMax = 200,
                        SpCurrent = 100,
                        SpMax = 200
                    },
                    Settlements =
                    {
                        
                    },

                    Panels =
                    {

                    }
                };
                foreach (var item in domainDataTable[chapterId].settlementGroup)
                {
                    scene.Settlements.Add(item, new ScdFactorySyncSceneBandwidth()
                    {
                        Current = 100,
                        Max = 200,
                        TravelPoleCurrent = 1,
                        TravelPoleMax = 10,
                        BattleCurrent = 0,
                        BattleMax = 200,
                        SpCurrent = 100,
                        SpMax = 200,
                        
                    });
                    
                }
                chapter.Scenes.Add(scene);
                if(chapter.Maps.ToList().Find(a=>a.MapId== strIdNumTable.chapter_map_id.dic[GetLevelData(GetSceneNumIdFromLevelData(levelGroup)).mapIdStr])==null)
                chapter.Maps.Add(new ScdFactorySyncMap()
                {
                    MapId = strIdNumTable.chapter_map_id.dic[GetLevelData(GetSceneNumIdFromLevelData(levelGroup)).mapIdStr],
                    
                    Wires =
                    {

                    },
                    
                });
            });

            SetData(ScMessageId.ScFactorySyncChapter, chapter);
        }

    }
}
