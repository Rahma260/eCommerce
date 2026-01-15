using eCommerce.Domain.Entities.Identity;
using eCommerce.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.Infrastructure.Repositories
{
    public class UserRepository(DBContext context) : GenericRepository<User>(context), IUserRepository
    {

        public async Task<User?> GetUserWithImageAsync(string userId)
        {
             return await context.Users
               .Include(u => u.Image)
               .FirstOrDefaultAsync(u => u.Id == userId);
        }
    }
}
