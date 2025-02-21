using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Bson.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson.IO;
using MongoDB.Bson;
using System.Reflection;
using static ArkFieldPS.Game.Factory.FactoryNode;

namespace ArkFieldPS.Database
{
    public class CustomDictionarySerializer<TKey, TValue> : IBsonSerializer<Dictionary<TKey, TValue>>
    {
        public Type ValueType => typeof(Dictionary<TKey, TValue>);
        public Dictionary<TKey, TValue> Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
        {
            var dictionary = new Dictionary<TKey, TValue>();
            var reader = context.Reader;

            reader.ReadStartDocument();
            while (reader.ReadBsonType() != BsonType.EndOfDocument)
            {
                var key = (TKey)Convert.ChangeType(reader.ReadName(), typeof(TKey));
                var value = (TValue)BsonSerializer.Deserialize<TValue>(reader);
                dictionary[key] = value;
            }
            reader.ReadEndDocument();

            return dictionary;
        }

        public void Serialize(BsonSerializationContext context, BsonSerializationArgs args, Dictionary<TKey, TValue> value)
        {
            var writer = context.Writer;

            writer.WriteStartDocument();
            foreach (var kvp in value)
            {
                writer.WriteName(kvp.Key.ToString());
                BsonSerializer.Serialize(writer, kvp.Value);
            }
            writer.WriteEndDocument();
        }

        public void Serialize(BsonSerializationContext context, BsonSerializationArgs args, object value)
        {
            var writer = context.Writer;

            writer.WriteStartDocument();
            foreach (var kvp in (Dictionary < TKey, TValue > )value)
            {
                writer.WriteName(kvp.Key.ToString());
                BsonSerializer.Serialize(writer, kvp.Value);
            }
            writer.WriteEndDocument();
        }
        object IBsonSerializer.Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args) =>
            Deserialize(context, args);
    }

    public class DatabaseManager
    {
        public static Database db;
        public static void Init()
        {
            BsonSerializer.RegisterSerializer(typeof(Dictionary<int, ulong>), new CustomDictionarySerializer<int, ulong>());
            BsonSerializer.RegisterSerializer(typeof(Dictionary<int, List<int>>), new CustomDictionarySerializer<int, List<int>>());
            RegisterSubclasses<FComponent>();
            Logger.Print("Connecting to MongoDB..."); 
            try
            {
                db = new Database(Server.config.mongoDatabase.uri, Server.config.mongoDatabase.collection);
                Logger.Print("Connected to MongoDB database");
            }
            catch (Exception ex)
            {
                Logger.PrintError(ex.Message);
                Logger.PrintError("Without initialized database the game server will crash. You can't run this server without MongoDB");
            }
           
        }
        static void RegisterSubclasses<TBase>()
        {
            // Trova tutte le classi che ereditano da TBase
            var derivedTypes = Assembly.GetExecutingAssembly()
                                       .GetTypes()
                                       .Where(t => t.IsClass && !t.IsAbstract && typeof(TBase).IsAssignableFrom(t));

            foreach (var type in derivedTypes)
            {
                if (!BsonClassMap.IsClassMapRegistered(type))
                {
                    BsonClassMap.LookupClassMap(type); // Registra automaticamente il mapping BSON
                }
            }
        }
    }
}
