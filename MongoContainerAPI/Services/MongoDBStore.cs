using MongoDB.Driver;
using MongoContainerAPI.Models;

namespace MongoContainerAPI.Services
{
    public class MongoDBStore : IDB
    {
        private readonly IMongoCollection<ValueEntity> _collection;

        public MongoDBStore(IConfiguration config, IMongoClient client)
        {
            var dbName = config["MongoDB:DatabaseName"];
            var collName = config["MongoDB:CollectionName"];
            _collection = client
                .GetDatabase(dbName)
                .GetCollection<ValueEntity>(collName);
        }

        public void SaveValue(string parameter)
        {
            var entity = new ValueEntity { Value = parameter };
            _collection.InsertOne(entity);
        }

        public IEnumerable<string> GetValue()
        {
            return _collection
                .Find(FilterDefinition<ValueEntity>.Empty)
                .Project(v => v.Value)
                .ToList();
        }
    }
}