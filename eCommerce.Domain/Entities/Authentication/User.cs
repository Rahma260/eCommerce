using Microsoft.AspNetCore.Identity;
namespace eCommerce.Domain.Entities.Identity
{
    public class User : IdentityUser
    {
        public string FullName { get; set; } = null!;
        public Gender Gender { get; set; }

        public ICollection<Address> Addresses { get; set; } = new List<Address>();
        public ICollection<Order> Orders { get; set; } = new List<Order>();

        public string? Provider { get; set; }     // Google
        public string? ProviderId { get; set; }   // sub from Google

        public Image? Image { get; set; }

        public static User CreateGoogleUser(
            string email,
            string? fullName,
            string? pictureUrl,
            string providerId)
        {
            var user = new User
            {
                Email = email,
                UserName = email,
                FullName = fullName ?? email,
                Gender = Gender.Male,
                Provider = "Google",
                ProviderId = providerId
            };

            var tempPassword = Guid.NewGuid().ToString("N") + "Aa1!";
            user.PasswordHash =
                new PasswordHasher<User>().HashPassword(user, tempPassword);

            return user;
        }
    }

}
