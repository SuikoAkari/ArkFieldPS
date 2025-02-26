using ArkFieldPS.Resource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ArkFieldPS.Game.Factory.FactoryNode;

namespace ArkFieldPS.Game.Factory.Components
{
    public class FComponentSubHub : FComponent
    {
        public int level = 1;
        public FComponentSubHub(uint id) : base(id, FCComponentType.SubHub)
        {
        }

        public override void SetComponentInfo(ScdFacCom proto)
        {
            proto.SubHub = new()
            {
                Level = level,
            };
        }
    }
}
