using EndFieldPS.Game.Character;
using EndFieldPS.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static EndFieldPS.Resource.ResourceManager;

namespace EndFieldPS.Game.Entities
{
    public class EntityCharacter : Entity
    {

        public new double curHp
        {
            get
            {
                return GetChar().curHp;
            }
        }
        public new Vector3f Position
        {
            get
            {
                return GetOwner().position;
            }
            set
            {
                GetOwner().position = value;
            }
        }
        public new Vector3f Rotation
        {
            get
            {
                return GetOwner().rotation;
            }
            set
            {
                GetOwner().rotation = value;
            }
        }
        public EntityCharacter(ulong guid, ulong worldOwner)
        {
            this.guid = guid;
            this.worldOwner = worldOwner;

        }
        public override void Damage(double dmg)
        {
            GetChar().curHp -= dmg;

            ScCharSyncStatus state = new()
            {
                IsDead = GetChar().curHp < 1,
                Objid = guid,
                
                BattleInfo = new()
                {
                    Hp=curHp,
                    
                    Ultimatesp=GetChar().ultimateSp,
                },
                
            };
            ScEntityPropertyChange prop = new()
            {
                InstId=guid,
                Info = new()
                {
                    Hp=curHp,
                    Ultimatesp= GetChar().ultimateSp,
                    
                }
            };
            GetOwner().Send(ScMessageId.ScCharSyncStatus, state);
            GetOwner().Send(ScMessageId.ScEntityPropertyChange, prop);
        }

        public override void Heal(double heal)
        {
            GetChar().curHp += heal;

            ScCharSyncStatus state = new()
            {
                IsDead = GetChar().curHp < 1,
                Objid = guid,
                BattleInfo = new()
                {
                    Hp = curHp,
                    Ultimatesp = GetChar().ultimateSp,
                }
            };
            ScEntityPropertyChange prop = new()
            {
                InstId = guid,
                Info = new()
                {
                    Hp = curHp,
                    Ultimatesp = GetChar().ultimateSp,
                }
            };
            GetOwner().Send(ScMessageId.ScCharSyncStatus, state);
            GetOwner().Send(ScMessageId.ScEntityPropertyChange, prop);
        }
        public Character.Character GetChar()
        {
            return GetOwner().chars.Find(c => c.guid == guid);
        }
    }
}
