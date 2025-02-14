using ArkFieldPS.Game.Entities;
using ArkFieldPS.Packets.Sc;
using ArkFieldPS.Protocol;
using ArkFieldPS.Resource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArkFieldPS.Commands.Handlers
{
    public static class CommandHeal
    {

        [Server.Command("heal", "Revives/Heals your team characters", true)]
        public static void Handle(Player sender, string cmd, string[] args, Player target)
        {
            target.GetCurTeam().ForEach(chara =>
            {
                chara.curHp = chara.CalcAttributes()[AttributeType.MaxHp].val;
                
            });
            target.Send(ScMessageId.ScSceneRevival, new ScSceneRevival()
            {
                
            });
            target.sceneManager.LoadCurrentTeamEntities();
            target.Send(new PacketScSelfSceneInfo(target,SelfInfoReasonType.SlrReviveRest));
            CommandManager.SendMessage(sender, "Healed!");
        }
    }
}
