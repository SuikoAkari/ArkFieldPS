using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArkFieldPS
{
    public class ConfigFile
    {
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
        public bool debugPrint=false;

        public LogSettings()
        {
        }
    }
    public struct GameserverSettings
    {
        public string bindAddress = "127.0.0.1";
        public int bindPort = 30000;
        public string accessAddress = "127.0.0.1";
        public int accessPort = 30000;
        public GameserverSettings()
        {
        }
    }
    public struct DispatchServerSettings
    {
        public string bindAddress = "127.0.0.1";
       
        public int bindPort = 5000;
        public string accessAddress = "127.0.0.1";
        public int accessPort = 5000;
        public string emailFormat = "@endfield.ps";
        public DispatchServerSettings()
        {
        }
    }
    public struct MongoDatabaseSettings
    {
        public string uri = "mongodb://localhost:27017";
        public string collection = "ArkFieldPS";
        public MongoDatabaseSettings()
        {
        }
    }
}
