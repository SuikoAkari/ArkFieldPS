using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EndFieldPS
{
    public class ConfigFile
    {
        public string ServerIp = "127.0.0.1";
        public int LocalPort = 30000;
        public int DispatchPort = 5000;
        public string DispatchIp = "127.0.0.1";
        public int MaxClients = 20;
        public bool QuestEnabled = false; 
    }
}
