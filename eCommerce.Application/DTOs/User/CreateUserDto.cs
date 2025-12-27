using eCommerce.Domain.Entities;

namespace eCommerce.Application.DTOs.User
{
    public class CreateUserDto : BaseUserDto
    {
        public string FullName { get; set; } = null!;
        public Gender Gender { get; set; }
        public string ConfirmPassword { get; set; } = null!;

    }

}
