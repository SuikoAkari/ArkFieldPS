using EndFieldPS.Game.Spaceship;
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
    public class PacketScSpaceshipSyncRoomStation : Packet
    {

        public PacketScSpaceshipSyncRoomStation(Player client,SpaceshipRoom room) {

           
            ScSpaceshipSyncRoomStation proto = new ScSpaceshipSyncRoomStation()
            {
                Rooms =
                {
                    new ScdSpaceshipRoomStation()
                    {
                        StationedCharList =
                        {
                            room.stationedCharList,
                        },
                        HasCharWorking=room.HasCharWorking(),
                        Id=room.id,
                        Type=room.GetType(),
                        AttrsMap =
                        {
                            {0, new ScdSpaceshipRoomAttr()
                            {
                                Value=24.8f,
                                TheoreticalValue=24.8f,
                                BaseAttrs =
                                {
                                    new ScdSpaceshipRoomAttrUnit()
                                    {
                                        Value=20,

                                        Source = new()
                                        {
                                            SourceType=1,
                                        }
                                    }
                                },
                                PercentAttrs =
                                {
                                    new ScdSpaceshipRoomAttrUnit()
                                    {
                                        Value=0.24f,
                                        Source = new()
                                        {
                                            CharId="chr_0004_pelica",
                                            SkillId="spaceship_skill_acc_all_ps_recovery1_2"
                                        }
                                    }
                                }
                            } },
                            {1, new ScdSpaceshipRoomAttr()
                            {
                                Value=12,
                                TheoreticalValue=12,
                                BaseAttrs =
                                {
                                    new ScdSpaceshipRoomAttrUnit()
                                    {
                                        Type=1,
                                        Value=12,
                                        Source = new()
                                        {
                                            SourceType=1
                                        }
                                    }
                                }
                            }
                            }
                        }
                    }
                },
                

            };
            foreach (var chara in client.spaceshipManager.chars)
            {
                proto.Chars.Add(new ScdSpaceshipCharStation()
                {
                    CharId=chara.id,
                    IsWorking=chara.isWorking,
                    StationedRoomId=chara.stationedRoomId,
                    PhysicalStrength=chara.physicalStrength,
                    
                });
            }
            SetData(ScMessageId.ScSpaceshipSyncRoomStation, proto);
        }

    }
}
