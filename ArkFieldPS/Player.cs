
using ArkFieldPS.Network;
using ArkFieldPS.Protocol;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Pastel;
using SQLite;
using SQLiteNetExtensions.Attributes;
using System.Drawing;
using System.Linq;
using System.Numerics;
using MongoDB.Bson.Serialization.Attributes;
using System.Reflection;
using System.Net.Sockets;
using static ArkFieldPS.Http.Dispatch;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Collections;
using System;
using ArkFieldPS.Packets.Sc;
using ArkFieldPS.Game.Character;
using ArkFieldPS.Resource;
using ArkFieldPS.Game.Inventory;
using static ArkFieldPS.Resource.ResourceManager;
using ArkFieldPS.Database;
using ArkFieldPS.Game;
using ArkFieldPS.Game.Gacha;
using ArkFieldPS.Game.Spaceship;
using ArkFieldPS.Game.Dungeons;
using ArkFieldPS.Game.Factory;


namespace ArkFieldPS
{
    public class GuidRandomizer
    {
        public ulong v = 1;
        public List<ulong> usedGuids = new();
        public Random random = new();
        public Player player;
        public GuidRandomizer(Player p)
        {
            this.player = p;
        }
        public ulong Next()
        {
            if(v+1>= IdConst.LOGIC_ID_SEGMENT)
            {
                v = IdConst.MAX_LOGIC_ID_BOUND+1;
            }
            v++;
            return (ulong)v;
        }
        
        public ulong NextRand()
        {
            var maxGuid = IdConst.MAX_LOGIC_ID_BOUND+1;
            
            ulong val = (ulong)random.NextInt64((long)maxGuid,(long)IdConst.MAX_RUNTIME_CLIENT_ID);
           
            if(val <= v)
            {
                return NextRand();
            }
            if(player.sceneManager.GetCurScene()!=null)
            if (player.sceneManager.GetCurScene().entities.Find(e => e.guid == val) != null)
            {
                return NextRand();
            }
            if (usedGuids.Contains(val))
            {
                return NextRand();
            }
            else
            {
                usedGuids.Add(val);
                return val;
            }
        }
    }
    public class PlayerSafeZoneInfo
    {
        public int sceneNumId;
        public Vector3f position;
        public Vector3f rotation;
    }
    public class Player
    {
        public List<string> temporanyChatMessages = new(); //for cbt2 only as no chat exist
        public GuidRandomizer random;
        public Thread receivorThread;
        public Socket socket;
        //Data
        public string accountId = "";
        public string nickname = "Endministrator";
        public ulong roleId= 1;
        public uint level = 20;
        public uint xp = 0;
        //
        public Vector3f position;
        public Vector3f rotation;
        public Vector3f safeZonePoint; //Don't need to be saved
        public int curSceneNumId;
        public List<Character> chars = new List<Character>();
        public InventoryManager inventoryManager;
        public SpaceshipManager spaceshipManager;
        public SceneManager sceneManager;
        public GachaManager gachaManager;
        public BitsetManager bitsetManager;
        public FactoryManager factoryManager;
        public int teamIndex = 0;
        public List<Team> teams= new List<Team>();
        public List<Mail> mails = new List<Mail>();
        public List<int> unlockedSystems = new();
        public List<ulong> noSpawnAnymore = new();
        public long maxDashEnergy = 250;
        public uint curStamina = 10;
        public long nextRecoverTime = 0;
        public Dungeon currentDungeon;
        public PlayerSafeZoneInfo savedSaveZone;
        
        public uint maxStamina {
            get{
                return (uint)200;
            } 
        }
        public bool Initialized = false;

        
        public Player(Socket socket)
        {
            this.random = new(this);
            this.socket = socket;
            roleId = (ulong)new Random().Next();
            bitsetManager = new(this);
            inventoryManager = new(this);
            sceneManager = new(this);
            gachaManager = new(this);
            spaceshipManager = new(this);   
            factoryManager = new(this);
            receivorThread = new Thread(new ThreadStart(Receive));
           
        }
        public List<Character> GetCurTeam()
        {
            return chars.FindAll(c=> teams[teamIndex].members.Contains(c.guid));
        }
        public void Load(string accountId)
        {
            this.accountId = accountId;
            PlayerData data = DatabaseManager.db.GetPlayerById(this.accountId);
            
            if (data != null)
            {
                nickname=data.nickname;
                position = data.position;
                rotation = data.rotation;
                curSceneNumId = data.curSceneNumId;
                teams = data.teams;
                roleId = data.roleId;
                random.v = data.totalGuidCount;
                teamIndex = data.teamIndex;
                if(data.unlockedSystems!=null)
                unlockedSystems = data.unlockedSystems;
                if (data.noSpawnAnymore != null)
                    noSpawnAnymore = data.noSpawnAnymore;
                maxDashEnergy = data.maxDashEnergy;
                curStamina = data.curStamina;
                nextRecoverTime=data.nextRecoverTime;
                LoadCharacters();
                mails = DatabaseManager.db.LoadMails(roleId);
                inventoryManager.Load();
                spaceshipManager.Load();
                if (data.scenes != null)
                {
                    sceneManager.scenes = data.scenes;
                }
                bitsetManager.Load(data.bitsets);
                savedSaveZone = data.savedSafeZone;
            }
            else
            {
                Initialize(); //only if no account found
            }
            sceneManager.Load();
            factoryManager.Load();
        }
        public void LoadCharacters()
        {
            chars = DatabaseManager.db.LoadCharacters(roleId);
        }
        //Added in 1.0.7
        public Character GetCharacter(ulong guid)
        {
            return chars.Find(c => c.guid == guid);
        }
        public Character GetCharacter(string templateId)
        {
            return chars.Find(c => c.id==templateId);
        }
        public void Initialize()
        {
            foreach (var item in ResourceManager.characterTable)
            {
                chars.Add(new Character(roleId,item.Key,20));
            }
            foreach(var item in itemTable)
            {
                if(item.Value.maxStackCount == -1)
                {
                    inventoryManager.items.Add(new Item(roleId, item.Value.id, 10000000));
                }
                else
                {
                    inventoryManager.items.Add(new Item(roleId, item.Value.id, item.Value.maxStackCount));
                }
                
            }
            teams.Add(new Team()
            {
                leader = chars[0].guid,
                members={ chars[0].guid }
            });
            teams.Add(new Team());
            teams.Add(new Team());
            teams.Add(new Team());
            teams.Add(new Team());

            /*mails.Add(new Mail()
            {
                expireTime=DateTime.UtcNow.AddDays(30).Ticks,
                sendTime=DateTime.UtcNow.Ticks,
                claimed=false,
                guid=random.Next(),
                owner=roleId,
                isRead=false,
                content=new Mail_Content()
                {
                    content= "Welcome to ArkField PS, Join our Discord for help: https://discord.gg/5uJGJJEFHa",
                    senderName="SuikoAkari",
                    title="Welcome",
                    templateId="",
                }

            });*/

            UnlockImportantSystems();
            spaceshipManager.Load();
        }
        public void UnlockImportantSystems()
        {
            unlockedSystems.Add((int)UnlockSystemType.Watch);
            unlockedSystems.Add((int)UnlockSystemType.Weapon);
            unlockedSystems.Add((int)UnlockSystemType.Equip);
            unlockedSystems.Add((int)UnlockSystemType.EquipEnhance);
            unlockedSystems.Add((int)UnlockSystemType.NormalAttack);
            unlockedSystems.Add((int)UnlockSystemType.NormalSkill);
            unlockedSystems.Add((int)UnlockSystemType.UltimateSkill);
            unlockedSystems.Add((int)UnlockSystemType.TeamSkill);
            unlockedSystems.Add((int)UnlockSystemType.ComboSkill);
            unlockedSystems.Add((int)UnlockSystemType.TeamSwitch);
            unlockedSystems.Add((int)UnlockSystemType.Dash);
            unlockedSystems.Add((int)UnlockSystemType.Jump);
            unlockedSystems.Add((int)UnlockSystemType.Friend);
            unlockedSystems.Add((int)UnlockSystemType.SNS);
            unlockedSystems.Add((int)UnlockSystemType.Settlement);
            unlockedSystems.Add((int)UnlockSystemType.Map);
            unlockedSystems.Add((int)UnlockSystemType.FacZone);
            unlockedSystems.Add((int)UnlockSystemType.FacHub);
            unlockedSystems.Add((int)UnlockSystemType.AdventureBook);
            unlockedSystems.Add((int)UnlockSystemType.FacSystem);
            unlockedSystems.Add((int)UnlockSystemType.CharUI);
            unlockedSystems.Add((int)UnlockSystemType.EquipProduce);
            unlockedSystems.Add((int)UnlockSystemType.EquipTech);
            unlockedSystems.Add((int)UnlockSystemType.Gacha);
            unlockedSystems.Add((int)UnlockSystemType.Inventory);
            unlockedSystems.Add((int)UnlockSystemType.ItemQuickBar);
            unlockedSystems.Add((int)UnlockSystemType.ItemSubmitRecycle);
            unlockedSystems.Add((int)UnlockSystemType.ItemUse);
            unlockedSystems.Add((int)UnlockSystemType.Mail);
            unlockedSystems.Add((int)UnlockSystemType.ValuableDepot);
            unlockedSystems.Add((int)UnlockSystemType.Wiki);
            unlockedSystems.Add((int)UnlockSystemType.AIBark);
            unlockedSystems.Add((int)UnlockSystemType.AdventureExpAndLv);
            unlockedSystems.Add((int)UnlockSystemType.CharTeam);
            unlockedSystems.Add((int)UnlockSystemType.FacMode);
            unlockedSystems.Add((int)UnlockSystemType.FacOverview);
            unlockedSystems.Add((int)UnlockSystemType.SpaceshipSystem);
            unlockedSystems.Add((int)UnlockSystemType.SpaceshipControlCenter);
            unlockedSystems.Add((int)UnlockSystemType.FacBUS);
            unlockedSystems.Add((int)UnlockSystemType.PRTS);
            unlockedSystems.Add((int)UnlockSystemType.Dungeon);
            unlockedSystems.Add((int)UnlockSystemType.RacingDungeon);
            unlockedSystems.Add((int)UnlockSystemType.CheckIn);
            unlockedSystems.Add((int)UnlockSystemType.SubmitEther);
            unlockedSystems.Add((int)UnlockSystemType.FacZone);

        }
        public void EnterScene()
        {
            if (curSceneNumId == 0)
            {
                EnterScene(Server.config.serverOptions.defaultSceneNumId); //or 101                
            }
            else
            {
                //sceneManager.UnloadCurrent(false);
                //sceneManager.LoadCurrent();
                LoadFinish = false;
                Send(new PacketScEnterSceneNotify(this, curSceneNumId));
            }
            if (savedSaveZone == null || savedSaveZone.sceneNumId == 0)
            {
                savedSaveZone = new PlayerSafeZoneInfo()
                {
                    sceneNumId = curSceneNumId,
                    position = this.position,
                    rotation = this.rotation
                };
            }
        }
        public bool LoadFinish = true;
        public void EnterScene(int sceneNumId, Vector3f pos, Vector3f rot, PassThroughData passThroughData = null)
        {
           // if (!LoadFinish) return;
            if (GetLevelData(sceneNumId) != null)
            {
                string sceneConfigPathCur = GetLevelData(curSceneNumId).defaultState.exportedSceneConfigPath;
                string sceneConfigPathNew = GetLevelData(sceneNumId).defaultState.exportedSceneConfigPath;
                if (sceneConfigPathCur != sceneConfigPathNew)
                {
                    sceneManager.UnloadAllByConfigPath(sceneConfigPathCur);
                }
                curSceneNumId = sceneNumId;
                position = pos;
                rotation = rot;
                LoadFinish = false;
                Send(new PacketScEnterSceneNotify(this, sceneNumId, pos, passThroughData));
                //sceneManager.LoadCurrent();
            }
            else
            {
                Logger.PrintError($"Scene {sceneNumId} not found");
            }
        }
        public void EnterScene(int sceneNumId)
        {
            if(GetLevelData(sceneNumId) != null)
            {
                //sceneManager.UnloadCurrent(true);
                string sceneConfigPathCur = GetLevelData(curSceneNumId).defaultState.exportedSceneConfigPath;
                string sceneConfigPathNew = GetLevelData(sceneNumId).defaultState.exportedSceneConfigPath;
                if(sceneConfigPathCur != sceneConfigPathNew)
                {
                   sceneManager.UnloadAllByConfigPath(sceneConfigPathCur);
                }
                curSceneNumId = sceneNumId;
                position = GetLevelData(sceneNumId).playerInitPos;
                rotation = GetLevelData(sceneNumId).playerInitRot;
                // sceneManager.LoadCurrent();
                LoadFinish = false;
                Send(new PacketScEnterSceneNotify(this, sceneNumId));
                
            }
            else
            {
                Logger.PrintError($"Scene {sceneNumId} not found");
            }

        }

        public bool SocketConnected(Socket s)
        {
            try
            {
                return !((s.Poll(1000, SelectMode.SelectRead) && (s.Available == 0)) || !s.Connected);
            }
            catch (Exception e)
            {
                return false;
            }
            
        }
        public void Send(Packet packet, ulong seq = 0, uint totalPackCount = 1, uint currentPackIndex = 0)
        {
            Send(Packet.EncodePacket(packet,seq,totalPackCount,currentPackIndex));
        }
        public void Send(ScMessageId id,IMessage mes, ulong seq = 0, uint totalPackCount = 1, uint currentPackIndex = 0)
        {
            Send(Packet.EncodePacket((int)id, mes, seq, totalPackCount, currentPackIndex));
        }
        public void Send(Packet packet)
        {
            byte[] datas = packet.set_body.ToByteArray();
            int maxChunkSize = 65535;

            if(datas.Length < maxChunkSize)
            {
                Send(Packet.EncodePacket(packet));
                return;
            }
            int totalChunks = Math.Max((int)Math.Ceiling((double)datas.Length / maxChunkSize), 1);
            List<byte[]> chunks = new List<byte[]>();
            for (int i = 0; i < totalChunks; i++)
            {
                int offset = i * maxChunkSize;
                int chunkSize = Math.Min(maxChunkSize, datas.Length - offset);
                byte[] chunk = new byte[chunkSize];
                Array.Copy(datas, offset, chunk, 0, chunkSize);
                chunks.Add(chunk);
            }
            ulong seqNext = Packet.seqNext;
            // Stampa i chunk (opzionale)
            for (int i = 0; i < chunks.Count; i++)
            {
                byte[] data = chunks[i];
                
                Send(Packet.EncodePacket(packet.cmdId, data, seqNext, (uint)chunks.Count, (uint)i));
            }
        }
        public void Send(byte[] data)
        {
            try
            {
                socket.Send(data);
            }
            catch (Exception e)
            {
                Disconnect();
            }
            
        }
        public static byte[] ConcatenateByteArrays(byte[] array1, byte[] array2)
        {
            return array1.Concat(array2).ToArray();
        }
        
        public void Receive()
        {
            try
            {
                while (SocketConnected(socket))
                {
                    byte[] buffer = new byte[3];
                    int length = socket.Receive(buffer);
                    if (length == 3)
                    {
                        Packet packet = null;
                        byte headLength = Packet.GetByte(buffer, 0);
                        ushort bodyLength = Packet.GetUInt16(buffer, 1);
                        byte[] moreData = new byte[bodyLength + headLength];
                        while (socket.Available < moreData.Length)
                        {

                        }
                        int mLength = socket.Receive(moreData);
                        if (mLength == moreData.Length)
                        {
                            buffer = ConcatenateByteArrays(buffer, moreData);
                            packet = Packet.Read(this, buffer);

                            if (Server.config.logOptions.packets && !Server.csMessageToHide.Contains((CsMessageId)packet.csHead.Msgid))
                            {
                                Logger.Print("CmdId: " + (CsMessageId)packet.csHead.Msgid);
                                Logger.Print(BitConverter.ToString(packet.finishedBody).Replace("-", string.Empty).ToLower());
                            }
                           
                            try
                            {
                                NotifyManager.Notify(this, (CsMessageId)packet.cmdId, packet);
                            }
                            catch (Exception e)
                            
                            {
                                
                                Logger.PrintError("Error while notify packet: " + e.Message+": "+ e.StackTrace);
                            }

                        }
                    }
                }
            }
            catch(Exception e)
            {

            }
            



            Disconnect();
        }
        public void Kick(CODE code, string optionalMsg="")
        {

            Send(ScMessageId.ScNtfErrorCode, new ScNtfErrorCode()
            {
                Details = optionalMsg,
                ErrorCode = (int)code
            });
            Disconnect();
        }
        public void Disconnect()
        {
            Server.clients.Remove(this);
            if (Initialized)
            {
                Initialized = false;
                Save();
                Logger.Print($"{nickname} Disconnected");
                socket.Disconnect(false);
            }
            
            
        }
        public void Save()
        {
            //Save playerdata
            DatabaseManager.db.SavePlayerData(this);
            inventoryManager.Save();
            spaceshipManager.Save();
            SaveCharacters();
            SaveMails();
            
        }
        public void AddStamina(uint stamina)
        {
            curStamina += stamina;
            if(curStamina > maxStamina)
            {
                curStamina = maxStamina;
            }
            if(Initialized)Send(new PacketScSyncStamina(this));
        }
        public void Update()
        {
            //Check recover time
            long curtimestamp = DateTime.UtcNow.ToUnixTimestampMilliseconds();
            if (curtimestamp >= nextRecoverTime)
            {
                nextRecoverTime= DateTime.UtcNow.AddMinutes(7).ToUnixTimestampMilliseconds();
                AddStamina(1);
            }
            if(LoadFinish)
            sceneManager.Update();
        }
        public void SaveMails()
        {
            foreach(Mail mail in mails)
            {
                DatabaseManager.db.UpsertMail(mail);
            }
        }
        public void SaveCharacters()
        {
            foreach(Character c in chars)
            {
                DatabaseManager.db.UpsertCharacter(c);
            }
        }

        public void EnterDungeon(string dungeonId, EnterRacingDungeonParam racingParam)
        {
            Dungeon dungeon = new()
            {
                player = this,
                prevPlayerPos = position,
                prevPlayerRot = rotation,
                prevPlayerSceneNumId = curSceneNumId,
                table = ResourceManager.dungeonTable[dungeonId],
            };
            this.currentDungeon = dungeon;
            ScEnterDungeon enter = new()
            {
                DungeonId = dungeonId,
                SceneId = dungeon.table.sceneId,
                
            };
           
            Send(new PacketScSyncAllUnlock(this));
            
            EnterScene(GetSceneNumIdFromLevelData(dungeon.table.sceneId));
            Send(ScMessageId.ScEnterDungeon, enter);

        }

        public void LeaveDungeon(CsLeaveDungeon req)
        {
            ScLeaveDungeon rsp = new()
            {
                DungeonId = req.DungeonId,
            };
            Send(ScMessageId.ScLeaveDungeon, rsp);
            Dungeon dungeon = currentDungeon;
            currentDungeon = null;
            EnterScene(dungeon.prevPlayerSceneNumId, dungeon.prevPlayerPos, dungeon.prevPlayerRot);
        }

        public string GetCurrentChapter()
        {
            
            DomainDataTable table = domainDataTable.Values.ToList().Find(c => c.levelGroup.Contains(GetLevelData(curSceneNumId).id));
            if (table != null)
            {
                return table.domainId;
            }
            else
            {
                return "";
            }
           
        }
    }
}
