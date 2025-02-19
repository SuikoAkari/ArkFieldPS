using ArkFieldPS.Protocol;
using ArkFieldPS.Resource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ArkFieldPS.Resource.ResourceManager;
using static ArkFieldPS.Resource.ResourceManager.LevelScene.LevelData;

namespace ArkFieldPS.Game.Entities
{
    public class EntityInteractive : Entity
    {
        public string templateId;
        public Dictionary<InteractiveComponentType, List<ParamKeyValue>> componentProperties = new();
        public EntityInteractive()
        {

        }
        public EntityInteractive(string templateId, ulong worldOwner, Vector3f pos, Vector3f rot,ulong g=0)
        {
            if (g == 0)
            {
                this.guid = (ulong)new Random().NextInt64();
            }
            else
            {
                this.guid = g;
            }
            this.level = 1;
            this.worldOwner = worldOwner;
            this.Position = pos;
            this.Rotation = rot;
            this.templateId = templateId;
        }
        
        
        public SceneInteractive ToProto()
        {
            
            SceneInteractive proto = new SceneInteractive()
            {
                CommonInfo = new()
                {
                    Hp = 100,
                    
                    Id = guid,
                    Templateid = templateId,
                    BelongLevelScriptId = belongLevelScriptId,
                    
                    SceneNumId = GetOwner().curSceneNumId,
                    Position = Position.ToProto(),
                    Rotation = Rotation.ToProto(),
                    
                    Type = (int)type,
                },

                //Meta =dependencyGroupId,
                BattleInfo = new()
                {
                    
                },
                Properties =
                {
                    
                }
                
            };
            int i = 1;
            
                foreach (var prop in properties)
                {
                    DynamicParameter p = prop.ToProto();
                    if (p != null)
                    {
                        proto.Properties.Add(i, p);
                        i++;
                    }

                }
                foreach (var comp in componentProperties)
                {
                    foreach (var prop in comp.Value)
                    {
                        DynamicParameter p = prop.ToProto();
                        if (p != null)
                        {
                            proto.Properties.Add(i, p);
                            i++;
                        }
                    }
                }
            
            
            return proto;
        }

        public override void Damage(double dmg)
        {
            
        }

        public override void Heal(double heal)
        {
            
        }

    }
}
