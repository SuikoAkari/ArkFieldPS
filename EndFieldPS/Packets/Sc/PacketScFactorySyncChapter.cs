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

        public PacketScFactorySyncChapter(EndminPlayer client, string chapterId) {

            
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
                        TravelPoleCurrent = 2,
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
                        new ScdFactorySyncScenePanel()
                        {
                            Index=0,
                            Level=1,
                            MainMesh =
                            {
                                new ScdRectInt()
                                {
                                    X=-512,
                                    Y=128,
                                },
                                new ScdRectInt()
                                {
                                    X=-256,
                                    Y=768,
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
                        TravelPoleCurrent = 0,
                        TravelPoleMax = 10,
                        BattleCurrent = 0,
                        BattleMax = 200,
                        SpCurrent = 100,
                        SpMax = 200,
                        
                    });
                }
                chapter.Scenes.Add(scene);
                chapter.Maps.Add(new ScdFactorySyncMap()
                {
                    MapId = GetSceneNumIdFromLevelData(levelGroup),
                    
                    Wires =
                    {
                        new ScdFactorySyncMapWire()
                        {
                            Index=0,
                            FromComId=0,
                            ToComId=0,
                            
                        }
                    },
                    

                });
            });
            
            SetData(ScMessageId.ScFactorySyncChapter, chapter);
        }

    }
}
