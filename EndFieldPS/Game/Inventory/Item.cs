using EndFieldPS.Resource;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization.IdGenerators;
using static EndFieldPS.Resource.ResourceManager;
using Google.Protobuf.Collections;
using EndFieldPS.Packets.Sc;
using EndFieldPS.Protocol;

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
            this.level = GetDefaultLevel();
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
        public ulong GetDefaultLevel()
        {
            switch (ItemType)
            {
                case ItemValuableDepotType.Weapon:
                    return 1;
                case ItemValuableDepotType.Equip:
                    return equipTable[id].minWearLv;
                default:
                    return 0;
            }
        }
        public List<AttributeModifier> GetEquipAttributeModifier()
        {
            return ResourceManager.equipTable[id].attrModifiers;
        }
        public ItemValuableDepotType ItemType
        {
            get{
                return ResourceManager.GetItemTable(id).valuableTabType;
            }
        }
        public virtual ScdItemGrid ToProto()
        {
            try
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
                                    AttachGemId = attachGemId,
                                    BreakthroughLv = breakthroughLv,
                                    RefineLv = refineLv
                                },
                                IsLock = locked
                            }
                        };
                    case ItemValuableDepotType.Equip:
                        return new ScdItemGrid()
                        {
                            Count = 1,
                            Id = id,

                            Inst = new()
                            {
                                InstId = guid,
                                
                                Equip = new()
                                {
                                    
                                    EquipCharId = GetOwner().chars.Find(c => c.IsEquipped(guid)) != null ? GetOwner().chars.Find(c => c.IsEquipped(guid)).guid : 0,
                                    Equipid=guid,
                                    Templateid=ResourceManager.GetItemTemplateId(id),
                                    
                                },
                                IsLock = locked
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
            catch(Exception e)
            {
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
        public (ulong, ulong, ulong) CalculateLevelAndGoldCost(ulong addedXp)
        {
            ulong gold = 0;
            ulong curLevel = this.level;
            WeaponBasicTable table = ResourceManager.weaponBasicTable[id];
            WeaponUpgradeTemplateTable upgradeTable = ResourceManager.weaponUpgradeTemplateTable[table.levelTemplateId];
            while (addedXp >= upgradeTable.list.Find(c=>c.weaponLv==curLevel).lvUpExp)
            {
                gold += upgradeTable.list.Find(c => c.weaponLv == curLevel).lvUpGold;
                addedXp -= upgradeTable.list.Find(c => c.weaponLv == curLevel).lvUpExp;
                curLevel++;
                if (curLevel >= 80)
                {
                    curLevel = 80;
                }
            }
            return (curLevel, gold, addedXp);
        }
        public ulong GetMaterialExp(string id)
        {
            switch (id)
            {
                case "item_weapon_expcard_low":
                    return 200;
                case "item_weapon_expcard_mid":
                    return 1000;
                case "item_weapon_expcard_high":
                    return 10000;
                default:
                    return 0;
            }
        }
        public void LevelUp(MapField<string, ulong> costItemId2Count, RepeatedField<ulong> costWeaponIds)
        {
            //TODO add exp from costWeapons
            ulong addedXp = 0;
            foreach (var material in costItemId2Count)
            {
                addedXp += GetMaterialExp(material.Key) * material.Value;
            }
            (ulong, ulong, ulong) CalculatedValues = CalculateLevelAndGoldCost(xp + addedXp);

            costItemId2Count.Add("item_gold",CalculatedValues.Item2);
            if (GetOwner().inventoryManager.ConsumeItems(costItemId2Count))
            {
                this.level = CalculatedValues.Item1;
                this.xp = CalculatedValues.Item3;
                ScWeaponAddExp levelUp = new()
                {
                    Weaponid = guid,
                    WeaponLv=level,
                    NewExp=xp,

                };
                GetOwner().Send(ScMessageId.ScWeaponAddExp, levelUp);
                GetOwner().Send(new PacketScSyncWallet(GetOwner()));
            }
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
