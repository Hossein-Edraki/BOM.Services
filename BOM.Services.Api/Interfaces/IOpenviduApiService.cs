using BOM.Services.Api.Models;

namespace BOM.Services.Api.Interfaces
{
    public interface IOpenviduApiService
    {
        Task<CreateSessionResponse> CreateSession(CreateSessionRequest request);
        Task<ConnectResponse> Connect(ConnectRequest request);
    }
}
