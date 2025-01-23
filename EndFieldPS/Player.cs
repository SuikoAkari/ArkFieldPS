
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


namespace EndFieldPS
{
    public class GuidRandomizer
    {
        public ulong v = 0;
        public ulong Next()
        {
            v++;
            return v;
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
        public string token = "";
        public string nickname = "Endministrator";
        public ulong roleId= 1;
        public uint level = 20;
        public uint xp = 0;
        //
        public Vector3f position;
        public Vector3f rotation;
        public int curSceneNumId;
        public List<Character> chars = new List<Character>();
        public InventoryManager inventoryManager;

        public int teamIndex = 0;
        public List<Team> teams= new List<Team>();  

        public Player(Socket socket)
        {
            this.socket = socket;
            roleId = (ulong)new Random().Next();
            inventoryManager = new(this);
            receivorThread = new Thread(new ThreadStart(Receive));
           
        }
        public void Load(string token)
        {
            //TODO
            this.token = token;
            Initialize(); //only if no account found (no mongodb implementation for now, so calling this directly)
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

        }
        public Weapon AddWeapon(string id, ulong level)
        {
            Weapon weapon = new Weapon(roleId, id, level);
            inventoryManager.weapons.Add(weapon);
            return weapon;
        }
        public void EnterScene(int sceneNumId)
        {
            curSceneNumId = sceneNumId;
            position = GetLevelData(sceneNumId).playerInitPos;
            rotation = GetLevelData(sceneNumId).playerInitRot;
            Send(new PacketScEnterSceneNotify(this,sceneNumId));
        }
        public bool SocketConnected(Socket s)
        {
            return !((s.Poll(1000, SelectMode.SelectRead) && (s.Available == 0)) || !s.Connected);
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
           
                while (SocketConnected(socket))
                {
                    byte[] buffer = new byte[3];
                    int length = socket.Receive(buffer);
                    if (length ==3)
                    {
                        Packet packet = null;
                        byte headLength = Packet.GetByte(buffer, 0);
                        ushort bodyLength = Packet.GetUInt16(buffer, 1);
                        byte[] moreData = new byte[bodyLength+headLength];
                        while (socket.Available < moreData.Length)
                        {

                        }
                        int mLength = socket.Receive(moreData);
                        if (mLength == moreData.Length)
                        {
                            buffer = ConcatenateByteArrays(buffer, moreData);
                            packet = Packet.Read(this, buffer);


                            Logger.Print("CmdId: " + (CsMessageId)packet.csHead.Msgid);
                            NotifyManager.Notify(this, (CsMessageId)packet.cmdId, packet);
                        }
                       

                    }
                }



            Disconnect();
        }

        public void Disconnect()
        {
           Server.clients.Remove(this);
            Logger.Print($"{nickname} Disconnected");
            //TODO Save
        }
    }
}
