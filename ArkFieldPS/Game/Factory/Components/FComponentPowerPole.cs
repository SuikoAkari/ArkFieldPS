using ArkFieldPS.Resource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ArkFieldPS.Game.Factory.FactoryNode;

namespace ArkFieldPS.Game.Factory.Components
{
    public class FComponentPowerPole : FComponent
    {
        public FComponentPowerPole(uint id) : base(id, FCComponentType.PowerPole)
        {
        }

        public override void SetComponentInfo(ScdFacCom proto)
        {
            proto.PowerPole = new();
        }
    }
}
