using eCommerce.Domain.Entities;
using eCommerce.Domain.Interfaces;

namespace eCommerce.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DBContext _context;

        public IGenericRepository<Product> Products { get; private set; }
        public IGenericRepository<Category> Categories { get; private set; }
        public IGenericRepository<Cart> Carts { get; private set; }
        public IGenericRepository<CartItem> CartItems { get; private set; }

        public IGenericRepository<Order> Orders { get; private set; }
        public IGenericRepository<OrderItem> OrderItems { get; private set; }

        public IGenericRepository<Payment> Payments { get; private set; }

        public IGenericRepository<User> Users { get; private set; }
        public IGenericRepository<Role> Roles { get; private set; }
        public IGenericRepository<UserRole> UserRoles { get; private set; }
        public UnitOfWork(DBContext context)
        {
            _context = context;
            Products = new GenericRepository<Product>(_context);
            Categories = new GenericRepository<Category>(_context);
            Carts = new GenericRepository<Cart>(_context);
            CartItems = new GenericRepository<CartItem>(_context);
            Orders = new GenericRepository<Order>(_context);
            OrderItems = new GenericRepository<OrderItem>(_context);
            Payments = new GenericRepository<Payment>(_context);
            Users = new GenericRepository<User>(_context);
            Roles = new GenericRepository<Role>(_context);
            UserRoles = new GenericRepository<UserRole>(_context);
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