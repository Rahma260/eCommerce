using eCommerce.Domain.Entities;

namespace eCommerce.Application.DTOs.User
{
    public class UserDto
    {
        public string Id { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public Gender Gender { get; set; }
        //public List<AddressBaseDto> Addresses { get; set; } = new();
        //public List<string> Roles { get; set; } = new();
    }

}
