namespace eCommerce.Application.DTOs.User
{
    public class AddressBaseDto
    {
        public int Id { get; set; }
        public string City { get; set; } = null!;
        public string Street { get; set; } = null!;
    }

}
