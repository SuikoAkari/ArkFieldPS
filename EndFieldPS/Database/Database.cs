using EndFieldPS.Game.Character;
using EndFieldPS.Game.Inventory;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static EndFieldPS.Player;
using static EndFieldPS.Resource.ResourceManager;
using static SQLite.SQLite3;

namespace EndFieldPS.Database
{
    public class PlayerData
    {
        [BsonId]
        public ulong roleId;
        
        public string accountId;
        public Vector3f position;
        public Vector3f rotation;
        public int curSceneNumId;
        public uint level = 20;
        public uint xp = 0;
        public string nickname = "Endministrator";
        public List<Team> teams = new List<Team>();
        public ulong totalGuidCount = 1;
    }
    public class Account
    {
        public string id;
        public string username;
        public string token;
        public string grantToken;

        public static string GenerateAccountId()
        {
            byte[] bytes = new byte[4];
            RandomNumberGenerator.Fill(bytes);

            // Converte i byte in un intero positivo tra 100000000 e 999999999
            int number = BitConverter.ToInt32(bytes, 0) & int.MaxValue;
            number = 100000000 + (number % 900000000);

            return number.ToString();
        }
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
        public static string GenerateToken(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            Random random = new Random();
            StringBuilder result = new StringBuilder(length);

            for (int i = 0; i < length; i++)
            {
                result.Append(chars[random.Next(chars.Length)]);
            }

            return result.ToString();
        }
        public void SavePlayerData(Player player)
        {
            PlayerData data = new()
            {
                accountId = player.accountId,
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
        public void CreateAccount(string username)
        {
            Account account = new()
            {
                username = username,
                id = Account.GenerateAccountId(),
                token= GenerateToken(22),
                grantToken = GenerateToken(192)
            };
            UpsertAccountAsync(account);
            Logger.Print($"Account with username: {username} created with Account UID: {account.id}");
        }
        public async Task UpsertPlayerDataAsync(PlayerData player)
        {
            var collection = _database.GetCollection<PlayerData>("players");

            var filter = 
                Builders<PlayerData>.Filter.Eq(p => p.roleId,player.roleId)
                &
                Builders<PlayerData>.Filter.Eq(p => p.accountId, player.accountId);

            await collection.ReplaceOneAsync(
                filter,
                player,
                new ReplaceOptions { IsUpsert = true }
            );
        }
        public async Task UpsertAccountAsync(Account player)
        {
            var collection = _database.GetCollection<Account>("accounts");

            var filter =
                Builders<Account>.Filter.Eq(p => p.id, player.id)
                &
                Builders<Account>.Filter.Eq(p => p.token, player.token);

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
        public string GrantCode(Account account)
        {
            account.grantToken = GenerateToken(192);
            UpsertAccountAsync(account);
            return account.grantToken;
        }
        public Account GetAccountByToken(string token)
        {
            try
            {
                return _database.GetCollection<Account>("accounts").Find(p => p.token == token).ToList().FirstOrDefault();
            }
            catch (Exception e)
            {
                Logger.PrintError("No account found with token: " + token);
                return null;
            }
        }
        public Account GetAccountByTokenGrant(string token)
        {
            try
            {
                return _database.GetCollection<Account>("accounts").Find(p => p.grantToken == token).ToList().FirstOrDefault();
            }
            catch (Exception e)
            {
                Logger.PrintError("No account found with grant token: " + token);
                return null;
            }
        }
        public Account GetAccountByUsername(string username)
        {
            try
            {
                return _database.GetCollection<Account>("accounts").Find(p => p.username == username).ToList().FirstOrDefault();
            }
            catch (Exception e)
            {
                Logger.PrintError("No account found with username: "+username);
                return null;
            }
        }
        public PlayerData GetPlayerById(string id)
        {
            try
            {
                return _database.GetCollection<PlayerData>("players").Find(p => p.accountId == id).ToList().FirstOrDefault();
            }
            catch(Exception e)
            {
                Logger.PrintError("Error occured while loading Player with account id: " + id+" ERROR:\n"+e.Message);
                return null;
            }
        }


    }
}
