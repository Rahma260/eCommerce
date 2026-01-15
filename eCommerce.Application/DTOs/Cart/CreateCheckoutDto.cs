
namespace eCommerce.Application.DTOs.Cart
{
    public class CreateCheckoutDto
    {
        public string UserId { get; set; } = null!;
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
