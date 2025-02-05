﻿using EndFieldPS.Game.Entities;
using EndFieldPS.Packets.Sc;
using EndFieldPS.Resource;
using MongoDB.Bson.Serialization.Attributes;
using SharpCompress.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
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
        public void KillEntity(ulong guid)
        {
            Scene scene = GetCurScene();

            if (scene != null)
            {
                scene.entities.Remove(GetEntity(guid));
                //Leave packet disabled for now
                //player.Send(new PacketScObjectLeaveView(player, guid));
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
            //Load actual scene entities (Missing full LevelData for that)

            //Send all entities excluding chars that are spawned different
            GetOwner().Send(new PacketScObjectEnterView(GetOwner(), GetEntityExcludingChar()));
        }
       
        public Player GetOwner()
        {
            return Server.clients.Find(c => c.roleId == ownerId);
        }
    }
}
