using BeyondTools.VFS.Crypto;
using EndFieldPS.Network;
using EndFieldPS.Packets.Sc;
using EndFieldPS.Protocol;
using EndFieldPS.Resource;
using Google.Protobuf;
using Google.Protobuf.WellKnownTypes;
using Org.BouncyCastle.Asn1.Utilities;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;

using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static EndFieldPS.Resource.ResourceManager;
using static System.Net.Mime.MediaTypeNames;

namespace EndFieldPS.Packets.Cs
{
    public class HandleCsLogin
    {
        [Server.Handler(CsMessageId.CsCreateRole)]
        public static void HandleCsCreateRole(EndminPlayer session, CsMessageId cmdId, Packet packet)
        {
            CsCreateRole req = packet.DecodeBody<CsCreateRole>();
            
            session.Send(new PacketScSyncBaseData(session));
            ScItemBagCommonSync common = new()
            {
                LostAndFound =
                {

                }
            };
            
            session.Send(ScMessageId.ScItemBagCommonSync, common);
            session.Send(new PacketScItemBagScopeSync(session));
            session.Send(new PacketScSyncCharBagInfo(session));
            ScSyncAllUnlock unlock = new()
            {

            };
            foreach (var item in gameSystemConfigTable)
            {
                unlock.UnlockSystems.Add(item.Value.unlockSystemType);
            }
            foreach (var item in systemJumpTable)
            {
                unlock.UnlockSystems.Add(item.Value.bindSystem);
            }
            foreach (UnlockSystemType unlockType in System.Enum.GetValues(typeof(UnlockSystemType)))
            {
              
            }
            unlock.UnlockSystems.Add((int)UnlockSystemType.Watch);
            unlock.UnlockSystems.Add((int)UnlockSystemType.Weapon);
            unlock.UnlockSystems.Add((int)UnlockSystemType.Equip);
            unlock.UnlockSystems.Add((int)UnlockSystemType.EquipEnhance);
            unlock.UnlockSystems.Add((int)UnlockSystemType.NormalAttack);
            unlock.UnlockSystems.Add((int)UnlockSystemType.NormalSkill);
            unlock.UnlockSystems.Add((int)UnlockSystemType.UltimateSkill);
            unlock.UnlockSystems.Add((int)UnlockSystemType.TeamSkill);
            unlock.UnlockSystems.Add((int)UnlockSystemType.ComboSkill);
            unlock.UnlockSystems.Add((int)UnlockSystemType.TeamSwitch);
            unlock.UnlockSystems.Add((int)UnlockSystemType.Dash);
            unlock.UnlockSystems.Add((int)UnlockSystemType.Jump);
            unlock.UnlockSystems.Add((int)UnlockSystemType.Friend);
            unlock.UnlockSystems.Add((int)UnlockSystemType.SNS);
            unlock.UnlockSystems.Add((int)UnlockSystemType.Settlement);

            ScSceneCollectionSync collection = new ScSceneCollectionSync()
            {
                CollectionList =
                {
                },
                
            };
            
            foreach (var item in ResourceManager.levelDatas)
            {
                collection.CollectionList.Add(new SceneCollection()
                {
                    Count = 1,
                    PrefabId = item.id,
                    SceneName = item.id
                });
            }
            
            session.Send(ScMessageId.ScSceneCollectionSync, collection);
            ScSyncAllMission missions = new()
            {

            };
            ScSyncAllGameVar GameVars = new()
            {

            };
            for(int cVar=1; cVar<=38; cVar++)
            {
                GameVars.ClientVars.Add(cVar, 1);
            }
            for (int sVar= 1; sVar <= 50; sVar++)
            {
                GameVars.ServerVars.Add(sVar, 1);
            }
            
            ScAdventureSyncAll adventure = new()
            {
                Exp = 0,
                Level = 20,
            };
           
            ScSyncAllRoleScene role = new ScSyncAllRoleScene()
            {
                SceneGradeInfo =
                {

                },
                UnlockAreaInfo =
                {

                },
                SubmitEtherCount = 0,
                SubmitEtherLevel = 1,
                
            };
            
            List<AreaUnlockInfo> areas = new();
            foreach (var item in ResourceManager.sceneAreaTable)
            {
                AreaUnlockInfo info = areas.Find(a => a.SceneId == item.Value.sceneId);
                if (info == null)
                {
                    info = new()
                    {
                        SceneId = item.Value.sceneId
                        
                    };

                    info.UnlockAreaId.Add(item.Value.areaId);
                    areas.Add(info);
                    //Server.Print(item.Value.sceneId);
                    role.SceneGradeInfo.Add(new SceneGradeInfo()
                    {
                        Grade = 1,
                        LastDownTs=DateTime.UtcNow.Ticks,
                        SceneNumId = ResourceManager.GetLevelData(item.Value.sceneId).idNum,
                    });

                }
                else
                {
                    info.UnlockAreaId.Add(item.Value.areaId);
                }
            }
            role.UnlockAreaInfo.AddRange(areas);
            session.Send(Packet.EncodePacket((int)ScMessageId.ScSyncAllRoleScene, role));
            foreach (var item in blocMissionTable)
            {
                missions.Missions.Add(item.Key,new Mission()
                {
                    MissionId=item.Value.missionId,
                    MissionState=(int)MissionState.Completed,
                });
            }
            foreach (var item in missionAreaTable.m_areas)
            {
                foreach (var m in item.Value)
                {
                    if (!missions.Missions.ContainsKey(m.Key))
                    {
                        missions.Missions.Add(m.Key, new Mission()
                        {
                            MissionId = m.Key,
                            MissionState = (int)MissionState.Completed,

                        });
                    }
                    
                }
                
            }
            ScSyncGameMode gameMode = new()
            {
                ModeId = "default",
            };
            ScSyncAllBloc allblocks = new()
            {
                Blocs =
                {

                }
            };
            foreach (var region in blocDataTable)
            {
                allblocks.Blocs.Add(new BlocInfo()
                {
                    Exp=0,
                    Level=1,
                    Blocid=region.Value.blocId
                });
            }
            ScGameMechanicsSync mechanics = new()
            {

            };
            foreach (var item in gameMechanicTable)
            {
                mechanics.GameRecords.Add(new ScdGameMechanicsRecord()
                {
                    GameId=item.Value.gameMechanicsId,
                    IsPass=true,
                    
                });
            }
            session.Send(ScMessageId.ScGachaSync, new ScGachaSync()
            {
                CharGachaPool = new()
                {
                    GachaPoolInfos =
                    {
                        new ScdGachaPoolInfo()
                        {
                            GachaPoolId="special_2025_1_1",
                            
                        }
                    }
                }
            });
            ScSettlementSyncAll settlements = new ScSettlementSyncAll()
            {
                LastTickTime= DateTime.UtcNow.Ticks,
            };
            foreach (var item in settlementBasicDataTable)
            {
                settlements.Settlements.Add(new Settlement()
                {
                    Level=1,
                    SettlementId=item.Value.settlementId,
                    UnlockTs=DateTime.UtcNow.Ticks,
                    AutoSubmit=false,
                    OfficerCharTemplateId = characterTable.Values.ToList()[0].charId,
                    
                });
            }
            ScSyncAllStat stat = new()
            {
                StatsInfo =
                {
                    new StatInfo()
                    {
                        
                    }
                }
            };
            session.Send(ScMessageId.ScSpaceshipSync, new ScSpaceshipSync()
            {
                
            });
            
            session.Send(ScMessageId.ScSettlementSyncAll, settlements);
            session.Send(ScMessageId.ScGameMechanicsSync, mechanics);
            session.Send(ScMessageId.ScSyncAllBloc, allblocks);
            session.Send(ScMessageId.ScSyncGameMode, gameMode);
            session.Send(ScMessageId.ScSyncAllGameVar, GameVars);
            session.Send(ScMessageId.ScSyncAllUnlock, unlock);
            session.Send(ScMessageId.ScSyncAllMission, missions);
            
            session.Send(ScMessageId.ScAdventureSyncAll, adventure);
            session.Send(ScMessageId.ScSyncFullDataEnd, new ScSyncFullDataEnd());
            session.Send(ScMessageId.ScFactoryHsSync, new ScFactoryHsSync()
            {
                Blackboard = new()
                {
                    Power = new()
                    {
                        
                    },
                    
                },
                CcList =
                {
        
                },
                

            });
            session.Send(ScMessageId.ScFactorySyncScope, new ScFactorySyncScope()
            {
                BookMark = new(),
                ScopeName=1,
                
                TransportRoute = new()
                {
                    
                },
                
            });
            session.Send(ScMessageId.ScFactorySyncStatistic, new ScdFactorySyncStatistic()
            {
                LastDay = new()
                {
                    
                },
                Other = new()
                {

                },
                
            });
            session.EnterScene(101);
        }
        [Server.Handler(CsMessageId.CsLogin)]
        public static void Handle(EndminPlayer session, CsMessageId cmdId, Packet packet)
        {
            CsLogin req = packet.DecodeBody<CsLogin>();
            session.Initialize();
            ScLogin rsp = new()
            {
                IsEnc = false,
                Uid = req.Uid,
                IsFirstLogin = true,
                IsReconnect=false,
               // LastRecvUpSeqid =
               // packet.csHead.UpSeqid,
            };
            byte[] encKey = GenerateRandomBytes(32);

            string serverPublicKeyPem = req.ClientPublicKey.ToStringUtf8();
            byte[] serverPublicKey = ConvertPemToBytes(serverPublicKeyPem);

   
            byte[] encryptedEncKey = EncryptWithRsa(encKey, serverPublicKey);


            byte[] serverEncrypNonce = GenerateRandomBytes(12);
           // rsp.ServerEncrypNonce = ByteString.CopyFrom(serverEncrypNonce);
          //  rsp.ServerPublicKey = ByteString.CopyFrom(encryptedEncKey);
       
            CSChaCha20 cipher = new CSChaCha20(encKey, serverEncrypNonce, 1);
            session.Send(Packet.EncodePacket((int)ScMessageId.ScLogin, rsp));
           
            
        }
        static byte[] GenerateRandomBytes(int length)
        {
            using var rng = new RNGCryptoServiceProvider();
            byte[] bytes = new byte[length];
            rng.GetBytes(bytes);
            return bytes;
        }
        static byte[] ConvertPemToBytes(string pem)
        {
            string base64Key = pem
                .Replace("-----BEGIN PUBLIC KEY-----", "")
                .Replace("-----END PUBLIC KEY-----", "")
                .Replace("\n", "")
                .Replace("\r", "");
            return Convert.FromBase64String(base64Key);
        }

        // Crittografare con RSA (PKCS#1)
        static byte[] EncryptWithRsa(byte[] data, byte[] publicKeyBytes)
        {
            var rsa = RSA.Create();
            rsa.ImportSubjectPublicKeyInfo(publicKeyBytes, out _);
            return rsa.Encrypt(data, RSAEncryptionPadding.Pkcs1);
        }

    }
}
