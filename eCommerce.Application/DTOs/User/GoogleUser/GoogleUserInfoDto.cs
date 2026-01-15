using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.Application.DTOs.User.GoogleUser
{
    public class GoogleUserInfoDto
    {
        public string Email { get; set; } = null!;
        public string? Name { get; set; }  // Made nullable
        public string? Picture { get; set; }  // Made nullable
        public string Sub { get; set; } = null!;  // Added for Google user ID
    }
}
