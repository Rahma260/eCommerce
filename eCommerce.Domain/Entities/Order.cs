using eCommerce.Domain.Entities.Identity;

namespace eCommerce.Domain.Entities
{
    public class Order
    {
        public int Id { get; set; }

        public string UserId { get; set; } = null!;
        public User User { get; set; } = null!;

        public DateTime OrderDate { get; set; } = DateTime.UtcNow;
        public OrderStatus Status { get; set; } = OrderStatus.Pending;

        public decimal TotalAmount { get; set; }

        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

        public Payment? Payment { get; set; }
    }

}
