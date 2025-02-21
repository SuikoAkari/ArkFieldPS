using ArkFieldPS.Resource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ArkFieldPS.Game.Factory.FactoryNode;

namespace ArkFieldPS.Game.Factory.Components
{
    public class FComponentBusLoader : FComponent
    {
        public string lastPutinItemId = "";
        public FComponentBusLoader(uint id) : base(id, FCComponentType.BusLoader)
        {
        }

        public override void SetComponentInfo(ScdFacCom proto)
        {
            proto.BusLoader = new()
            {
                LastPutinItemId= lastPutinItemId
            };
        }
    }
}
