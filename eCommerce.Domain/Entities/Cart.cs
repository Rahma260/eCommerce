namespace eCommerce.Domain.Entities
{
    public class Cart
    {
        public string UserId { get; set; } = null!;
        public List<CartItem> Items { get; set; } = new();
    }


}
