using EndFieldPS.Resource;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization.IdGenerators;

namespace EndFieldPS.Game.Inventory
{
    public class Item
    {
        [BsonId(IdGenerator = typeof(ObjectIdGenerator))]
        public ObjectId _id { get; set; }
        [BsonElement("templateId")]
        public string id;
        public ulong guid;
        public int amount = 1;
        public ulong owner;
        public ulong level = 1;
        public ulong xp;
        public bool locked = false;
        public ulong attachGemId;
        public ulong breakthroughLv;
        public ulong refineLv;
        public Item() { 
        
        }
        public Item(ulong owner, string id, int amt)
        {
            this.owner = owner;
            this.id = id;
            this.amount = amt;
            guid = GetOwner().random.Next();
        }
        public Item(ulong owner, string id, ulong level)
        {
            this.owner = owner;
            this.id = id;
            this.amount = 1;
            this.level = level;
            guid = GetOwner().random.Next();
        }
        public ItemValuableDepotType ItemType
        {
            get{
                return ResourceManager.GetItemTable(id).valuableTabType;
            }
        }
        public virtual ScdItemGrid ToProto()
        {
            switch (ItemType)
            {
                case ItemValuableDepotType.Weapon:
                    return new ScdItemGrid()
                    {
                        Count = 1,
                        Id = id,

                        Inst = new()
                        {
                            InstId = guid,
                            Weapon = new()
                            {
                                InstId = guid,
                                EquipCharId = GetOwner().chars.Find(c => c.weaponGuid == guid) != null ? GetOwner().chars.Find(c => c.weaponGuid == guid).guid : 0,
                                WeaponLv = level,
                                TemplateId = ResourceManager.GetItemTemplateId(id),
                                Exp = xp,
                                AttachGemId= attachGemId,
                                BreakthroughLv= breakthroughLv,
                                RefineLv=refineLv
                            },
                            IsLock=locked
                        }
                    };
                default:
                    return new ScdItemGrid()
                    {
                        Count = amount,
                        Id = id,
                    };
            }
        }
        public Player GetOwner()
        {
            return Server.clients.Find(c => c.roleId == this.owner);
        }

        public bool InstanceType()
        {
            switch (ItemType)
            {
                case ItemValuableDepotType.Weapon:
                    return true;
                case ItemValuableDepotType.WeaponGem:
                    return false;
                case ItemValuableDepotType.Equip:
                    return true;
                case ItemValuableDepotType.SpecialItem:
                    return false;
                case ItemValuableDepotType.MissionItem:
                    return true;
                default:
                    return false;
            }
        }
    }

}
