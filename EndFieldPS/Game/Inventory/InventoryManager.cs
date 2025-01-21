using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EndFieldPS.Game.Inventory
{
    public class InventoryManager
    {
        public EndminPlayer owner;
        public List<Weapon> weapons= new List<Weapon>();

        public InventoryManager(EndminPlayer o) {

            owner = o;
        
        }
    }
}
