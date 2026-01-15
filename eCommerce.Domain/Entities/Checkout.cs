using eCommerce.Domain.Entities.Identity;

namespace eCommerce.Domain.Entities
{
    public class Checkout
    {
        public int Id { get; set; }
        public string UserId { get; set; } = null!;
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public DateTime CreatedData { get; set; } = DateTime.UtcNow;
        public User User { get; set; } = null!;
    }

}
