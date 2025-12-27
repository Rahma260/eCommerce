using eCommerce.Domain.Entities.Identity;

namespace eCommerce.Domain.Entities
{
    public class UserRole
    {
        public string UserId { get; set; } = null!;
        public User User { get; set; } = null!;

        public string RoleId { get; set; } = null!;
        public Role Role { get; set; } = null!;
    }


}
