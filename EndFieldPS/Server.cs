
using BeyondTools.VFS.Crypto;
using EndFieldPS.Commands;
using EndFieldPS.Network;
using EndFieldPS.Protocol;
using EndFieldPS.Resource;
using Google.Protobuf;
using Newtonsoft.Json;
using Pastel;
using SQLite;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Titanium.Web.Proxy.Http;
using static EndFieldPS.Dispatch;


namespace EndFieldPS
{
    public class Server
    {
        public class HandlerAttribute : Attribute
        {
            public CsMessageId CmdId { get; set; }
            public HandlerAttribute(CsMessageId cmdID)
            {
                this.CmdId = cmdID;
            }
            public delegate void HandlerDelegate(Client client, int cmdId, Network.Packet packet);
        }
        public class CommandAttribute : Attribute
        {
            public string command;
            public string desc;

            public CommandAttribute(string cmdName, string desc = "No description")
            {
                this.command = cmdName;
                this.desc = desc;
            }
            public delegate void HandlerDelegate(string command, string[] args);
        }
        public static List<Client> clients = new List<Client>();
        public static string ServerVersion = "1.0.0";
        public static bool Initialized = false;
        public IntPtr server;
        public static bool showLogs = true;
        public static SQLiteConnection _db;
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
            
            Logger.Initialize(); // can also pass hideLogs here
            showLogs = !hideLogs;
            // showLogs = false;
            Print($"Logs are {(showLogs ? "enabled" : "disabled")}");
            Server.config = config;
            //Resource Manager todo
            ResourceManager.Init();
            new Thread(new ThreadStart(DispatchServer)).Start();
            Initialized = true;
            IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
            int port = 30000;

            // Crea il socket del server
            Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                // Associa il socket all'indirizzo IP e alla porta
                serverSocket.Bind(new IPEndPoint(ipAddress, port));

                // Avvia l'ascolto delle connessioni
                serverSocket.Listen(10); // Fino a 10 connessioni in coda
                Console.WriteLine($"Server in ascolto su {ipAddress}:{port}");

                while (true)
                {
                    Console.WriteLine("In attesa di connessioni...");

                    // Accetta una connessione in arrivo
                    Socket clientSocket = serverSocket.Accept();
                    Console.WriteLine("Client connesso!");
                    if (clientSocket.Connected)
                    {
                        Client client = new Client(clientSocket);
                        clients.Add(client);
                        client.receivorThread.Start();

                        Print("Connected new client: " + clients.Count());
                    }


                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Errore: {ex.Message}");
            }
            finally
            {
                // Arresta il server
                serverSocket.Close();
                Console.WriteLine("Server arrestato.");
            }

        }
        public void DispatchServer()
        {
            dispatch = new Dispatch();
            dispatch.Start();
        }
        public static CsMessageId[] hideLog = [];


        public static uint GetCurrentSeconds()
        {
            return (uint)DateTime.Now.Millisecond / 1000;
        }
       
        public static string ColoredText(string text, string color)
        {
            return text.Pastel(color);
        }
        public static void Print(string text)
        {
            Logger.Log(text);
            Console.WriteLine($"[{ColoredText("Server", "03fcce")}] " + text);
        }
        public static void Shutdown()
        {
            //TODO
        }
    }
}
