using ArkFieldPS.Resource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ArkFieldPS.Game.Factory.FactoryNode;

namespace ArkFieldPS.Game.Factory.Components
{
    public class FComponentPortManager : FComponent
    {
        public class FPort
        {
            public int index = 0;
            public uint ownerComId;
            public uint touchComId;

            public ScdFacComSubPort ToProto()
            {
                return new ScdFacComSubPort()
                {
                    InBlock = false,
                    Index = index,
                    OwnerComId = ownerComId,
                    TouchComId = touchComId
                };
            }
        }
        public List<FPort> ports = new();
        public FComponentPortManager(uint id, uint mainId) : base(id, FCComponentType.PortManager)
        {
            for(int i=0; i < 14; i++)
            {
                ports.Add(new FPort()
                {
                    index = i,
                    ownerComId = mainId,
                    touchComId = 0
                });
            }
        }

        public override void SetComponentInfo(ScdFacCom proto)
        {
            proto.PortManager = new();
            foreach(FPort port in ports)
            {
                proto.PortManager.Ports.Add(port.ToProto());
            }
        }
    }
}
