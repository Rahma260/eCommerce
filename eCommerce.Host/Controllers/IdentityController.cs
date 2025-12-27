using eCommerce.Application.DTOs.User;
using eCommerce.Application.Services.Implementations.Identity;
using eCommerce.Application.Services.Interfaces.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eCommerce.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IdentityController(IIdentityService identityService) : ControllerBase
    {
        [HttpPost("register")]
        public async Task<IActionResult> Register(CreateUserDto user)
        {
            var result = await identityService.CreateUser(user);
            if (!result.success)
                return BadRequest(result);
            return Ok(result);
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginUserDto user)
        {
            var result = await identityService.LoginUser(user);
            if (!result.success)
                return BadRequest(result);
            return Ok(result);
        }
        [HttpGet("refreshToken/{refreshToken}")]
        public async Task<IActionResult> RefreshToken(string refreshToken)
        {
            var result = await identityService.RevokeToken(refreshToken);
            if (!result.success)
                return BadRequest(result);
            return Ok(result);
        }
    }
}
