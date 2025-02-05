
using BeyondTools.VFS.Crypto;
using EndFieldPS.Commands;
using EndFieldPS.Database;
using EndFieldPS.Game;
using EndFieldPS.Http;
using EndFieldPS.Network;
using EndFieldPS.Protocol;
using EndFieldPS.Resource;
using Google.Protobuf;
using Newtonsoft.Json;
using Pastel;
using SQLite;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static EndFieldPS.Http.Dispatch;


namespace EndFieldPS
{
    public static class DateTimeExtensions
    {
        private static readonly DateTime UnixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public static long ToUnixTimestampMilliseconds(this DateTime dateTime)
        {
            // Calcola il numero di millisecondi dall'epoca UNIX
            return (long)(dateTime - UnixEpoch).TotalMilliseconds;
        }
    }
    public class Server
    {

        public class HandlerAttribute : Attribute
        {
            public CsMessageId CmdId { get; set; }
            public HandlerAttribute(CsMessageId cmdID)
            {
                this.CmdId = cmdID;
            }
            public delegate void HandlerDelegate(Player client, int cmdId, Network.Packet packet);
        }
        public class CommandAttribute : Attribute
        {
            public string command;
            public string desc;
            public bool requiredTarget;
            public CommandAttribute(string cmdName, string desc = "No description", bool requireTarget=false)
            {
                this.command = cmdName;
                this.desc = desc;
                this.requiredTarget = requireTarget;
            }
            public delegate void HandlerDelegate(Player sender, string command, string[] args, Player target);
        }
        public static List<Player> clients = new List<Player>();
        public static string ServerVersion = "1.0.7";
        public static bool Initialized = false;
        public static bool showLogs = true;
        public static Dispatch dispatch;
        public static ResourceManager resourceManager;
        public static ConfigFile config;
        public static ResourceManager GetResources()
        {
            return resourceManager;
        }
        public void Start(bool hideLogs = false, ConfigFile config = null)
        {
            {
                Assembly assembly = Assembly.GetExecutingAssembly();
                Type[] types = assembly.GetTypes();

                foreach (var type in types)
                {
                    NotifyManager.AddReqGroupHandler(type);
                    CommandManager.AddReqGroupHandler(type);
                }

                NotifyManager.Init();
                CommandManager.Init();
            }
            
            Logger.Initialize();
            Logger.Print($"Starting server version {ServerVersion} with supported client version {GameConstants.GAME_VERSION}");
            showLogs = !hideLogs;
            Logger.Print($"Logs are {(showLogs ? "enabled" : "disabled")}");
            Server.config = config;
            DatabaseManager.Init();
            ResourceManager.Init();
            new Thread(new ThreadStart(DispatchServer)).Start();
            
            IPAddress ipAddress = IPAddress.Parse(Server.config.gameServer.bindAddress);
            int port = Server.config.gameServer.bindPort;
            Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            new Thread(new ThreadStart(CmdListener)).Start();
            new Thread(new ThreadStart(Update)).Start();
            //Logger.Print(""+DateTime.UtcNow.ToUnixTimestampMilliseconds());
            try
            {
                serverSocket.Bind(new IPEndPoint(ipAddress, port));
                serverSocket.Listen(int.MaxValue);
                Logger.Print($"Server listening on {ipAddress}:{port}");
                Initialized = true;
                while (true)
                {
                    Socket clientSocket = serverSocket.Accept();
                    
                    if (clientSocket.Connected)
                    {
                        Player client = new Player(clientSocket);
                        clients.Add(client);
                        client.receivorThread.Start();

                        Logger.Print("Connected new client: " + clients.Count());
                    }


                }
            }
            catch (Exception ex)
            {
                Logger.PrintError($" {ex.Message}");
            }
            finally
            {
                serverSocket.Close();
                Logger.Print("Server stopped.");
            }

        }
        public void Update()
        {
            while (true)
            {
                try
                {
                    clients.ForEach(client => { if (client != null) client.Update(); });
                    Thread.Sleep(1000);
                }
                catch (Exception ex)
                {

                }
            }
        }
        public void CmdListener()
        {
            while (true)
            {
                try
                {
                    string cmd = Console.ReadLine()!;
                    string[] split = cmd.Split(" ");
                    string[] args = cmd.Split(" ").Skip(1).ToArray();
                    string command = split[0].ToLower();
                    CommandManager.Notify(null,command, args,clients.Find(c=>c.accountId==CommandManager.targetId));
                }
                catch (Exception ex)
                {
                    Logger.PrintError(ex.Message);
                }

            } 

        }
        public void DispatchServer()
        {
            dispatch = new Dispatch();
            dispatch.Start();
        }
        public static CsMessageId[] hideLog = [];

        public static string ColoredText(string text, string color)
        {
            return text.Pastel(color);
        }
        public static void Shutdown()
        {
            foreach (Player player in clients)
            {
                if(player.Initialized)
                player.Save();
                player.Kick(CODE.ErrServerClosed);
            }
        }
    }
}
