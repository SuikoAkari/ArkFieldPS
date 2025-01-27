
using EndFieldPS.Network;
using EndFieldPS.Protocol;
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
using static EndFieldPS.Dispatch;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Collections;
using System;
using EndFieldPS.Packets.Sc;
using EndFieldPS.Game.Character;
using EndFieldPS.Resource;
using EndFieldPS.Game.Inventory;
using static EndFieldPS.Resource.ResourceManager;
using EndFieldPS.Database;
using EndFieldPS.Game;


namespace EndFieldPS
{
    public class GuidRandomizer
    {
        public ulong v = 1;
        public ulong Next()
        {
            v++;
            return (ulong)v;
        }
    }
    public class Player
    {
        //TODO move to Team class
        public class Team
        {
            public string name="";
            public ulong leader;
            public List<ulong> members=new();
        }
        public GuidRandomizer random = new GuidRandomizer();
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
        public SceneManager sceneManager;
        public int teamIndex = 0;
        public List<Team> teams= new List<Team>();
        public bool Initialized = false;
        public Player(Socket socket)
        {
            this.socket = socket;
            roleId = (ulong)new Random().Next();
            inventoryManager = new(this);
            sceneManager = new(this);
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
                LoadCharacters();
                inventoryManager.Load();
            }
            else
            {
                Initialize(); //only if no account found
            }
            sceneManager.Load();
        }
        public void LoadCharacters()
        {
            chars = DatabaseManager.db.LoadCharacters(roleId);
            
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
                    inventoryManager.items.Add(new Item(roleId, item.Value.id, 1000000));
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

        }
        public void EnterScene()
        {
            if (curSceneNumId == 0)
            {
                EnterScene(Server.config.serverOptions.defaultSceneNumId); //or 101
            }
            else
            {
                Send(new PacketScEnterSceneNotify(this, curSceneNumId));
            }
        }
        public void EnterScene(int sceneNumId)
        {
            sceneManager.UnloadCurrent();
            curSceneNumId = sceneNumId;
            position = GetLevelData(sceneNumId).playerInitPos;
            rotation = GetLevelData(sceneNumId).playerInitRot;
            sceneManager.LoadCurrentTeamEntities();
            Send(new PacketScEnterSceneNotify(this,sceneNumId));
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
        public void Send(Packet packet)
        {
            Send(Packet.EncodePacket(packet));
        }
        public void Send(ScMessageId id,IMessage mes)
        {
            Send(Packet.EncodePacket((int)id, mes));
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

                            if (Server.config.logOptions.packets)
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
                                Logger.PrintError("Error while notify packet: " + e.Message);
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
            }
            
            
        }
        public void Save()
        {
            //Save playerdata
            DatabaseManager.db.SavePlayerData(this);
            SaveCharacters();
            inventoryManager.Save();
        }
        public void SaveCharacters()
        {
            foreach(Character c in chars)
            {
                DatabaseManager.db.UpsertCharacterAsync(c);
            }
        }
    }
}
