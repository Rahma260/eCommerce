using eCommerce.Domain.Entities.Identity;
using eCommerce.Domain.Interfaces.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Threading.Tasks;

namespace eCommerce.Infrastructure.Repositories.Identity
{
    public class UserManagement(IRoleManagement roleManagement, UserManager<User> userManager) : IUserManagement
    {
        public async Task<bool> CreateUser(User user)
        {
            var _user = await GetUserByEmail(user.Email!);
            if (_user != null) return false;
            return (await userManager.CreateAsync(user!, user!.PasswordHash!)).Succeeded;
        }

        public async Task<IEnumerable<User>> GetAllUsers()
        {
            return await userManager.Users.ToListAsync();
        }

        public async Task<User?> GetUserByEmail(string email)
        {
            return await userManager.FindByEmailAsync(email);
        }

        public async Task<User?> GetUserById(string id)
        {
            var user = await userManager.FindByIdAsync(id);
            return user;
        }

        public async Task<List<Claim>> GetUserClaims(string email)
        {
            var user = await userManager.FindByEmailAsync(email);
            string? roleName = await roleManagement.GetUserRole(user!.Email!);
            List<Claim> claims = [
                new Claim(ClaimTypes.Name, user.UserName!),
                new Claim(ClaimTypes.Email, user.Email!),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Role, roleName!)
            ];
            return claims;

        }

        public async Task<bool> LoginUser(User user)
        {
            var _user = await GetUserByEmail(user.Email!);
            if(_user is null) return false;
            string ? roleName = await roleManagement.GetUserRole(_user!.Email!);
            if(roleName is null) return false;
            return await userManager.CheckPasswordAsync(_user!, user.PasswordHash!);
        }

        public async Task<bool> RemoveUserByEmail(string email)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user is null) return false;

            return (await userManager.DeleteAsync(user)).Succeeded;
        }

        public async Task<bool> UpdateAsyncasswordHash(User user, string currentPassword, string newPassword)
        {
            return (await userManager.ChangePasswordAsync(user, currentPassword, newPassword)).Succeeded;
        }
        public async Task<IdentityResult> ResetPasswordAsync(
            User user,
            string token,
            string newPassword)
        {
            return await userManager.ResetPasswordAsync(user, token, newPassword);
        }
        public async Task<string> GenerateResetPasswordToken(User user)
        {
            return await userManager.GeneratePasswordResetTokenAsync(user);
        }

    }
}
