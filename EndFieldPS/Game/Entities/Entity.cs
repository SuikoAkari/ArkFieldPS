using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static EndFieldPS.Resource.ResourceManager;

namespace EndFieldPS.Game.Entities
{
    public class Entity
    {
        public ulong guid;
        public int level;
        public ulong worldOwner;
        public double curHp;

        public Vector3f Position=new();
        public Vector3f Rotation = new();
        public Entity()
        {

        }
        public Entity(ulong guid, int level, ulong worldOwner)
        {
            this.guid = guid;
            this.level = level;
            this.worldOwner = worldOwner;

        }
        public virtual void Damage(double dmg)
        {

        }

        public virtual void Heal(double heal)
        {

        }

        public Player GetOwner()
        {
            return Server.clients.Find(c => c.roleId == worldOwner);
        }
    }
}
