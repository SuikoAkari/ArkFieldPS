using ArkFieldPS.Resource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ArkFieldPS.Game.Factory.FactoryNode;

namespace ArkFieldPS.Game.Factory.Components
{
    public class FComponentSelector : FComponent
    {
        public string selectedItemId = "";
        public FComponentSelector(uint id) : base(id, FCComponentType.Selector)
        {
        }

        public override void SetComponentInfo(ScdFacCom proto)
        {
            proto.Selector = new()
            {
                SelectedItemId= selectedItemId
            };
        }
    }
}
