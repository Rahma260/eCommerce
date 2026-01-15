using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.Application.DTOs.Responses
{
    public record LoginResponse
    (
        bool success = false,
        string message = null!,
        string token = null!,
        string refreshToken = null!
    );
}
