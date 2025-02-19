using ArkFieldPS.Network;
using ArkFieldPS.Protocol;
using ArkFieldPS.Resource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ArkFieldPS.Packets.Sc
{
    public class PacketScSyncCharBagInfo : Packet
    {

        public PacketScSyncCharBagInfo(Player client) {

            ScSyncCharBagInfo proto = new()
            {
                ScopeName=1,
                
                CharInfo =
                {

                },
                CurrTeamIndex=client.teamIndex,
                MaxCharTeamMemberCount=4,
                
                TeamInfo =
                {

                },
               
            };
            client.chars.ForEach(c => proto.CharInfo.Add(c.ToProto()));
            client.teams.ForEach(c =>
            {
                proto.TeamInfo.Add(new CharTeamInfo()
                {
                    CharTeam = { c.members },
                    Leaderid=c.leader,
                    TeamName=c.name,
                });
            });
            SetData(ScMessageId.ScSyncCharBagInfo, proto);
        }

    }
}
