namespace eCommerce.Domain.Entities
{
    public class Address
    {
        public int UserId { get; set; }
        public string City { get; set; } = null!;
        public string Street { get; set; } = null!;
        public string Building { get; set; } = null!;
    }

}
