using eCommerce.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
namespace eCommerce.Infrastructure.Repositories
{
    public class GenericRepository<TEntity>(DBContext context) : IGenericRepository<TEntity> where TEntity : class
    {
        //private readonly DBContext _context = context;

        public async Task AddAsync(TEntity entity)
        {
            await context.Set<TEntity>().AddAsync(entity);
            //return await context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await context.Set<TEntity>().FindAsync(id);
            //don't throw an exception in application layer
            //throw new ItemNotFoundException($"Item with id {id} is not found");
            if (entity == null)
                return;
            context.Set<TEntity>().Remove(entity);
           // return await context.SaveChangesAsync();
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            //use AsNoTracking() for read-only queries
            return await context.Set<TEntity>().AsNoTracking().ToListAsync();
        }

        public async Task<TEntity?> GetByIdAsync(int id)
        {
            return await context.Set<TEntity>().FindAsync(id); 
        }

        public async Task UpdateAsync(TEntity entity)
        {
            context.Set<TEntity>().Update(entity);
           // return await context.SaveChangesAsync();
        }
    }
}
