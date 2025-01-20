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
        public static void HandleSceneFinish(Client session, CsMessageId cmdId, Packet packet)
        {
            CsSceneLoadFinish req = packet.DecodeBody<CsSceneLoadFinish>();

            ScSelfSceneInfo sceneInfo = new()
            {
                SceneId = 0,
                SceneNumId = req.SceneNumId,
                SelfInfoReason = 0,
                
                TeamInfo = new()
                {
                    CurLeaderId=2,
                    TeamIndex=0,
                    TeamType=CharBagTeamType.Main
                    
                },
                SceneGrade=1,
                
                Detail =new()
                {
                   TeamIndex=0,
                   
                   CharList =
                    {
                        new SceneCharacter()
                        {
                            Level=1,
                            
                            BattleInfo =new()
                            {
                                MsgGeneration=1,
                                SkillList =
                                {
                                    new ServerSkill()
                                    {
                                        Blackboard = new()
                                        {
                                            
                                        },
                                        InstId=101,
                                        Level=1,
                                        Source=BattleSkillSource.Default,
                                        PotentialLv=1,
                                        SkillId="chr_0015_lifeng_NormalSkill",
                                    },
                                    new ServerSkill()
                                    {
                                        Blackboard = new()
                                        {

                                        },
                                        InstId=102,
                                        Level=1,
                                        Source=BattleSkillSource.Default,
                                        PotentialLv=1,
                                        SkillId="chr_0015_lifeng_ComboSkill",
                                    },
                                    new ServerSkill()
                                    {
                                        Blackboard = new()
                                        {

                                        },
                                        InstId=103,
                                        Level=1,
                                        Source=BattleSkillSource.Default,
                                        PotentialLv=1,
                                        SkillId="chr_0015_lifeng_UltimateSkill",
                                    },
                                    new ServerSkill()
                                    {
                                        Blackboard = new()
                                        {

                                        },
                                        InstId=104,
                                        Level=1,
                                        Source=BattleSkillSource.Default,
                                        PotentialLv=1,
                                        SkillId="chr_0015_lifeng_NormalAttack",
                                    }
                                }
                            },
                            /*
                             * chr_0003_endminf_NormalAttack
                             * NormalSkill="chr_0015_lifeng_NormalSkill",
                            ComboSkill="chr_0015_lifeng_ComboSkill",
                            UltimateSkill="chr_0015_lifeng_UltimateSkill",*/
                            Name="Lifeng",
                            CommonInfo = new()
                            {
                                Hp=ResourceManager.characterTable["chr_0015_lifeng"].attributes[1].Attribute.attrs.Find(A=>A.attrType==(int)AttributeType.MaxHp).attrValue,
                                Id=2,
                                Position = new Vector()
                                {
                                    X = 292.06f,
                                    Y = 86.515f,
                                    Z = -624.24f
                                },
                                Rotation=new(),
                                SceneNumId=req.SceneNumId,
                                Templateid="chr_0015_lifeng",
                                Type=(int)0,
                                
                            },
                            Attrs =
                            {

                            }
                        },
                        new SceneCharacter()
                        {
                            Level=1,

                            BattleInfo =new()
                            {
                                MsgGeneration=1,
                                SkillList =
                                {
                                    new ServerSkill()
                                    {
                                        Blackboard = new()
                                        {

                                        },
                                        InstId=101,
                                        Level=1,
                                        Source=BattleSkillSource.Default,
                                        PotentialLv=1,
                                        SkillId="chr_0003_endminf_NormalSkill",
                                    },
                                    new ServerSkill()
                                    {
                                        Blackboard = new()
                                        {

                                        },
                                        InstId=102,
                                        Level=1,
                                        Source=BattleSkillSource.Default,
                                        PotentialLv=1,
                                        SkillId="chr_0003_endminf_ComboSkill",
                                    },
                                    new ServerSkill()
                                    {
                                        Blackboard = new()
                                        {

                                        },
                                        InstId=103,
                                        Level=1,
                                        Source=BattleSkillSource.Default,
                                        PotentialLv=1,
                                        SkillId="chr_0003_endminf_UltimateSkill",
                                    },
                                    new ServerSkill()
                                    {
                                        Blackboard = new()
                                        {

                                        },
                                        InstId=104,
                                        Level=1,
                                        Source=BattleSkillSource.Default,
                                        PotentialLv=1,
                                        SkillId="chr_0003_endminf_NormalAttack",
                                    }
                                }
                            },
                            /*
                             * chr_0003_endminf_NormalAttack
                             * NormalSkill="chr_0015_lifeng_NormalSkill",
                            ComboSkill="chr_0015_lifeng_ComboSkill",
                            UltimateSkill="chr_0015_lifeng_UltimateSkill",*/
                            Name="Endministrator",
                            CommonInfo = new()
                            {
                                Hp=ResourceManager.characterTable["chr_0003_endminf"].attributes[1].Attribute.attrs.Find(A=>A.attrType==(int)AttributeType.MaxHp).attrValue,
                                Id=1,
                                Position = new Vector()
                                {
                                    X = 292.06f,
                                    Y = 86.515f,
                                    Z = -624.24f
                                },
                                Rotation=new(),
                                SceneNumId=req.SceneNumId,
                                Templateid="chr_0003_endminf",
                                Type=(int)0,

                            },
                            Attrs =
                            {

                            }
                        }
                    },
                }
            };
            
            //Attrib lev 20?
            ResourceManager.characterTable["chr_0015_lifeng"].attributes[1].Attribute.attrs.ForEach(attr =>
            {
                sceneInfo.Detail.CharList[0].Attrs.Add(new AttrInfo()
                {
                    AttrType=attr.attrType,
                    BasicValue = 0,
                    Value=attr.attrValue
                    
                });
                
            });

            ResourceManager.characterTable["chr_0003_endminf"].attributes[1].Attribute.attrs.ForEach(attr =>
            {
                sceneInfo.Detail.CharList[1].Attrs.Add(new AttrInfo()
                {
                    AttrType = attr.attrType,
                    BasicValue = attr.attrValue,
                    Value = attr.attrValue
                });

            });
          //  session.Send(Packet.EncodePacket((int)ScMessageId.ScObjectEnterView, test));
            session.Send(Packet.EncodePacket((int)ScMessageId.ScSelfSceneInfo, sceneInfo));
           
        }
        [Server.Handler(CsMessageId.CsCreateRole)]
        public static void HandleCsCreateRole(Client session, CsMessageId cmdId, Packet packet)
        {
            CsCreateRole req = packet.DecodeBody<CsCreateRole>();
            session.Send(new PacketScSyncBaseData(session));
            session.Send(new PacketScSyncCharBagInfo(session));
            session.Send(Packet.EncodePacket((int)ScMessageId.ScItemBagScopeSync, new ScItemBagScopeSync()
            {
                Bag =new()
                {
                    GridLimit=30,
                    
                },
                ScopeName = 1,
                Depot = { {(int)ItemValuableDepotType.Weapon,new ScdItemDepot()
                {
                    InstList =
                    {
                        new ScdItemGrid()
                        {
                            Count = 1,
                            Id="wpn_lance_0003",

                            Inst = new()
                            {
                                InstId=4611757057225326607,
                                Weapon = new()
                                {
                                    InstId=4611757057225326607,
                                    EquipCharId=2,
                                    WeaponLv=1,
                                    TemplateId=ResourceManager.GetItemTemplateId("wpn_lance_0003"),
                                    Exp=0,

                                },

                            }
                        },
                        new ScdItemGrid()
                        {
                            Count = 1,

                            Id="wpn_sword_0009",
                            Inst = new()
                            {
                                InstId=4611757057225326608,
                                Weapon = new()
                                {
                                    InstId=4611757057225326608,
                                    EquipCharId=1,
                                    WeaponLv=1,
                                    TemplateId=ResourceManager.GetItemTemplateId("wpn_sword_0009"),
                                    Exp=0,

                                },

                            }
                        }


                    }
                } } }

            }));

            ScSyncAllUnlock unlock = new()
            {

            };
            foreach (var item in gameSystemConfigTable)
            {
                unlock.UnlockSystems.Add(item.Value.unlockSystemType);
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
            
            session.Send(Packet.EncodePacket((int)ScMessageId.ScSceneCollectionSync, collection));
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
            for (int i=1; i < 50; i++)
            {
                GameVars.ClientVars.Add(i,0);
            }
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
            session.Send(Packet.EncodePacket((int)ScMessageId.ScSyncGameMode, gameMode));
            session.Send(Packet.EncodePacket((int)ScMessageId.ScSyncAllGameVar, GameVars));
            session.Send(Packet.EncodePacket((int)ScMessageId.ScSyncAllUnlock, unlock));
            session.Send(Packet.EncodePacket((int)ScMessageId.ScSyncAllMission, missions));
            session.Send(Packet.EncodePacket((int)ScMessageId.ScAdventureSyncAll, adventure));
            ScMessageId.
            session.EnterScene(101);
        }
        [Server.Handler(CsMessageId.CsLogin)]
        public static void Handle(Client session, CsMessageId cmdId, Packet packet)
        {
            CsLogin req = packet.DecodeBody<CsLogin>();
            
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
