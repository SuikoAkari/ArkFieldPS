using ArkFieldPS.Protocol;
using ArkFieldPS.Resource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ArkFieldPS.Resource.ResourceManager;

namespace ArkFieldPS.Game.Entities
{
    public class EntityMonster : Entity
    {
        public string templateId;
        public EntityMonster()
        {

        }
        public EntityMonster(string templateId, int level, ulong worldOwner, Vector3f pos, Vector3f rot,ulong g=0)
        {
            if (g == 0)
            {
                this.guid = (ulong)new Random().NextInt64();
            }
            else
            {
                this.guid = g;
            }
            
            this.level = level;
            this.worldOwner = worldOwner;
            this.Position = pos;
            this.Rotation = rot;
            this.templateId = templateId;
            this.curHp = GetAttribValue(AttributeType.MaxHp);
        }
        public double GetAttribValue(AttributeType type)
        {
            return GetAttributes().Find(a => a.AttrType == (int)type).Value;
        }
        public List<AttrInfo> GetAttributes()
        {
            List<AttrInfo> attrInfo = new();
            EnemyTable table = ResourceManager.enemyTable[templateId];
            enemyAttributeTemplateTable[table.attrTemplateId].levelDependentAttributes[level].attrs.ForEach(attr =>
            {
                attrInfo.Add(new AttrInfo()
                {
                    AttrType = attr.attrType,
                    BasicValue = attr.attrValue,
                    Value = attr.attrValue

                });

            });
            enemyAttributeTemplateTable[table.attrTemplateId].levelIndependentAttributes.attrs.ForEach(attr =>
            {
                attrInfo.Add(new AttrInfo()
                {
                    AttrType = attr.attrType,
                    BasicValue = attr.attrValue,
                    Value = attr.attrValue

                });

            });

            return attrInfo;
        }
        
        public SceneMonster ToProto()
        {
            SceneMonster proto = new SceneMonster()
            {
                Level = level,
                CommonInfo = new()
                {
                    Hp = curHp,
                    Id = guid,
                    Templateid = templateId,
                    BelongLevelScriptId = belongLevelScriptId,
                    SceneNumId = GetOwner().curSceneNumId,
                    Position = Position.ToProto(),
                    Rotation = Rotation.ToProto(),

                    Type =(int) ObjectTypeIndex.Enemy, 
                },
                Attrs =
                {
                    GetAttributes()
                },
                BattleInfo = new()
                {
                    
                },
                
            };
            return proto;
        }
        public override void Damage(double dmg)
        {
            curHp -= dmg;
            ScEntityPropertyChange prop = new()
            {
                InstId = guid,
                Info = new()
                {
                    Hp = curHp,
                   

                }
            };
            GetOwner().Send(ScMessageId.ScEntityPropertyChange, prop);
        }

        public override void Heal(double heal)
        {
            curHp += heal;
        }

    }
}
