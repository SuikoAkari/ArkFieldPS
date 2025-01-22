using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EndFieldPS.Game.Inventory
{
    public class InventoryManager
    {
        public Player owner;
        public List<Weapon> weapons= new List<Weapon>();
        public List<Item> items= new List<Item>();
        public InventoryManager(Player o) {

            owner = o;
        
        }
    }
}
