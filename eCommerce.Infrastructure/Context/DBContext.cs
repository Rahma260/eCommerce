using eCommerce.Domain.Entities;
using eCommerce.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

public class DBContext : IdentityDbContext<User, Role, string>
{
    public DBContext(DbContextOptions<DBContext> options)
        : base(options) { }

    public DbSet<RefreshToken> RefreshToken { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Checkout> Checkouts { get; set; }
    public DbSet<Image> Images { get; set; }
    public DbSet<PaymentMethod> PaymentMethodands { get; set; }


    //didn't use yet
    //public DbSet<Payment> Payments { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    public DbSet<Order> Orders { get; set; }

    //public DbSet<Cart> Carts { get; set; }
    //public DbSet<CartItem> CartItems { get; set; }

    //public DbSet<Brand> Brands { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Decimal precision
        modelBuilder.Entity<Product>()
            .Property(p => p.Price)
            .HasPrecision(18, 2);

    
        modelBuilder.Entity<RefreshToken>()
            .HasOne(r => r.User)
            .WithMany()
            .HasForeignKey(r => r.UserId);

        modelBuilder.Entity<Checkout>()
            .HasOne(c => c.User)
            .WithMany()
            .HasForeignKey(c => c.UserId);

        // Seed Roles
        modelBuilder.Entity<Role>().HasData(
            new Role
            {
                Id = "ADMIN_ROLE_ID",
                Name = "Admin",
                NormalizedName = "ADMIN"
            },
            new Role
            {
                Id = "USER_ROLE_ID",
                Name = "User",
                NormalizedName = "USER"
            });
        modelBuilder.Entity<PaymentMethod>().HasData(
            new PaymentMethod
            {
                Id = 1,
                Name = "Credit Card",
            },
            new PaymentMethod
            {
                Id = 2,
                Name = "PayPal",
            });
    }
}
