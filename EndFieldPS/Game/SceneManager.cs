using EndFieldPS.Game.Entities;
using EndFieldPS.Resource;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EndFieldPS.Game
{
    public class SceneManager
    {
        public List<Scene> scenes = new List<Scene>();
        public Player player;
        public SceneManager(Player player) {
        
            this.player = player;   
            
        }
        public void UnloadCurrent()
        {
            Scene scene = scenes.Find(s => s.sceneNumId == player.curSceneNumId);

            if (scene != null)
            {
                scene.Unload();
            }

        }
        public Entity GetEntity(ulong guid)
        {
            Scene scene = scenes.Find(s => s.sceneNumId == player.curSceneNumId);

            if (scene != null)
            {
                return scene.entities.Find(e => e.guid == guid);
            }

            return null;
        }
        public void LoadCurrentTeamEntities()
        {
            Scene scene = scenes.Find(s => s.sceneNumId == player.curSceneNumId);

            if (scene != null)
            {
                scene.Unload();
            }
            
            scene.entities.RemoveAll(e => e is EntityCharacter);
            foreach(Character.Character chara in player.GetCurTeam())
            {
                EntityCharacter ch = new(chara.guid, player.roleId);
                scene.entities.Add(ch);
            }
        }
        public void LoadCurrent()
        {
            Scene scene = scenes.Find(s => s.sceneNumId == player.curSceneNumId);

            if (scene != null)
            {
                scene.Load();
            }

        }
        public ulong GetSceneGuid(int sceneNumId)
        {
            return scenes.Find(s=>s.sceneNumId == sceneNumId).guid;
        }
        //TODO Save and get
        public void Load()
        {
            foreach (var level in ResourceManager.levelDatas)
            {
                scenes.Add(new Scene()
                {
                    guid = (ulong)new Random().NextInt64(1000000000000),
                    ownerId=player.roleId,
                    sceneNumId=level.idNum,

                });
            }
        }
    }

    public class Scene
    {
        public ulong ownerId;
        public ulong guid;
        public int sceneNumId;
        [BsonIgnore]
        public List<Entity> entities = new();


        public void Unload()
        {
            entities.Clear();
        }

        public void Load()
        {
            //Load actual scene entities
        }

        public Player GetOwner()
        {
            return Server.clients.Find(c => c.roleId == guid);
        }
    }
}
