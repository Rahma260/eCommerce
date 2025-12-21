using eCommerce.Domain.Entities;
using eCommerce.Domain.Interfaces;
using eCommerce.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DBContext _context;

        public IGenericRepository<Product> Products { get; private set; }
        public IGenericRepository<Category> Categories { get; private set; }

        public UnitOfWork(DBContext context)
        {
            _context = context;
            Products = new GenericRepository<Product>(_context);
            Categories = new GenericRepository<Category>(_context);
        }

        public async Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
