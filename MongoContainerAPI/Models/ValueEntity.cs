using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MongoContainerAPI.Models
{
    public class ValueEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("value")]
        public string Value { get; set; }
    }
}
