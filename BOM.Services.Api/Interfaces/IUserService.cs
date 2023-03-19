using BOM.Services.Api.Entites;

namespace BOM.Services.Api.Interfaces
{
    public interface IUserService
    {
        Task<List<Meeting>> GetAsync();
        Task<Meeting?> GetAsync(int id);
        Task CreateAsync(Meeting user);
        Task UpdateAsync(int id, Meeting user);
        Task RemoveAsync(int id);
        Task<Meeting?> GetLastSessionAsync(int id, string sessionId);
    }
}
