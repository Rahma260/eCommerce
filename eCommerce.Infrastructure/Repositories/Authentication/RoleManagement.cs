using eCommerce.Domain.Entities.Identity;
using eCommerce.Domain.Interfaces.Identity;
using Microsoft.AspNetCore.Identity;

namespace eCommerce.Infrastructure.Repositories.Identity
{
    public class RoleManagement(UserManager<User> userManager) : IRoleManagement
    {
        public async Task<bool> AddUserToRole(User user, string roleName)
        {
            return (await userManager.AddToRoleAsync(user, roleName)).Succeeded;
        }

        public async Task<string?> GetUserRole(string useEmail)
        {
            var user = await userManager.FindByEmailAsync(useEmail);
            return user is null ? null : (await userManager.GetRolesAsync(user)).FirstOrDefault();
        }
    }

}
