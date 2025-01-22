using BeyondTools.VFS.Crypto;
using EndFieldPS.Game;
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
        public static void HandleCsCreateRole(Player session, CsMessageId cmdId, Packet packet)
        {
            CsCreateRole req = packet.DecodeBody<CsCreateRole>();

            
        }
        [Server.Handler(CsMessageId.CsLogin)]
        public static void Handle(Player session, CsMessageId cmdId, Packet packet)
        {
            CsLogin req = packet.DecodeBody<CsLogin>();
            session.Initialize();
            ScLogin rsp = new()
            {
                IsEnc = false,
                Uid = req.Uid,
                IsFirstLogin = false,
                IsReconnect=false,
                LastRecvUpSeqid = packet.csHead.UpSeqid,
                
            };
            byte[] encKey = GenerateRandomBytes(32);
            string serverPublicKeyPem = req.ClientPublicKey.ToStringUtf8();
            byte[] serverPublicKey = ConvertPemToBytes(serverPublicKeyPem);
            byte[] encryptedEncKey = EncryptWithRsa(encKey, serverPublicKey);
            byte[] serverEncrypNonce = GenerateRandomBytes(12);
           // rsp.ServerEncrypNonce = ByteString.CopyFrom(serverEncrypNonce);
          //  rsp.ServerPublicKey = ByteString.CopyFrom(encryptedEncKey);
       
            CSChaCha20 cipher = new CSChaCha20(encKey, serverEncrypNonce, 1);
            if (req.ClientVersion == GameConstants.GAME_VERSION)
            {
                session.Send(ScMessageId.ScLogin, rsp);
            }
            else
            {
                session.Send(ScMessageId.ScNtfErrorCode, new ScNtfErrorCode()
                {
                    Details="Unsupported client version",
                    ErrorCode=-1
                });
                session.Disconnect();
                return;
            }
            

            session.Send(new PacketScSyncBaseData(session));
            ScItemBagCommonSync common = new()
            {
                LostAndFound =
                {

                },

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
                // unlock.UnlockSystems.Add((int)unlockType);
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
            unlock.UnlockSystems.Add((int)UnlockSystemType.Map);
            unlock.UnlockSystems.Add((int)UnlockSystemType.FacZone);
            unlock.UnlockSystems.Add((int)UnlockSystemType.FacHub);
            unlock.UnlockSystems.Add((int)UnlockSystemType.DungeonFactory);
            unlock.UnlockSystems.Add((int)UnlockSystemType.FacSystem);
            unlock.UnlockSystems.Add((int)UnlockSystemType.FacTransferPort);
            unlock.UnlockSystems.Add((int)UnlockSystemType.FacMode);
            unlock.UnlockSystems.Add((int)UnlockSystemType.FacOverview);
            unlock.UnlockSystems.Add((int)UnlockSystemType.SpaceshipSystem);
            unlock.UnlockSystems.Add((int)UnlockSystemType.SpaceshipControlCenter);
            unlock.UnlockSystems.Add((int)UnlockSystemType.FacBUS);
            unlock.UnlockSystems.Add((int)UnlockSystemType.PRTS);

            ScSceneCollectionSync collection = new ScSceneCollectionSync()

            {
                CollectionList =
                {

                },

            };

            foreach (var item in ResourceManager.levelDatas)
            {
                foreach (var item1 in collectionTable)
                {
                    collection.CollectionList.Add(new SceneCollection()
                    {
                        Count = 1,
                        PrefabId = item1.Value.prefabId,
                        SceneName = item.id
                    });
                }

            }

            session.Send(ScMessageId.ScSceneCollectionSync, collection);
            ScSyncAllMission missions = new()
            {

            };
            ScSyncAllGameVar GameVars = new()
            {

            };
            for (int cVar = 1; cVar <= 38; cVar++)
            {
                GameVars.ClientVars.Add(cVar, 1);
            }
            for (int sVar = 1; sVar <= 50; sVar++)
            {
                GameVars.ServerVars.Add(sVar, 1);
            }

            ScAdventureSyncAll adventure = new()
            {
                Exp = 0,
                Level = 20,
            };

            ScSyncGameMode gameMode = new()
            {
                ModeId = "Default",

            };
            ScGachaSync gacha = new ScGachaSync()
            {
                CharGachaPool = new()
                {
                    GachaPoolInfos =
                    {

                    }
                }
            };
            foreach (var item in gachaCharPoolTable)
            {
                gacha.CharGachaPool.GachaPoolInfos.Add(new ScdGachaPoolInfo()
                {
                    GachaPoolId = item.Value.id,

                });
            }
            session.Send(ScMessageId.ScGachaSync, gacha);
            ScSettlementSyncAll settlements = new ScSettlementSyncAll()
            {
                LastTickTime = DateTime.UtcNow.Ticks,

            };
            int stid = 0;
            foreach (var item in settlementBasicDataTable)
            {
                settlements.Settlements.Add(new Settlement()
                {
                    Level = 1,
                    SettlementId = item.Value.settlementId,
                    RequireId = "item_plant_grass_powder_2",
                    Exp = 1,
                    Reports =
                    {

                    },
                    UnlockTs = DateTime.UtcNow.Ticks,
                    AutoSubmit = false,
                    LastManualSubmitTime = DateTime.UtcNow.Ticks,

                    OfficerCharTemplateId = characterTable.Values.ToList()[stid].charId,


                });
                stid++;
            }

            ScSyncAllBitset bitset = new()
            {
                Bitset =
                {
                    new BitsetData()
                    {
                        Type=44,
                        Value =
                        {

                        }
                    }
                }
            };

            session.Send(new PacketScSyncAllRoleScene(session));
            session.Send(ScMessageId.ScSettlementSyncAll, settlements);
            session.Send(new PacketScGameMechanicsSync(session));
            session.Send(new PacketScSyncAllBloc(session));
            session.Send(ScMessageId.ScSyncGameMode, gameMode);
            session.Send(ScMessageId.ScSyncAllGameVar, GameVars);
            session.Send(ScMessageId.ScSyncAllUnlock, unlock);
            session.Send(ScMessageId.ScSyncAllMission, missions);
            ScSceneMapMarkSync mapMarks = new()
            {
                SceneStaticMapMarkList =
                {

                },
                TrackPoint = new()
                {

                }
            };

            for (int i = 0; i <= 28; i++)
            {
                mapMarks.SceneStaticMapMarkList.Add(new SceneStaticMapMark()
                {
                    Index = i
                });
            }
            session.Send(ScMessageId.ScSceneMapMarkSync, mapMarks);
            session.Send(ScMessageId.ScAdventureSyncAll, adventure);
            ScFactorySync fsync = new()
            {
                FormulaMan = new()
                {

                },
                Stt = new()
                {
                    Layers =
                    {
                    },

                }
            };
            session.Send(ScMessageId.ScFactorySync, fsync);
            session.Send(ScMessageId.ScFactorySyncScope, new ScFactorySyncScope()
            {
                BookMark = new()
                {

                },
                ScopeName = 1,
                Quickbars =
                {
                    new ScdFactorySyncQuickbar()
                    {
                        Type=0,
                        List =
                        {
                            "","","","","","","",""
                        }
                    },
                    new ScdFactorySyncQuickbar()
                    {
                        Type=1,
                        List =
                        {
                            "","","","","","","",""
                        }
                    }
                },

                TransportRoute = new()
                {
                    UpdateTs = DateTime.UtcNow.Ticks + 100000000,
                    Routes =
                    {

                    },

                },
                CurrentChapterId = "domain_1",
            });
            session.Send(new PacketScFactorySyncChapter(session, "domain_1"));
            session.Send(new PacketScFactorySyncChapter(session, "domain_2"));



            ScSyncFullDungeonStatus dst = new()
            {
                CurStamina = 200,
                MaxStamina = 200,

            };
            session.Send(ScMessageId.ScSyncFullDungeonStatus, dst);
            session.Send(ScMessageId.ScSpaceshipSync, new ScSpaceshipSync()
            {
                Rooms =
                {
                    new ScdSpaceshipRoom()
                    {
                        Id="control_center",
                        Type=0,
                        ControlCenter = new()
                        {

                        },
                        Level=1,

                    }
                },

            });

            session.Send(ScMessageId.ScSyncFullDataEnd, new ScSyncFullDataEnd());
            session.EnterScene(98); //101

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
