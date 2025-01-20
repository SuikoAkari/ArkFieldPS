using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EndFieldPS
{
    public class ConfigFile
    {
        public bool InternalProxy = true;
        public string LocalIp = "127.0.0.1";
        public int LocalPort = 22102;
        public int MaxClients = 20;
        public bool QuestEnabled = false;
    }
}
