using eCommerce.Domain.Entities;
using eCommerce.Domain.Entities.Identity;
using eCommerce.Domain.Interfaces.Authentication;
namespace eCommerce.Domain.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<Product> Products { get; }
        IGenericRepository<Category> Categories { get; }
     //   IGenericRepository<Cart> Carts { get; }
      //  IGenericRepository<CartItem> CartItems { get; }
        IGenericRepository<Checkout> Checkouts { get; }
        IGenericRepository<OrderItem> OrderItems { get; }
        IOrderRepository Orders { get; }
    //    IGenericRepository<Payment> Payments { get; }
        IUserRepository Users { get; }
        IGenericRepository<Role> Roles { get; }
        IGenericRepository<UserRole> UserRoles { get; }
        IRefreshTokenRepository RefreshToken { get; }
      //  IGenericRepository<Brand> Brands { get; }
        IGenericRepository<PaymentMethod> PaymentMethods { get; }
        IGenericRepository<Image> Images { get; }

        Task<int> SaveAsync();
    }
}
