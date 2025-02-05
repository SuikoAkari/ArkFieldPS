using EndFieldPS.Game.Entities;
using EndFieldPS.Packets.Sc;
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
        public static void Handle(Player sender, string cmd, string[] args, Player target)
        {
            target.GetCurTeam().ForEach(chara =>
            {
                chara.curHp = chara.CalcAttributes()[AttributeType.MaxHp];
                
            });
            target.Send(ScMessageId.ScSceneRevival, new ScSceneRevival()
            {
                
            });
            target.sceneManager.LoadCurrentTeamEntities();
            target.Send(new PacketScSelfSceneInfo(target,true,SelfInfoReasonType.SlrReviveRest));
            CommandManager.SendMessage(sender, "Healed!");
        }
    }
}
