using BeyondTools.VFS.Crypto;
using EndFieldPS.Database;
using EndFieldPS.Game;
using EndFieldPS.Network;
using EndFieldPS.Packets.Sc;
using EndFieldPS.Protocol;
using EndFieldPS.Resource;
using System.Security.Cryptography;
using static EndFieldPS.Resource.ResourceManager;

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
            if(Server.clients.Count > Server.config.serverOptions.maxPlayers)
            {
                session.Send(ScMessageId.ScNtfErrorCode, new ScNtfErrorCode()
                {
                    Details = "Server Full",
                    ErrorCode = (int)CODE.ErrCommonServerOverload,
                });
                session.Disconnect();
                return;
            }
            Account account = DatabaseManager.db.GetAccountByTokenGrant(req.Token);
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
           // rsp.ServerPublicKey = ByteString.CopyFrom(encryptedEncKey);
       
            CSChaCha20 cipher = new CSChaCha20(encKey, serverEncrypNonce, 1);
            if (req.ClientVersion == GameConstants.GAME_VERSION)
            {
                if (account == null)
                {
                    session.Send(ScMessageId.ScNtfErrorCode, new ScNtfErrorCode()
                    {
                        Details = "Account error",
                        ErrorCode = (int)CODE.ErrLoginProcessLogin,
                    });
                    session.Disconnect();
                    return;
                }
                session.Load(account.id);
                
                rsp.Uid = ""+session.accountId;
                session.Send(ScMessageId.ScLogin, rsp);
                
            }
            else
            {
                session.Send(ScMessageId.ScNtfErrorCode, new ScNtfErrorCode()
                {
                    Details="Unsupported client version",
                    ErrorCode= (int)CODE.ErrCommonClientVersionNotEqual
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
            session.Send(new PacketScItemBagScopeSync(session,ItemValuableDepotType.Weapon));
            session.Send(new PacketScItemBagScopeSync(session, ItemValuableDepotType.WeaponGem));
            session.Send(new PacketScItemBagScopeSync(session, ItemValuableDepotType.Equip));
            session.Send(new PacketScItemBagScopeSync(session, ItemValuableDepotType.CommercialItem));
            session.Send(new PacketScItemBagScopeSync(session, ItemValuableDepotType.Factory));
            session.Send(new PacketScItemBagScopeSync(session, ItemValuableDepotType.SpecialItem));
            session.Send(new PacketScSyncAllMail(session));
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
                Missions =
                {
                    {"e5m2_q#5d3", 
                        new Mission()
                        {
                            MissionId="e5m2_q#5d3",
                            MissionState=(int)MissionState.Completed,
                            
                        } 
                    },
                    {"e5m2_q#14",
                        new Mission()
                        {
                            MissionId="e5m2_q#14",
                            MissionState=(int)MissionState.Completed,

                        }
                    }
                }
            };
           
            ScAdventureSyncAll adventure = new()
            {
                Exp = session.xp,
                Level = (int)session.level,
                
            };

            ScSyncGameMode gameMode = new()
            {
                ModeId = "Default",

            };
            session.Send(new PacketScGachaSync(session));
            ScSettlementSyncAll settlements = new ScSettlementSyncAll()
            {
                LastTickTime = DateTime.UtcNow.ToUnixTimestampMilliseconds(),
                
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
                    UnlockTs = DateTime.UtcNow.ToUnixTimestampMilliseconds(),
                    AutoSubmit = false,
                    LastManualSubmitTime = DateTime.UtcNow.ToUnixTimestampMilliseconds(),

                    OfficerCharTemplateId = characterTable.Values.ToList()[stid].charId,

                    
                });
                stid++;
            }
            
            session.Send(new PacketScSyncAllRoleScene(session));
            session.Send(ScMessageId.ScSettlementSyncAll, settlements);
            session.Send(new PacketScGameMechanicsSync(session));
            session.Send(new PacketScSyncAllBloc(session));
            session.Send(new PacketScSyncWallet(session));
            session.Send(ScMessageId.ScSyncGameMode, gameMode);
            session.Send(new PacketScSyncAllGameVar(session));
            session.Send(new PacketScSyncAllUnlock(session));
            session.Send(ScMessageId.ScSyncAllMission, missions);
            session.Send(new PacketScSyncAllBitset(session));
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
          //session.Send(new PacketScFactorySync(session));
            session.Send(new PacketScFactorySyncScope(session));
            session.Send(new PacketScFactorySyncChapter(session, "domain_1"));
            session.Send(new PacketScFactorySyncChapter(session, "domain_2"));

            session.Send(new PacketScSyncCharBagInfo(session));
            
            session.Send(new PacketScSpaceshipSync(session));
            session.Send(ScMessageId.ScSyncFullDataEnd, new ScSyncFullDataEnd());
            session.EnterScene();
            session.Initialized = true;
            session.Update();
            ScSyncFullDungeonStatus dungeonStatus = new()
            {
                CurStamina = session.curStamina,
                MaxStamina = session.maxStamina,
                NextRecoverTime = session.nextRecoverTime / 1000,
            };
            session.Send(ScMessageId.ScSyncFullDungeonStatus, dungeonStatus);
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
