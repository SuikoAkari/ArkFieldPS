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

            sender.unlockedSystems.Clear();
            sender.UnlockImportantSystems();
            sender.maxDashEnergy = 250;
            foreach (var system in sender.unlockedSystems)
            {
                ScUnlockSystem unlocked = new()
                {
                    UnlockSystemType = system,
                };
                sender.Send(ScMessageId.ScUnlockSystem, unlocked);
            }
        }
    }
}
