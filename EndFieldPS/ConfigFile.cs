using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EndFieldPS
{
    public class ConfigFile
    {
        /*  public string ServerIp = "127.0.0.1";
          public int LocalPort = 30000;
          public int DispatchPort = 5000;
          public string DispatchIp = "127.0.0.1";
          public int MaxClients = 20;
          public bool QuestEnabled = false; //Not used

          public string MongoDb_connectionUri = "mongodb://localhost:27017";
          public string MongoDb_databaseName = "endfieldps";*/
        public MongoDatabaseSettings mongoDatabase = new();
        public DispatchServerSettings dispatchServer = new();
        public GameserverSettings gameServer = new();
        public ServerOptions serverOptions = new();
        public LogSettings logOptions = new();

    }
    public struct ServerOptions
    {
        public int defaultSceneNumId = 98;
        public int maxPlayers = 20;

        public ServerOptions()
        {
        }


       /* public struct WelcomeMail
        {

        }*/
    }
    public struct LogSettings
    {
        public bool packets;
        public bool debugPrint=true;

        public LogSettings()
        {
        }
    }
    public struct GameserverSettings
    {
        public string bindAddress = "127.0.0.1";
        public int bindPort = 30000;

        public GameserverSettings()
        {
        }
    }
    public struct DispatchServerSettings
    {
        public string bindAddress = "127.0.0.1";
        public int bindPort = 5000;
        public string emailFormat = "@endfield.ps";
        public DispatchServerSettings()
        {
        }
    }
    public struct MongoDatabaseSettings
    {
        public string uri = "mongodb://localhost:27017";
        public string collection = "endfieldps";
        public MongoDatabaseSettings()
        {
        }
    }
}
