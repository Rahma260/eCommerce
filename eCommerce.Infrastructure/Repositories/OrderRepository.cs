using eCommerce.Domain.Entities;
using eCommerce.Domain.Entities.Identity;
using eCommerce.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.Infrastructure.Repositories
{
    public class OrderRepository (DBContext _context): GenericRepository<Order>(_context), IOrderRepository
    {
       

        // ===========================
        // Get all orders with optional filter, orderBy, includeProperties
        // ===========================
        public async Task<IEnumerable<Order>> GetAllAsync(
            Expression<Func<Order, bool>>? filter = null,
            Func<IQueryable<Order>, IOrderedQueryable<Order>>? orderBy = null,
            string includeProperties = "")
        {
            IQueryable<Order> query = _context.Orders;

            if (filter != null)
                query = query.Where(filter);

            // Include related entities
            foreach (var includeProperty in includeProperties.Split(
                         new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty.Trim());
            }

            if (orderBy != null)
                query = orderBy(query);

            return await query.ToListAsync();
        }

        // ===========================
        // Get single order by Id
        // ===========================
        public async Task<Order?> GetByIdAsync(int id, string includeProperties = "")
        {
            IQueryable<Order> query = _context.Orders.Where(o => o.Id == id);

            foreach (var includeProperty in includeProperties.Split(
                         new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty.Trim());
            }

            return await query.FirstOrDefaultAsync();
        }
    }
}
