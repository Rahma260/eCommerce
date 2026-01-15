using eCommerce.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.Domain.Interfaces.Identity
{
    public interface IRoleManagement
    {
        Task<string?> GetUserRole(string useEmail);
        Task<bool> AddUserToRole(User user, string roleName);

    }
}
