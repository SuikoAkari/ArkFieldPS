using BeyondTools.VFS.Crypto;
using EndFieldPS.Network;
using EndFieldPS.Packets.Sc;
using EndFieldPS.Protocol;
using EndFieldPS.Resource;
using Google.Protobuf;
using Google.Protobuf.WellKnownTypes;
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
        [Server.Handler(CsMessageId.CsSceneLoadFinish)]
        public static void HandleSceneFinish(EndminPlayer session, CsMessageId cmdId, Packet packet)
        {
            CsSceneLoadFinish req = packet.DecodeBody<CsSceneLoadFinish>();

            ScSelfSceneInfo sceneInfo = new()
            {
                SceneId = 0,
                SceneNumId = req.SceneNumId,
                SelfInfoReason = 0,
                
                TeamInfo = new()
                {
                    CurLeaderId= session.teams[session.teamIndex].leader,
                    TeamIndex= session.teamIndex,
                    TeamType=CharBagTeamType.Main
                    
                },
                SceneGrade=1,
                
                Detail =new()
                {
                   TeamIndex= session.teamIndex,
                   
                   CharList =
                   {

                   }
                }
            };

            session.teams[session.teamIndex].members.ForEach(m =>
            {
                sceneInfo.Detail.CharList.Add(session.chars.Find(c => c.guid == m).ToSceneProto());
            });
          //  session.Send(Packet.EncodePacket((int)ScMessageId.ScObjectEnterView, test));
            session.Send(Packet.EncodePacket((int)ScMessageId.ScSelfSceneInfo, sceneInfo));
           
        }
        [Server.Handler(CsMessageId.CsCreateRole)]
        public static void HandleCsCreateRole(EndminPlayer session, CsMessageId cmdId, Packet packet)
        {
            CsCreateRole req = packet.DecodeBody<CsCreateRole>();
            
            session.Send(new PacketScSyncBaseData(session));
            
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
            ScSceneCollectionSync collection = new ScSceneCollectionSync()
            {
                CollectionList =
                {
                }
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


            foreach (var item in ResourceManager.sceneAreaTable)
            {
                AreaUnlockInfo info = role.UnlockAreaInfo.ToList().Find(a => a.SceneId == item.Value.sceneId);
                if (info == null)
                {
                    info = new()
                    {
                        SceneId = item.Value.sceneId
                        
                    };

                    info.UnlockAreaId.Add(item.Value.areaId);
                    role.UnlockAreaInfo.Add(info);
                    //Server.Print(item.Value.sceneId);
                    role.SceneGradeInfo.Add(new SceneGradeInfo()
                    {
                        Grade = 1,
                        SceneNumId = ResourceManager.GetSceneNumIdFromLevelData(item.Value.sceneId)
                    });

                }
                else
                {
                    info.UnlockAreaId.Add(item.Value.areaId);
                }
            }
            session.Send(Packet.EncodePacket((int)ScMessageId.ScSyncAllRoleScene, role));
            foreach (var item in blocMissionTable)
            {
                missions.Missions.Add(item.Key,new Mission()
                {
                    MissionId=item.Value.missionId,
                    MissionState=(int)MissionState.Completed,
                });
            }
            ScSyncGameMode gameMode = new()
            {
                ModeId = "default",
            };
            ScSyncAllBloc allblocks = new()
            {
                Blocs =
                {
                    new BlocInfo()
                    {
                        Blocid="tundra_bloc01_endfield",
                        Exp=0,
                        Level=1
                    },
                    new BlocInfo()
                    {
                        Blocid="tundra_bloc02_ursus",
                        Exp=0,
                        Level=1
                    }
                }
            };

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
            session.Send(ScMessageId.ScGameMechanicsSync, mechanics);
            session.Send(ScMessageId.ScSyncAllBloc, allblocks);
            session.Send(ScMessageId.ScSyncGameMode, gameMode);
            session.Send(ScMessageId.ScSyncAllGameVar, GameVars);
            session.Send(ScMessageId.ScSyncAllUnlock, unlock);
            session.Send(ScMessageId.ScSyncAllMission, missions);
            session.Send(ScMessageId.ScAdventureSyncAll, adventure);

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

        static byte[] GenerateIV(byte[] nonce, int counter)
        {
            byte[] iv = new byte[8]; 
            Array.Copy(nonce, 0, iv, 0, 8); 
            return iv;
        }

        static ChaChaEngine ChaCha20(byte[] key, byte[] nonce, int counter)
        {
            if (nonce.Length != 12)
            {
                throw new ArgumentException("Nonce must be 12 bytes long");
            }

            var engine = new ChaChaEngine(20);
            var parameters = new ParametersWithIV(new KeyParameter(key), GenerateIV(nonce, counter));
            engine.Init(true, parameters);

            return engine;
        }
    }
}
