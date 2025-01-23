using EndFieldPS.Game.Character;
using EndFieldPS.Game.Inventory;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static EndFieldPS.Player;
using static EndFieldPS.Resource.ResourceManager;
using static SQLite.SQLite3;

namespace EndFieldPS.Database
{
    public class PlayerData
    {
        public ulong roleId;
        [BsonId]
        public string token;
        public Vector3f position;
        public Vector3f rotation;
        public int curSceneNumId;
        public uint level = 20;
        public uint xp = 0;
        public string nickname = "Endministrator";
        public List<Team> teams = new List<Team>();
        public ulong totalGuidCount = 1;
    }
    public class Database
    {
        private readonly IMongoDatabase _database;

        public Database(string connectionString, string dbName)
        {
            var client = new MongoClient(connectionString);
            _database = client.GetDatabase(dbName);
        }

        public List<Character> LoadCharacters(ulong roleId)
        {
            return _database.GetCollection<Character>("avatars").Find(c=>c.owner== roleId).ToList();
        }
        public List<Item> LoadInventoryItems(ulong roleId)
        {
            return _database.GetCollection<Item>("items").Find(c => c.owner == roleId).ToList();
        }
        public void SavePlayerData(Player player)
        {
            PlayerData data = new()
            {
                token = player.token,
                curSceneNumId = player.curSceneNumId,
                level = player.level,
                nickname = player.nickname,
                position = player.position,
                rotation = player.rotation,
                roleId=player.roleId,
                teams=player.teams,
                xp=player.xp,
                totalGuidCount=player.random.v
            };
            UpsertPlayerDataAsync(data);
        }
        public async Task UpsertPlayerDataAsync(PlayerData player)
        {
            var collection = _database.GetCollection<PlayerData>("players");

            var filter = 
                Builders<PlayerData>.Filter.Eq(p => p.roleId,player.roleId)
                &
                Builders<PlayerData>.Filter.Eq(p => p.token, player.token);

            await collection.ReplaceOneAsync(
                filter,
                player,
                new ReplaceOptions { IsUpsert = true }
            );
        }
        public async Task UpsertCharacterAsync(Character character)
        {
            if (character._id == ObjectId.Empty)
            {
                character._id = ObjectId.GenerateNewId();
            }
            var collection = _database.GetCollection<Character>("avatars");

            var filter = 
                Builders<Character>.Filter.Eq(c => c.guid, character.guid)
                &
                Builders<Character>.Filter.Eq(c => c.owner, character.owner);

            var result=await collection.ReplaceOneAsync(
                filter,
                character,
                new ReplaceOptions { IsUpsert = true }
            );
        }
        public async Task UpsertItemAsync(Item item)
        {
            if (item._id == ObjectId.Empty)
            {
                item._id = ObjectId.GenerateNewId();
            }
            var collection = _database.GetCollection<Item>("items");

            var filter =
                Builders<Item>.Filter.Eq(c => c.guid, item.guid)
                &
                Builders<Item>.Filter.Eq(c => c.owner, item.owner);

            var result = await collection.ReplaceOneAsync(
                filter,
                item,
                new ReplaceOptions { IsUpsert = true }
            );

        }
        public PlayerData GetPlayerByToken(string token)
        {
            try
            {
                return _database.GetCollection<PlayerData>("players").Find(p => p.token == token).ToList().FirstOrDefault();
            }
            catch(Exception e)
            {
                Logger.PrintError("Error occured while loading Account for token: " + token+" ERROR:\n"+e.Message);
                return null;
            }
            
        }
    }
}
