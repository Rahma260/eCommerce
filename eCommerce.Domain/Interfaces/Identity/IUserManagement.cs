using eCommerce.Domain.Entities.Identity;
using System.Security.Claims;

namespace eCommerce.Domain.Interfaces.Identity
{
    public interface IUserManagement
    {
        Task<bool> CreateUser(User user);
        Task<bool> LoginUser(User user);
        Task<User?> GetUserByEmail(string email);
        Task<User?> GetUserById(string id);
        Task<IEnumerable<User>> GetAllUsers();
        Task<bool> RemoveUserByEmail(string email);
        Task<List<Claim>> GetUserClaims(string email);
       

    }
}
