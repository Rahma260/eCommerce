using eCommerce.Application.DTOs.User;
using eCommerce.Application.DTOs.User.GoogleUser;
using eCommerce.Application.Services.Implementations.Authentication;
using eCommerce.Application.Services.Implementations.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace eCommerce.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(Application.Services.Interfaces.Identity.IAuthenticationService identityService, GoogleLoginHandler handler) : ControllerBase
    {
        [HttpPost("google")]
        public async Task<IActionResult> GoogleLogin([FromBody] GoogleLoginRequestDto request)
        {
            var result = await handler.HandleAsync(request.IdToken);
            return Ok(result);
        }

        [HttpGet("google-login")]
        public IActionResult GoogleLogin()
        {
            var redirectUrl = "https://localhost:7221/api/Auth/google-callback";
            var properties = new AuthenticationProperties { RedirectUri = redirectUrl };
            return Challenge(properties, GoogleDefaults.AuthenticationScheme);
        }

        [HttpGet("google-callback")]
        public async Task<IActionResult> GoogleCallback()
        {
            var authenticateResult =
       await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            Console.WriteLine($"Succeeded: {authenticateResult.Succeeded}");

            if (authenticateResult.Properties != null)
            {
                foreach (var item in authenticateResult.Properties.Items)
                    Console.WriteLine($"PROP {item.Key}: {item.Value}");

                foreach (var token in authenticateResult.Properties.GetTokens())
                    Console.WriteLine($"TOKEN {token.Name}: {token.Value}");
            }

            if (!authenticateResult.Succeeded)
                return BadRequest("Authentication failed");

            // Extract ID token from properties (not claims)
            var idToken = authenticateResult.Properties.GetTokenValue("id_token");
            if (string.IsNullOrEmpty(idToken))
                return BadRequest("No ID token");

            // Now use the ID token in your GoogleLoginHandler
            var loginResult = await handler.HandleAsync(idToken);
            return Ok(loginResult);
        }

        [HttpPost("create-account")]
        public async Task<IActionResult> Register([FromForm] CreateUserDto user)
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
       

        [HttpGet("forget-password")]
        public async Task<IActionResult> ForgetPassword([FromQuery] ForgetPasswordDto forgetPasswordDto)
        {
            var result = await identityService.ForgetPassword(forgetPasswordDto);
            if (!result.success)
                return BadRequest(result);
            return Ok(result);
        }
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordDto resetPasswordDto)
        {
            var result = await identityService.ResetPassword(resetPasswordDto);
            if (!result.success)
                return BadRequest(result);
            return Ok(result);
        }
    }
}
