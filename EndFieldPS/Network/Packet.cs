using EndFieldPS.Protocol;
using Google.Protobuf;
using Pastel;
using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

using static System.Runtime.InteropServices.JavaScript.JSType;

namespace EndFieldPS.Network
{
    public class Packet
    {
        public int cmdId;
        public byte[] finishedBody;
        public CSHead csHead;
        public IMessage set_body;
        public static void PutUInt16(byte[] buf, ushort networkValue, int offset)
        {
            byte[] bytes = BitConverter.GetBytes(networkValue);
            Buffer.BlockCopy(bytes, 0, buf, offset, bytes.Length);
        }
       
        public static void PutUInt32(byte[] buf, uint networkValue, int offset)
        {
            byte[] bytes = BitConverter.GetBytes(networkValue);
            Buffer.BlockCopy(bytes, 0, buf, offset, bytes.Length);
        }
        public static void PutUInt64(byte[] buf, ulong networkValue, int offset)
        {
            byte[] bytes = BitConverter.GetBytes(networkValue);
            Buffer.BlockCopy(bytes, 0, buf, offset, bytes.Length);
        }
        public static ushort GetUInt16(byte[] buf, int index)
        {
            ushort networkValue = BitConverter.ToUInt16(buf, index);
            return networkValue;
        }

        public static uint GetUInt32(byte[] buf,int index)
        {
            uint networkValue = BitConverter.ToUInt32(buf, index);
            return (uint)IPAddress.NetworkToHostOrder((int)networkValue);
        }
        public static void PutByte(byte[] buf, byte networkValue, int offset)
        {
            byte[] bytes = new byte[1] {networkValue };
            Buffer.BlockCopy(bytes, 0, buf, offset, bytes.Length);
        }
        public static byte GetByte(byte[] buf, int index)
        {
            byte networkValue = buf[index];
            return networkValue;
        }
        public TBody DecodeBody<TBody>() where TBody : IMessage<TBody>, new()
        {
            return new MessageParser<TBody>(() => new()).ParseFrom(finishedBody);
        }
        public static void PutByteArray(byte[] destination, byte[] source, int offset)
        {
            if (destination == null)
                throw new ArgumentNullException(nameof(destination));
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (offset < 0 || offset > destination.Length - source.Length)
                throw new ArgumentOutOfRangeException(nameof(offset), "Offset is out of range.");

            Buffer.BlockCopy(source, 0, destination, offset, source.Length);
        }
        public static byte[] ToByteArray(IntPtr ptr, int length)
        {
            if (ptr == IntPtr.Zero)
            {
                throw new ArgumentException("Pointer cannot be null", nameof(ptr));
            }

            byte[] byteArray = new byte[length];
            Marshal.Copy(ptr, byteArray, 0, length);
            return byteArray;
        }
        public static IntPtr ByteArrayToIntPtr(byte[] data)
        {
            if (data == null) throw new ArgumentNullException(nameof(data));

            // Allocate unmanaged memory
            IntPtr ptr = Marshal.AllocHGlobal(data.Length);

            // Copy the byte array to the unmanaged memory
            Marshal.Copy(data, 0, ptr, data.Length);

            return ptr;
        }
        public static byte[] EncryptWithPublicKey(byte[] data, string publicKey)
        {
            // Crea un oggetto RSA
            using (RSA rsa = RSA.Create())
            {

                publicKey = publicKey.Replace("-----BEGIN PUBLIC KEY-----", "");
                publicKey = publicKey.Replace("\r", "");
                publicKey = publicKey.Replace("\n", "");
                publicKey = publicKey.Replace("-----END PUBLIC KEY-----", "");
                publicKey = publicKey.Trim();
                Logger.Print(publicKey);
                byte[] publicKey_ = Convert.FromBase64String(publicKey);
                // Importa la chiave pubblica
                rsa.ImportSubjectPublicKeyInfo(publicKey_, out _);

                // Crittografa i dati
                return rsa.Encrypt(data, RSAEncryptionPadding.OaepSHA256);
            }
        }
        public Packet SetData(ScMessageId msgId, IMessage body)
        {
            set_body = body;
            cmdId = (int)msgId;
            return this;
        }
        public static byte[] EncodePacket(Packet packet)
        {
            return EncodePacket(packet.cmdId,packet.set_body);
        }
        public static ulong seqNext = 1;
        public static byte[] EncodePacket(int msgId, IMessage body)
        {
            seqNext++;
            CSHead head = new() { Msgid = msgId, DownSeqid= seqNext, TotalPackCount=1 };
            int totalSerializedDataSize = 3 + head.ToByteArray().Length + body.ToByteArray().Length;
            byte[] data = new byte[totalSerializedDataSize];
            PutByte(data, (byte)head.ToByteArray().Length, 0);
            PutUInt16(data, (ushort)body.ToByteArray().Length, 1);
            PutByteArray(data, head.ToByteArray(), 3);
            PutByteArray(data, body.ToByteArray(), 3+head.ToByteArray().Length);
            if(Server.config.logOptions.packets)
                Logger.Print($"Sending packet: {((ScMessageId)msgId).ToString().Pastel(Color.LightBlue)} id: {msgId} with {data.Length} bytes");

            return data;
        }
        public static Packet Read(Player client,byte[] byteArray)
        {
            byte headLength = GetByte(byteArray, 0);
            ushort bodyLength = GetUInt16(byteArray, 1);
            
            byte[] csHeadBytes = new byte[headLength];
            byte[] BodyBytes = new byte[bodyLength];
            Array.Copy(byteArray, 3, csHeadBytes, 0, headLength);
            Array.Copy(byteArray, 3+ headLength, BodyBytes, 0, bodyLength);
            CSHead csHead_ = CSHead.Parser.ParseFrom(csHeadBytes);
            seqNext = csHead_.UpSeqid;
            return new Packet() { csHead = csHead_, finishedBody = BodyBytes,cmdId=csHead_.Msgid };
        }
    }
}
