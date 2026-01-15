using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.Application.DTOs.User.GoogleUser
{
    public class GoogleLoginRequestDto
    {
        public string IdToken { get; set; } = null!;
    }
}
