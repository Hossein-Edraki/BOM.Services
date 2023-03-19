using BOM.Services.Api.Interfaces;
using MongoDB.Bson;

namespace BOM.Services.Api.Services
{
    public class MeetingService : IMeetingService
    {
        private readonly IOpenviduApiService _openviduApiService;
        private readonly ILogger<MeetingService> _logger;
        private readonly UserService _userService;

        public MeetingService(IOpenviduApiService openviduApiService
            , ILogger<MeetingService> logger
            , UserService userService)
        {
            _openviduApiService = openviduApiService;
            _logger = logger;
            _userService = userService;
        }

        public async Task<string> ConnetToMeeting(ObjectId _id, int userId, string sessionId)
        {
            var meeting = await _userService.GetAsync(_id);
            if (meeting == null)
            {
                _logger.LogWarning("meeting not found");
                return default;
            }

            var result = await _openviduApiService.Connect(new Models.ConnectRequest { SessionId = meeting.SessionId });
            if (result == null)
            {
                _logger.LogWarning("connectToken is null");
                return default;
            }
            else
            {
                meeting.ConnectionToken = result.Token;
                await _userService.UpdateAsync(_id, meeting);
                return result.Token;
            }
        }
    }
}
