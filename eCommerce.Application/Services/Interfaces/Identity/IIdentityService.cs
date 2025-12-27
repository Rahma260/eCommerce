using eCommerce.Application.DTOs;
using eCommerce.Application.DTOs.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.Application.Services.Interfaces.Identity
{
    public interface IIdentityService
    {
        Task<ServiceResponse> CreateUser(CreateUserDto user);
        Task<LoginResponse> LoginUser(LoginUserDto user);
        Task<LoginResponse> RevokeToken(string refreshToken);

    }
}
