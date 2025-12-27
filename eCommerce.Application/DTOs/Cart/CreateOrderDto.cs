
namespace eCommerce.Application.DTOs.Cart
{
    public class CreateOrderDto
    {
        public string UserId { get; set; } = null!;
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
