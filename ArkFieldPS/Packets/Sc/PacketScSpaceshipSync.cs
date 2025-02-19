using ArkFieldPS.Network;
using ArkFieldPS.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ArkFieldPS.Packets.Sc
{
    public class PacketScSpaceshipSync : Packet
    {

        public PacketScSpaceshipSync(Player client) {
            ScSpaceshipSync proto = new ScSpaceshipSync()
            {

                Rooms =
                {
                   /* new ScdSpaceshipRoom()
                    {
                        Id="control_center",
                        Type=0,
                        ControlCenter = new()
                        {

                        },
                        Level=4,
                        HasCharWorking=true,
                        LevelUpConditionFlags =
                        {
                            { "control_center_level_5",true}
                        },
                        LevelUpConditonValues =
                        {
                            { "control_center_level_5",4}
                        },
                        StationedCharList =
                        {
                            "chr_0004_pelica"
                        },
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
                        },

                    },
                    new ScdSpaceshipRoom()
                    {
                        Id= "manufacturing_station_1",
                        Type=1,
                        Level=1,
                        ManufacturingStation = new()
                        {

                        },

                    }*/
                },
                Chars =
                {
                   /* new ScdSpaceshipChar()
                    {
                        CharId="chr_0004_pelica",
                        StationedRoomId="control_center",
                        PhysicalStrength=9802.77637f,
                        
                        Favorability=918,

                        IsWorking=true,
                        Skills =
                        {
                            new ScdSpaceshipCharSkill()
                            {
                                Index=1,
                                SkillId="spaceship_skill_acc_charmaterial_produce2_1"
                            },
                            new ScdSpaceshipCharSkill()
                            {
                                Index=0,
                                SkillId="spaceship_skill_acc_all_ps_recovery1_2"
                            }

                        },

                    }*/
                }
            };
            client.spaceshipManager.chars.ForEach(c =>
            {
                proto.Chars.Add(c.ToSpaceshipChar());
            });
            client.spaceshipManager.rooms.ForEach(c =>
            {
                proto.Rooms.Add(c.ToRoomProto());
            });
            SetData(ScMessageId.ScSpaceshipSync, proto);
        }

    }
}
