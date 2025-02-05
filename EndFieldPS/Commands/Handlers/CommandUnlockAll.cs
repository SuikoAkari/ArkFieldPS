using EndFieldPS.Database;
using EndFieldPS.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EndFieldPS.Commands.Handlers
{
    public static class CommandUnlockAll
    {
        [Server.Command("unlockall", "Unlock all systems",true)]
        public static void Handle(Player sender,string cmd, string[] args, Player target)
        {

            target.unlockedSystems.Clear();
            target.UnlockImportantSystems();
            target.maxDashEnergy = 250;
            foreach (var system in target.unlockedSystems)
            {
                ScUnlockSystem unlocked = new()
                {
                    UnlockSystemType = system,
                };
                target.Send(ScMessageId.ScUnlockSystem, unlocked);
            }
        }
    }
}
