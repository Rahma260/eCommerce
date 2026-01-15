using eCommerce.Application.DTOs.Responses;
using eCommerce.Application.DTOs.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.Application.Services.Interfaces.Identity
{
    public interface IAuthenticationService
    {
        Task<ServiceResponse> CreateUser(CreateUserDto user);
        Task<LoginResponse> LoginUser(LoginUserDto user);
        Task<LoginResponse> RevokeToken(string refreshToken);
        Task<ForgetPasswordResponse> ForgetPassword(ForgetPasswordDto forgetPasswordDto);
        Task<ServiceResponse> ResetPassword(ResetPasswordDto resetPasswordDto);
    }
}
