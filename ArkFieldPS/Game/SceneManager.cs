using ArkFieldPS.Game.Entities;
using ArkFieldPS.Game.Inventory;
using ArkFieldPS.Packets.Sc;
using ArkFieldPS.Resource;
using MongoDB.Bson.Serialization.Attributes;
using SharpCompress.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using static ArkFieldPS.Resource.ResourceManager;
using static ArkFieldPS.Resource.ResourceManager.LevelScene.LevelData;

namespace ArkFieldPS.Game
{
    public class SceneManager
    {
        public List<Scene> scenes = new List<Scene>();
        public Player player;
        public SceneManager(Player player) {
        
            this.player = player;   
            
        }
        public void UnloadCurrent(bool v)
        {
            Scene scene = scenes.Find(s => s.sceneNumId == player.curSceneNumId);

            if (scene != null)
            {
                if (v)
                {
                    List<ulong> g = new();
                    foreach (Entity e in scene.entities) {
                        g.Add(e.guid);
                    }
                    PacketScObjectLeaveView leave = new(player, g);
                    player.Send(leave);
                }
                player.random.usedGuids.Clear();
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
            Scene scene = GetCurScene();

            if (scene != null)
            {
                scene.entities.RemoveAll(e => e is EntityCharacter);
                foreach (Character.Character chara in player.GetCurTeam())
                {
                    EntityCharacter ch = new(chara.guid, player.roleId);
                    scene.entities.Add(ch);
                }
            }
            
            

        }
        public void LoadCurrent()
        {
            Scene scene = GetCurScene();

            if (scene != null)
            {
                scene.Load();
            }

        }
        public Scene GetCurScene()
        {
            return scenes.Find(s => s.sceneNumId == player.curSceneNumId);
        }
        public void SpawnEntity(Entity entity)
        {

            Scene scene = GetCurScene();

            if (scene != null)
            {
                scene.entities.Add(entity);
                //Spawn packet
                player.Send(new PacketScObjectEnterView(player,new List<Entity>{ entity }));
            }
        }
        public void KillEntity(ulong guid, bool killClient=false, int reason=1)
        {
            Scene scene = GetCurScene();

            if (scene != null)
            {
                if(GetEntity(guid) is EntityMonster)
                {
                    EntityMonster monster = (EntityMonster)GetEntity(guid);
                    CreateDrop(monster.Position, new RewardTable.ItemBundle()
                    {
                        id = "item_gem_rarity_3",
                        count=1
                    });
                    LevelScene lv_scene = ResourceManager.GetLevelData(GetCurScene().sceneNumId);
                    LevelEnemyData d = lv_scene.levelData.enemies.Find(l => l.levelLogicId == monster.guid);
                    if (d != null)
                    {
                        if (!d.respawnable)
                        {
                            player.noSpawnAnymore.Add(monster.guid);
                        }
                    }
                }
                scene.entities.Remove(GetEntity(guid));
                if (killClient)
                {
                    ScSceneDestroyEntity destroy = new()
                    {
                        Id = guid,
                        Reason = reason,
                        SceneNumId = player.curSceneNumId,
                    };
                    player.Send(Protocol.ScMessageId.ScSceneDestroyEntity, destroy);
                }
                //Leave packet disabled for now
                //player.Send(new PacketScObjectLeaveView(player, guid));
            }
        }
        public void CreateDrop(Vector3f pos,ResourceManager.RewardTable.ItemBundle bundle)
        {
            ItemTable info = ResourceManager.itemTable[bundle.id];
            Item item = new Item(player.roleId, info.id, bundle.count);
            EntityInteractive drop = new(info.modelKey, player.roleId, pos, new Vector3f())
            {
                type = (ObjectType)5,
                curHp = 100,
                properties =
                {
                    new ParamKeyValue()
                    {
                        key="nothing",
                        value = new()
                        {
                            type=ParamRealType.String,
                            valueArray=new ParamKeyValue.ParamValueAtom[1]
                            {
                                new ParamKeyValue.ParamValueAtom()
                                {

                                    valueString=info.id,
                                }
                            }
                        }
                    },
                    new ParamKeyValue()
                    {
                        key="item_id",
                        value = new()
                        {
                            type=ParamRealType.String,
                            valueArray=new ParamKeyValue.ParamValueAtom[1]
                            {
                                new ParamKeyValue.ParamValueAtom()
                                {
                                    
                                    valueString=info.id,
                                }
                            }
                        }
                    },

                }
                
            };
           
            
            drop.properties.Add(new ParamKeyValue()
            {
                key = "item_count",
                value = new()
                {
                    type = ParamRealType.Int,
                    
                    valueArray = new ParamKeyValue.ParamValueAtom[1]
                    {
                        new ParamKeyValue.ParamValueAtom()
                        {
                            valueBit64=bundle.count
                        }
                    }
                }
            });
            if (item.InstanceType())
            {
                drop.properties.Add(new ParamKeyValue()
                {
                    key = "item_instance",
                    value = new()
                    {
                        type = ParamRealType.String,
                        valueArray = new ParamKeyValue.ParamValueAtom[1]
                        {
                            new ParamKeyValue.ParamValueAtom()
                            {
                                valueString=Newtonsoft.Json.JsonConvert.SerializeObject(item.ToProto().Inst)
                            }
                        }
                    }
                });
            }
            SpawnEntity(drop);
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
                if(scenes.Find(s=>s.sceneNumId==level.idNum) == null)
                scenes.Add(new Scene()
                {
                    guid = (ulong)player.random.Next(),
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
        public Dictionary<string, int> collections = new();
        [BsonIgnore,JsonIgnore]
        public List<Entity> entities = new();
        [BsonIgnore, JsonIgnore]
        public bool alreadyLoaded = false;
        public int GetCollection(string id)
        {
            if (collections.ContainsKey(id))
            {
                return collections[id];
            }
            return 0; 
        }

        public void AddCollection(string id,int amt)
        {
            if (collections.ContainsKey(id))
            {
                collections[id] += amt;
            }
            else
            {
                collections.Add(id, amt);
            }
        }
        public List<Entity> GetEntityExcludingChar()
        {
            return entities.FindAll(c => c is not EntityCharacter);
        }
        public void Unload()
        {
            entities.Clear();
        }
        
        public void Load()
        {
            
            if (sceneNumId == 98)
            {
                //Load spaceship manager chars
                //Disabled as seem the client already spawn npcs automatically
                /*foreach (var chara in GetOwner().spaceshipManager.chars)
                {
                    EntityNpc npc = new EntityNpc(chara.GetNpcId(), chara.owner,chara.position,chara.rotation,chara.guid);
                    entities.Add(npc);
                }*/
            }
            LevelScene lv_scene = ResourceManager.GetLevelData(sceneNumId);
            GetOwner().random.NextRand();
            lv_scene.levelData.interactives.ForEach(en =>
            {
                if (en.defaultHide || GetOwner().noSpawnAnymore.Contains(en.levelLogicId))
                {
                    return;
                }
                EntityInteractive entity = new(en.entityDataIdKey, ownerId, en.position, en.rotation, en.levelLogicId)
                {
                    belongLevelScriptId=en.belongLevelScriptId,
                    dependencyGroupId=en.dependencyGroupId,
                    levelLogicId= en.levelLogicId,
                    type = en.entityType,
                    properties= en.properties,
                    componentProperties=en.componentProperties,
                };
                entities.Add(entity);
            });
            lv_scene.levelData.enemies.ForEach(en =>
            {
                if(en.defaultHide || GetOwner().noSpawnAnymore.Contains(en.levelLogicId)) return;
                EntityMonster entity = new(en.entityDataIdKey,en.level,ownerId,en.position,en.rotation, en.levelLogicId)
                {
                    type=en.entityType,
                    belongLevelScriptId=en.belongLevelScriptId,
                    levelLogicId = en.levelLogicId
                };
                entities.Add(entity);
            });
            lv_scene.levelData.npcs.ForEach(en =>
            {
                if (en.defaultHide) return;
                if (en.npcGroupId.Contains("chr")) return;
                EntityNpc entity = new(en.entityDataIdKey,ownerId,en.position,en.rotation,en.levelLogicId)
                {
                    belongLevelScriptId = en.belongLevelScriptId,
                    levelLogicId = en.levelLogicId,
                    type = en.entityType,
                    
                };
                entities.Add(entity);
            });
            GetEntityExcludingChar().ForEach(e =>
            {
                GetOwner().Send(new PacketScObjectEnterView(GetOwner(),new List<Entity>() { e}));
            });
            
           
        }
       
        public Player GetOwner()
        {
            return Server.clients.Find(c => c.roleId == ownerId);
        }
    }
}
