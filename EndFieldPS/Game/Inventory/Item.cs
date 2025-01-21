using EndFieldPS.Resource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EndFieldPS.Game.Inventory
{
    public class Weapon
    {
        public string id;
        public ulong guid;
        public ulong level;
        public ulong xp;
        public ulong owner;
        public Weapon()
        {

        }
        public Weapon(ulong owner, string id, ulong level)
        {
            this.owner = owner;
            this.id = id;
            this.level = level;
            guid = GetOwner().random.Next();
        }


        public ScdItemGrid ToProto()
        {
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
                        EquipCharId = GetOwner().chars.Find(c=>c.weaponGuid==guid) !=null? GetOwner().chars.Find(c => c.weaponGuid == guid).guid:0,
                        WeaponLv = level,
                        TemplateId = ResourceManager.GetItemTemplateId(id),
                        Exp = xp,

                    },

                }
            };
        }
        public EndminPlayer GetOwner()
        {
            return Server.clients.Find(c => c.roleId == this.owner);
        }
    }
}
