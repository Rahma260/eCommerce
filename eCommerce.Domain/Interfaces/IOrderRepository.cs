using eCommerce.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.Domain.Interfaces
{
    public interface IOrderRepository : IGenericRepository<Order>
    {
        Task<IEnumerable<Order>> GetAllAsync(
            Expression<Func<Order, bool>>? filter = null,
            Func<IQueryable<Order>, IOrderedQueryable<Order>>? orderBy = null,
            string includeProperties = "");

        Task<Order?> GetByIdAsync(int id, string includeProperties = "");
    }
}
