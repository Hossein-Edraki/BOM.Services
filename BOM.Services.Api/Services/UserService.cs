using BOM.Services.Api.Entites;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace BOM.Services.Api.Services
{
    public class UserService //: IUserService
    {
        private readonly IMongoCollection<Meeting> _meetingCollection;

        public UserService(IOptions<OpenviduStoreSetting> setting)
        {
            var mongoClient = new MongoClient(
                setting.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                setting.Value.DatabaseName);

            _meetingCollection = mongoDatabase.GetCollection<Meeting>(
                setting.Value.MeetingCollectionName);
        }

        public async Task<List<Meeting>> GetAsync() =>
            await _meetingCollection.Find(_ => true).ToListAsync();

        public async Task<Meeting?> GetAsync(ObjectId _id) =>
            await _meetingCollection.Find(x => x._id == _id).FirstOrDefaultAsync();

        public async Task CreateAsync(Meeting meeting) =>
            await _meetingCollection.InsertOneAsync(meeting);

        public async Task UpdateAsync(ObjectId _id, Meeting meeting) =>
            await _meetingCollection.ReplaceOneAsync(x => x._id == _id, meeting);

        public async Task RemoveAsync(ObjectId _id) =>
            await _meetingCollection.DeleteOneAsync(x => x._id == _id);

        public async Task<Meeting?> GetLastSessionAsync(int id, string sessionId) =>
            await _meetingCollection.Find(x => x.UserId == id && x.SessionId == sessionId && string.IsNullOrEmpty(x.ConnectionToken))?.FirstOrDefaultAsync();
    }
}
