using EndFieldPS.Database;
using EndFieldPS.Resource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace EndFieldPS.Game.Inventory
{
    public class InventoryManager
    {
        public Player owner;
        public List<Item> items= new List<Item>();

        public int item_diamond_amt
        {
            get
            {
                if (items.Find(i => i.id == "item_diamond") == null) return 0;
                return items.Find(i => i.id == "item_diamond")!.amount;
            }
        }
        public int item_gold_amt
        {
            get
            {
                if (items.Find(i => i.id == "item_gold") == null) return 0;
                return items.Find(i => i.id == "item_gold")!.amount;
            }
        }
        public InventoryManager(Player o) {

            owner = o;
        
        }
        public Item AddWeapon(string id, ulong level)
        {
            Item item = new Item(owner.roleId, id, level);
            items.Add(item);
            return item;
        }
        public void Save()
        {
            foreach (Item item in items)
            {
                DatabaseManager.db.UpsertItemAsync(item);
            }
        }
        public void Load()
        {
           items = DatabaseManager.db.LoadInventoryItems(owner.roleId);
        }
        public Item AddItem(string id, int amt)
        {
            if((int)ResourceManager.itemTable[id].valuableTabType > 5)
            {
                Item item = items.Find(i=>i.id == id);
                if (item != null)
                {
                    item.amount += amt;
                    return item;
                }
                else
                {
                    item = new Item(owner.roleId, id, amt);
                    items.Add(item);
                    return item;
                }
            }
            else
            {
                Item item = new Item(owner.roleId, id, amt);
                items.Add(item);
                return item;
            }

            
        }
    }
}
