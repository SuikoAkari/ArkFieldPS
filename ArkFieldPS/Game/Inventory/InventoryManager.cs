using ArkFieldPS.Database;
using ArkFieldPS.Packets.Sc;
using ArkFieldPS.Resource;
using Google.Protobuf.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static ArkFieldPS.Resource.ResourceManager;

namespace ArkFieldPS.Game.Inventory
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

        public Item GetItemById(string id)
        {
            return items.Find(i => i.id == id);
        }
        public InventoryManager(Player o) {

            owner = o;
        
        }
        public void AddRewards(string rewardTemplateId, Vector3f pos, int sourceType=1)
        {
            try
            {
                ScRewardToastBegin begin = new ScRewardToastBegin()
                {
                    RewardSourceType = sourceType,
                    RewardToastInstId = owner.random.NextRand(),

                };
                ScRewardToSceneBegin begin2 = new ScRewardToSceneBegin()
                {
                    RewardSourceType = sourceType,
                    SourceTemplateId = rewardTemplateId,
                };
                ScRewardToastEnd end = new()
                {
                    RewardToastInstId = begin.RewardToastInstId,

                };
                List<RewardTable.ItemBundle> bundles = rewardTable[rewardTemplateId].itemBundles;
                foreach(RewardTable.ItemBundle bundle in bundles)
                {
                    Item item = new Item()
                    {
                        id=bundle.id
                    };
                    
                    if (!item.InstanceType())
                    {
                        item = AddItem(bundle.id, bundle.count);
                        end.RewardVirtualList.Add(new RewardItem()
                        {
                            Count = bundle.count,
                            Id = bundle.id,
                            Inst=item.ToProto().Inst,
                        });

                    }
                    else
                    {
                        owner.sceneManager.CreateDrop(pos, bundle);
                        //TODO drops
                    }
                }
                owner.Send(Protocol.ScMessageId.ScRewardToastBegin, begin);
                owner.Send(Protocol.ScMessageId.ScRewardToSceneBegin, begin2);
                

                owner.Send(Protocol.ScMessageId.ScRewardToastEnd, end);
                owner.Send(Protocol.ScMessageId.ScRewardToSceneEnd, new ScRewardToSceneEnd());
                owner.Send(new PacketScSyncWallet(owner));
            }
            catch (Exception e)
            {
                Logger.PrintError(e.Message);
            }
            
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
                DatabaseManager.db.UpsertItem(item);
            }
        }
        public void Load()
        {
           items = DatabaseManager.db.LoadInventoryItems(owner.roleId);
        }
        public Item AddItem(string id, int amt)
        {
            Item it = new()
            {
                id = id,
            };
            if(!it.InstanceType())
            {
                
                Item item = items.Find(i=>i.id == id);
                if (item != null)
                {
                   // Logger.Print(id + ": " + amt+" added to existing");
                    item.amount += amt;
                    return item;
                }
                else
                {
                   // Logger.Print(id + ": " + amt + " added to new");
                    item = new Item(owner.roleId, id, amt);
                    items.Add(item);
                    return item;
                }
            }
            else
            {
                //Logger.Print(id + ": " + amt + " added to new as instance");
                Item item = new Item(owner.roleId, id, amt);
                items.Add(item);
                return item;
            } 
        }
        public void RemoveItem(Item item,int amt)
        {
            item.amount -= amt;
            if(item.amount <= 0)
            {
                items.Remove(item);
                DatabaseManager.db.DeleteItem(item);
            }
            this.owner.Send(new PacketScItemBagScopeModify(this.owner, item));
        }
        public bool ConsumeItems(MapField<string, ulong> costItemId2Count)
        {
            RepeatedField<ItemInfo> items = new RepeatedField<ItemInfo>();
            foreach (var item in costItemId2Count)
            {
                items.Add(new ItemInfo()
                {
                    ResCount=(int)item.Value,
                    ResId=item.Key,
                });
            }
            return ConsumeItems(items);
        }
        public bool ConsumeItems(RepeatedField<ItemInfo> items)
        {
            bool found = true;
            foreach (ItemInfo item in items)
            {
                Item i= GetItemById(item.ResId);
                if (i != null)
                {
                    if(i.amount < item.ResCount)
                    {
                        found = false;
                        break;
                    }
                }
                else
                {
                    found = false;
                    break;
                }
            }
            foreach (ItemInfo item in items)
            {
                Item i = GetItemById(item.ResId);
                if (i != null)
                {
                    if (i.amount >= item.ResCount)
                    {
                       RemoveItem(i,item.ResCount);
                    }
                }
            }
            return found;
        }

        public Dictionary<uint, int> GetInventoryChapter(string chapterId)
        {
            Dictionary<uint, int> dir= new Dictionary<uint, int>();
            List<Item> citems = items.FindAll(i=>!i.InstanceType());
            foreach (Item item in citems)
            {
                dir.Add((uint)ResourceManager.strIdNumTable.item_id.dic[item.id], item.amount);
            }

            return dir;
        }
    }
}
