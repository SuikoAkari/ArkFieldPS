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
    public class EntityInteractive : Entity
    {
        public string templateId;
        public EntityInteractive()
        {

        }
        public EntityInteractive(string templateId, ulong worldOwner, Vector3f pos, Vector3f rot)
        {
            this.guid = (ulong)new Random().NextInt64();
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
                    
                    SceneNumId = GetOwner().curSceneNumId,
                    Position = Position.ToProto(),
                    Rotation = Rotation.ToProto(),
                    
                    Type = 32, 
                },
                

            };
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
