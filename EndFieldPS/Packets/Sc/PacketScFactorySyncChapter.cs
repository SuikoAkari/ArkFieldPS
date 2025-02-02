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
                
                Tms=DateTime.UtcNow.ToUnixTimestampMilliseconds(),
                
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
                    
                },
                
                
            };
            foreach (var item in strIdNumTable.chapter_map_id.dic)
            {
                chapter.Maps.Add(new ScdFactorySyncMap()
                {
                    MapId = item.Value,
                    
                    Wires =
                    {

                    },

                });
            }
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
                        BattleMax = 6,
                        SpCurrent = 100,
                        SpMax = 200
                    },
                    Settlements =
                    {
                        
                    },

                    Panels =
                    {
                        new ScdFactorySyncScenePanel()
                        {
                            Level=2,
                            MainMesh =
                            {
                                new ScdRectInt()
                                {
                                    X=-728,
                                    Y=104,
                                    Z=186,
                                    L=32,
                                    W=32,
                                    H=50
                                }
                            }
                        }
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


            });

            SetData(ScMessageId.ScFactorySyncChapter, chapter);
        }

    }
}
