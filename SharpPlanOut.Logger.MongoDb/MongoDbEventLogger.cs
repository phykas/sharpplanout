using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using Newtonsoft.Json;
using SharpPlanOut.Core;
using System.Collections.Generic;

namespace SharpPlanOut.Logger.MongoDb
{
    public class MongoDbEventLogger : IEventLogger
    {
        protected static IMongoClient _client;
        protected static IMongoDatabase _database;
        private readonly IMongoCollection<BsonDocument> _mongoCollection;

        public MongoDbEventLogger()
        {
            _client = new MongoClient();
            _database = _client.GetDatabase("planout");
            _mongoCollection = _database.GetCollection<BsonDocument>("event_logs");
        }

        public void Log(Dictionary<string, object> logData)
        {
            var jsonDoc = JsonConvert.SerializeObject(logData);
            var bsonDoc = BsonSerializer.Deserialize<BsonDocument>(jsonDoc);
            _mongoCollection.InsertOne(bsonDoc);
        }
    }
}