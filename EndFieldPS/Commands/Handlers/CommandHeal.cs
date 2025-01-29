using EndFieldPS.Game.Entities;
using EndFieldPS.Protocol;
using EndFieldPS.Resource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EndFieldPS.Commands.Handlers
{
    public static class CommandHeal
    {

        [Server.Command("heal", "Revives/Heals your team characters", true)]
        public static void HealCmd(string cmd, string[] args, Player target)
        {
            target.sceneManager.GetCurScene().entities.FindAll(e => e is EntityCharacter).ForEach(e =>
            {
                EntityCharacter chara = (EntityCharacter)e;
                chara.GetChar().curHp = 0;
                chara.Heal(chara.GetChar().CalcAttributes()[AttributeType.MaxHp]);
            });
            target.Send(ScMessageId.ScSceneRevival, new ScSceneRevival()
            {
                
            });
            Logger.Print("Healed!");
        }
    }
}
