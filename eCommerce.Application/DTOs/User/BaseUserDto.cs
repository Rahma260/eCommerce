using eCommerce.Domain.Entities;

namespace eCommerce.Application.DTOs.User
{
    public abstract class BaseUserDto
    {
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
    }

}
