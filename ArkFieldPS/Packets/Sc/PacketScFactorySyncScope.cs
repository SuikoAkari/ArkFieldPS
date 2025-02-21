using ArkFieldPS.Network;
using ArkFieldPS.Protocol;

namespace ArkFieldPS.Packets.Sc
{
    public class PacketScFactorySyncScope : Packet
    {

        public PacketScFactorySyncScope(Player client) {
            // TODO: Remove hardcoded values
            SetData(ScMessageId.ScFactorySyncScope, new ScFactorySyncScope()
            {
                ScopeName = 1,
                CurrentChapterId = "domain_1",
                Quickbars =
                {
                    new ScdFactorySyncQuickbar()
                    {
                        List =
                        {
                            "item_port_power_pole_2",
                            "item_port_travel_pole_1",
                            "item_port_power_diffuser_1",
                            "item_port_grinder_1",
                            "item_port_battle_turret_1",
                            "item_port_miner_2",
                            "item_port_miner_3",
                            "item_port_sp_hub_1"
                        }
                    },
                    new ScdFactorySyncQuickbar()
                    {
                        Type = 1,
                        List =
                        {
                            "",
                            "",
                            "",
                            "",
                            "",
                            "",
                            "",
                            ""
                        }
                    }
                },
                TransportRoute = new()
                {
                    
                    UpdateTs = DateTime.UtcNow.AddMinutes(1).ToUnixTimestampMilliseconds()/1000,
                    Routes =
                    {
                        new ScdFactoryHubTransportRoute
                        {
                            ChapterId = "domain_1",
                            Index = 1,
                            
                        },
                        new ScdFactoryHubTransportRoute
                        {
                            ChapterId = "domain_2",
                            Index = 2,

                        }
                    }
                },
                BookMark = new()
                {
                
                },
                
            });
        }

    }
}
