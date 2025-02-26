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
    public class EntityNpc : Entity
    {
        public string templateId;
        public EntityNpc()
        {

        }
        public EntityNpc(string templateId, ulong worldOwner, Vector3f pos, Vector3f rot,int scene,ulong guid)
        {
            this.guid = (ulong)guid;
            this.level = 1;
            this.worldOwner = worldOwner;
            this.Position = pos;
            this.Rotation = rot;
            this.BornPos = pos;
            this.BornRot = rot;
            this.templateId = templateId;
            this.sceneNumId = scene;
        }
        
        
        public SceneNpc ToProto()
        {
            SceneNpc proto = new SceneNpc()
            {
                CommonInfo = new()
                {
                    Hp = 100,
                    Id = guid,
                    Templateid = templateId,
                    BelongLevelScriptId=belongLevelScriptId,
                    
                    SceneNumId =sceneNumId,
                    Position = Position.ToProto(),
                    Rotation = Rotation.ToProto(),

                    Type = (int)type, 
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
