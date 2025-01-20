using EndFieldPS.Protocol;
using Org.BouncyCastle.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EndFieldPS.Commands
{
    public static class BaseCommands
    {

        [Server.Command("unlockall", "Unlock all")]
        public static void UnlockAllCmd(string cmd, string[] args)
        {
            
        }
        
    }
}