namespace BOM.Services.Api.Interfaces
{
    public interface IJwtService
    {
        int? ValidateToken(string token);
        string GenerateToken(string userId);
    }
}
