using MongoDB.Bson;

namespace BOM.Services.Api.Interfaces
{
    public interface IMeetingService
    {
        Task<string> ConnetToMeeting(ObjectId _id, int userId, string sessionId);
    }
}
