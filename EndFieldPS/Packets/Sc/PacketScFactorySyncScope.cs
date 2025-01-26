using EndFieldPS.Network;
using EndFieldPS.Protocol;

namespace EndFieldPS.Packets.Sc
{
    public class PacketScFactorySyncScope : Packet
    {

        public PacketScFactorySyncScope(Player client) {
            SetData(ScMessageId.ScFactorySyncScope, new ScFactorySyncScope
            {
                ScopeName = 1,
                CurrentChapterId = "domain_1",
                Quickbars =
                {
                    new ScdFactorySyncQuickbar
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
                TransportRoute =
                {
                    UpdateTs = DateTime.UtcNow.Ticks,
                    Routes =
                    {
                        new ScdFactoryHubTransportRoute
                        {
                            ChapterId = "domain_1",
                            Index = 1
                        }
                    }
                },
                BookMark = {}
            });
        }

    }
}
