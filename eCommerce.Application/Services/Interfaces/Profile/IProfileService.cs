using eCommerce.Application.DTOs;
using eCommerce.Application.DTOs.Responses;
using eCommerce.Application.DTOs.User;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.Application.Services.Interfaces.Profile
{
    public interface IProfileService
    {
        Task<IEnumerable<UserDto>> GetAllUsers();
        Task<ServiceResponse> ChangePassword(ChangePasswordDto changePasswordDto);
        Task<UserDto?> GetUserByEmail(string email);
        Task<ServiceResponse> UpdateUser(UserDto userBaseDto); 
        Task<ServiceResponse> UpdateImage(string userId,
        IFormFile newImage); 
        Task<ServiceResponse> DeleteUser(string email); 
    }
}
