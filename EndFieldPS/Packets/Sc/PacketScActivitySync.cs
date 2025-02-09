using EndFieldPS.Network;
using EndFieldPS.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace EndFieldPS.Packets.Sc
{
    public class PacketScActivitySync : Packet
    {

        public PacketScActivitySync(Player client) {

            ScActivitySync proto = new ScActivitySync()
            {
                Info =
                {
                    new ActivityInfo()
                    {
                        Version=8,
                        IsEnable = true,
                        Id="activity_checkin_1",
                        StartTime=1736820000,
                        EndTime=1739473200,
                        Typ=ActivityType.Checkin,
                        Data = new()
                        {
                            Checkin = new()
                            {
                                LoginDays=1,
                                RewardDays=16,
                            }
                        }
                    }
                }
                
            };

            SetData(ScMessageId.ScActivitySync, proto);
        }

    }
}
