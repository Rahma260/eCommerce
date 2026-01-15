using System.Security.Claims;

namespace eCommerce.Domain.Interfaces.Identity
{
    public interface ITokenManagement
    {
        string GetRefreshToken();
        Task<List<Claim>> GetUserClaimsFromToken(string token);
        Task<bool> ValidateRefreshToken(string refreshToken);
        Task<string> GetUserIdByRefreshToken(string refreshToken);
        Task<int> AddRefreshToken(string refreshToken, string userId);
        Task<int> UpdateRefreshToken(string refreshToken, string userId);
        string GenerateToken(List<Claim> claims); 

    }
}
