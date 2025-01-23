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

        public int item_diamond_amt
        {
            get
            {
                return items.Find(i => i.id == "item_diamond")!.amount;
            }
        }
        public int item_gold_amt
        {
            get
            {
                return items.Find(i => i.id == "item_gold")!.amount;
            }
        }
        public InventoryManager(Player o) {

            owner = o;
        
        }
    }
}
