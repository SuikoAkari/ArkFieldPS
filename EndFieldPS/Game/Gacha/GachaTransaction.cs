using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization.IdGenerators;

namespace EndFieldPS.Game.Gacha
{
    public class GachaTransaction
    {
        [BsonId(IdGenerator = typeof(ObjectIdGenerator))]
        public ObjectId _id { get; set; }
        public ulong ownerId;
        public long transactionTime;
        public string itemId;
        public string gachaTemplateId;
        public int rarity;
        public bool hasLost = false;
    }
}
