using eCommerce.Application.DTOs;
using eCommerce.Application.DTOs.User;
using eCommerce.Application.Services.Interfaces.Profile;
using eCommerce.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using System.Security.Claims;

namespace eCommerce.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfileController(IProfileService profileService) : ControllerBase
    {
        [HttpPatch("change-password")]
        public async Task<IActionResult> ChangePassword(ChangePasswordDto changePasswordDto)
        {
            var result = await profileService.ChangePassword(changePasswordDto);
            if (!result.success)
                return BadRequest(result);
            return Ok(result);
        }
        [Authorize(Roles ="User")]
        [HttpPut("change-profile-picture")]
        public async Task<IActionResult> ChangePicture([FromForm] UpdateImageDto updateImageDto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value; 
            if (string.IsNullOrEmpty(userId))
                return Unauthorized(new { success = false, message = "Invalid token" });
            var result = await profileService.UpdateImage(userId, updateImageDto.Image);
            if (!result.success)
                return BadRequest(result);
            return Ok(result);
        }

        [HttpGet("all")]
        [Authorize(Roles = "Admin")] //---> get cors error
        public async Task<IActionResult> GetAllUsers()
        {
            var result = await profileService.GetAllUsers();
            if (!result.Any())
                return NotFound("No users found");
            return Ok(result);
        }
        [HttpGet("{email}")]
        public async Task<IActionResult> GetUserByEmail(string email)
        {
            var result = await profileService.GetUserByEmail(email);
            if (result == null)
                return NotFound("No user found with this email");
            return Ok(result);
        }
        [HttpPut("update")]
        public async Task<IActionResult> UpdateUser(UserDto userBaseDto)
        {
            var result = await profileService.UpdateUser(userBaseDto);
            if (!result.success)
                return BadRequest(result);
            return Ok(result);
        }
        [HttpDelete("delete/{email}")]
        public async Task<IActionResult> DeleteUser(string email)
        {
            var result = await profileService.DeleteUser(email);
            if (!result.success)
                return BadRequest(result);
            return Ok(result);
        }
    }
}
