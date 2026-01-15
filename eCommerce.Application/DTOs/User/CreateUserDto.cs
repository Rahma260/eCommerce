using eCommerce.Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace eCommerce.Application.DTOs.User
{
    public class CreateUserDto : BaseUserDto
    {
        public string FullName { get; set; } = null!;
        public Gender Gender { get; set; }
        public string ConfirmPassword { get; set; } = null!;
        public IFormFile Image { get; set; } = null!;
    }

}
