using eCommerce.Application.DTOs.User.GoogleUser;
using eCommerce.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.Application.Services.Interfaces.Identity
{
    public interface IGoogleAuthService
    {
        Task<GoogleUserInfoDto> ValidateTokenAsync(string idToken);
    }

}
