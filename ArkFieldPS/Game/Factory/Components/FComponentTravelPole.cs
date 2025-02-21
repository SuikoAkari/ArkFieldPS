using ArkFieldPS.Resource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ArkFieldPS.Game.Factory.FactoryNode;

namespace ArkFieldPS.Game.Factory.Components
{
    public class FComponentTravelPole : FComponent
    {
        public FComponentTravelPole(uint id) : base(id, FCComponentType.TravelPole)
        {
        }

        public override void SetComponentInfo(ScdFacCom proto)
        {
            proto.TravelPole = new();
        }
    }
}
