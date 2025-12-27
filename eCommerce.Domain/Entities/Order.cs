using eCommerce.Domain.Entities.Identity;

namespace eCommerce.Domain.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public string UserId { get; set; } = null!;
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public DateTime CreatedData { get; set; } = DateTime.UtcNow;
    }

}
