
using EndFieldPS.Network;
using EndFieldPS.Protocol;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Pastel;
using SQLite;
using SQLiteNetExtensions.Attributes;
using System.Drawing;


using System.Linq;
using Org.BouncyCastle.Ocsp;
using System.Numerics;
using MongoDB.Bson.Serialization.Attributes;
using System.Reflection;
using System.Net.Sockets;
using static EndFieldPS.Dispatch;
using Org.BouncyCastle.Crypto.Engines;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Collections;
using System;
using EndFieldPS.Packets.Sc;

namespace EndFieldPS
{
    

    public class GuidRandomizer
    {
        public int v = 0;
        public int Next()
        {
            v++;
            return v;
        }
    }

    public class Client
    {
        public GuidRandomizer random = new GuidRandomizer();
        public Thread receivorThread;
        public Socket socket;
        public ulong roleId= 1;
        public int curSceneNumId;
        public Client(Socket socket)
        {
            this.socket = socket;
            receivorThread = new Thread(new ThreadStart(Receive));

        }
        public void EnterScene(int sceneNumId)
        {
            curSceneNumId = sceneNumId;
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


                            Server.Print("CmdId: " + (CsMessageId)packet.csHead.Msgid);
                            NotifyManager.Notify(this, (CsMessageId)packet.cmdId, packet);
                        }
                       

                    }
                }



            //client.Disconnect();
        }
    }
}
