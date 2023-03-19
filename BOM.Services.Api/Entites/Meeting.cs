using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BOM.Services.Api.Entites
{
    public class Meeting
    {
        [BsonId]
        public ObjectId _id { get; set; }

        [BsonElement("userId")]
        public int UserId { get; set; }

        [BsonElement("createTime")]
        public DateTime CreateTime { get; set; }

        [BsonElement("sessionId")]
        public string SessionId { get; set; }

        [BsonElement("connectionToken")]
        public string ConnectionToken { get; set; }
        public Meeting()
        {
            this.CreateTime = DateTime.Now;
            _id = ObjectId.GenerateNewId();
        }
    }
}
