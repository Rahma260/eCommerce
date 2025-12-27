using eCommerce.Domain.Entities;
using eCommerce.Domain.Entities.Identity;
namespace eCommerce.Domain.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<Product> Products { get; }
        IGenericRepository<Category> Categories { get; }
     //   IGenericRepository<Cart> Carts { get; }
        IGenericRepository<CartItem> CartItems { get; }
        IGenericRepository<Order> Orders { get; }
        IGenericRepository<OrderItem> OrderItems { get; }
        IGenericRepository<Payment> Payments { get; }
        IGenericRepository<User> Users { get; }
        IGenericRepository<Role> Roles { get; }
        IGenericRepository<UserRole> UserRoles { get; }
        IGenericRepository<RefreshToken> RefreshToken { get; }
        IGenericRepository<Brand> Brands { get; }
        IGenericRepository<PaymentMethod> PaymentMethods { get; }

        Task<int> SaveAsync();
    }
}
