using Microsoft.AspNetCore.Identity;
namespace eCommerce.Domain.Entities.Identity
{
    public class User : IdentityUser
    {
        public string FullName { get; set; } = null!;
        public Gender Gender { get; set; }
        public ICollection<Address> Addresses { get; set; } = new List<Address>();
        //public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
        public Cart? Cart { get; set; }
        public ICollection<Order> Orders { get; set; } = new List<Order>();
    }

}
