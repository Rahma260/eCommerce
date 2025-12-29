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
    public class RefreshTokenRepository(DBContext context)
     : GenericRepository<RefreshToken>(context), IRefreshTokenRepository
    {
        public async Task<RefreshToken?> GetByTokenAsync(string token)
        {
            return await context.Set<RefreshToken>()
                .FirstOrDefaultAsync(x => x.Token == token);
        }
    }
}
