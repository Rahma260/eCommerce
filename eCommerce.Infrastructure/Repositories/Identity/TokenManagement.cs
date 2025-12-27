using eCommerce.Domain.Entities.Identity;
using eCommerce.Domain.Interfaces.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;

namespace eCommerce.Infrastructure.Repositories.Identity
{
    public class TokenManagement(IConfiguration configuration, DBContext dBContext) : ITokenManagement
    {
        public Task<int> AddRefreshToken(string refreshToken, string userId)
        {
            dBContext.RefreshToken.Add(new RefreshToken
            {
                Token = refreshToken,
                UserId = userId,
            });
            return dBContext.SaveChangesAsync();
        }

        public string GenerateToken(List<Claim> claims)
        {
            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(configuration["JWT:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expiration = DateTime.UtcNow.AddMinutes(Convert.ToDouble(configuration["JWT:DurationInMinutes"]));
            var token = new JwtSecurityToken(
                issuer: configuration["JWT:Issuer"],
                audience: configuration["JWT:Audience"],
                claims: claims,
                expires: expiration,
                signingCredentials: creds
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string GetRefreshToken()
        {
            const int byteSize = 64;
            byte[] randomBytes = new byte[byteSize];
            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                 rng.GetBytes(randomBytes);
            }
            string token = Convert.ToBase64String(randomBytes);
            return WebUtility.UrlEncode(token);
        }

        public async Task<List<Claim>> GetUserClaimsFromToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadJwtToken(token);
            if (jwtToken == null) return [];
            return jwtToken.Claims.ToList();
        }

        public async Task<string> GetUserIdByRefreshToken(string refreshToken)
        {
            return (await dBContext.RefreshToken.FirstOrDefaultAsync(rt => rt.Token == refreshToken))!.UserId;
        }

        public async Task<int> UpdateRefreshToken(string refreshToken, string userId)
        {
            var user = await dBContext.RefreshToken.FirstOrDefaultAsync(rt => rt.Token == refreshToken);
            if (user == null) return -1;
            user.Token = refreshToken;
            return await dBContext.SaveChangesAsync();
        }

        public async Task<bool> ValidateRefreshToken(string refreshToken)
        {
            var user = await dBContext.RefreshToken.FirstOrDefaultAsync(rt => rt.Token == refreshToken);
            return user != null;
        }        
    }
}
