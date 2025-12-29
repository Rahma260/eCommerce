using eCommerce.Domain.Entities.Identity;
using eCommerce.Domain.Interfaces;
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
    public class TokenManagement(
      IConfiguration configuration,
      IUnitOfWork unitOfWork
        ) : ITokenManagement
    {

        public async Task<int> AddRefreshToken(string refreshToken, string userId)
        {
            await unitOfWork.RefreshToken.AddAsync(new RefreshToken
            {
                Token = refreshToken,
                UserId = userId,
            });

            return await unitOfWork.SaveAsync();
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
            var token = await unitOfWork.RefreshToken.GetByTokenAsync(refreshToken);
            return token!.UserId;
        }


        public async Task<int> UpdateRefreshToken(string oldToken, string newToken)
        {
            var token = await unitOfWork.RefreshToken.GetByTokenAsync(oldToken);
            if (token == null) return -1;

            token.Token = newToken;
            return await unitOfWork.SaveAsync();
        }


        public async Task<bool> ValidateRefreshToken(string refreshToken)
        {
            var token = await unitOfWork.RefreshToken.GetByTokenAsync(refreshToken);
            return token != null;
        }

    }
}
