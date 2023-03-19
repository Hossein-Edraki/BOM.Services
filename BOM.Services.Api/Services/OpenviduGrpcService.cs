using BOM.Services.Api.Interfaces;
using BOM.Services.Api.Proto;
using Grpc.Core;

namespace BOM.Services.Api.Services
{
    public class OpenviduGrpcService : OpenviduService.OpenviduServiceBase
    {
        private readonly IOpenviduApiService _openviduApiService;
        private readonly UserService _userService;
        private readonly ILogger<OpenviduGrpcService> _logger;

        public OpenviduGrpcService(IOpenviduApiService openviduApiService
            , UserService userService
            , ILogger<OpenviduGrpcService> logger)
        {
            _openviduApiService = openviduApiService;
            _userService = userService;
            _logger = logger;
        }

        public override async Task<RpcCreateSessionResponse> CreateSession(RpcCreateSessionRequest request, ServerCallContext context)
        {
            var result = await _openviduApiService.CreateSession(new Models.CreateSessionRequest { });
            if (result == null)
            {
                _logger.LogWarning("create session result is null");
                return default;
            }
            else
            {
                var meeting = new Entites.Meeting { SessionId = result.SessionId, UserId = request.UserId };
                await _userService.CreateAsync(meeting);
                return new RpcCreateSessionResponse { SessionLink = $"http://localhost:5000/api/Meeting/connect/{result.SessionId}" };
            }
        }
    }
}
