using eCommerce.Domain.Entities.Identity;

namespace eCommerce.Domain.Entities
{
    public class Address
    {
        public int Id { get; set; }
        public string UserId { get; set; } = null!;
        public User User { get; set; } = null!;
        public string City { get; set; } = null!;
        public string Street { get; set; } = null!;
        public string Building { get; set; } = null!;
    }

}
