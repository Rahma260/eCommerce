using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.Application.DTOs.User
{
    public class ResetPasswordDto
    {
        public string email { get; set; } = null!;
        public string otp { get; set; } = null!;
        public string newPassword { get; set; } = null!;
    }
}
