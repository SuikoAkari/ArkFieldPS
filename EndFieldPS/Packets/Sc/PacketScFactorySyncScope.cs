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
    public class PacketScFactorySyncScope : Packet
    {

        public PacketScFactorySyncScope(Player client) {
            
            //TODO dehardcode current chapter, quickbars, routes, bookmark etc
            ScFactorySyncScope proto = new ScFactorySyncScope()
            {
                BookMark = new()
                {

                },
                ScopeName = 1,
                Quickbars =
                {
                    new ScdFactorySyncQuickbar()
                    {
                        Type=0,
                        List =
                        {
                            "","","","","","","",""
                        }
                    },
                    new ScdFactorySyncQuickbar()
                    {
                        Type=1,
                        List =
                        {
                            "","","","","","","",""
                        }
                    }
                },

                TransportRoute = new()
                {
                    UpdateTs = DateTime.UtcNow.Ticks + 100000000,
                    Routes =
                    {
                        
                    },

                },
                
                CurrentChapterId = "domain_1",
            };
            
            SetData(ScMessageId.ScFactorySyncScope, proto);
        }

    }
}
