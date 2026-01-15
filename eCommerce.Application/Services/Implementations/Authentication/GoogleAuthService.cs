using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eCommerce.Application.DTOs.User.GoogleUser;
using eCommerce.Application.Services.Interfaces.Identity;
using Google.Apis.Auth;
using Microsoft.Extensions.Configuration;
namespace eCommerce.Application.Services.Implementations.Identity
{

    public class GoogleAuthService : IGoogleAuthService
    {
        private readonly IConfiguration configuration;

        public GoogleAuthService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        //public async Task<GoogleUserInfoDto> ValidateTokenAsync(string idToken)
        //{
        //    var settings = new GoogleJsonWebSignature.ValidationSettings
        //    {
        //        Audience = new[] {  configuration["Authentication:Google:ClientId"], // your app client ID
        //"407408718192.apps.googleusercontent.com"       // OAuth Playground client ID
        //                                   } // OAuth Playground client ID
        //    };
        //    var payload = await GoogleJsonWebSignature.ValidateAsync(idToken, settings);
        //    return 
        //        new GoogleUserInfoDto 
        //        {
        //            Email = payload.Email,
        //            Name = payload.Name, // Null if not in
        //            Picture = payload.Picture, // Null if not in token
        //            Sub = payload.Subject 
        //        };
        //}
        public async Task<GoogleUserInfoDto> ValidateTokenAsync(string idToken)
        {
            try
            {
                var payload = await GoogleJsonWebSignature.ValidateAsync(idToken, new GoogleJsonWebSignature.ValidationSettings
                {
                    Audience = new[] { configuration["Authentication:Google:ClientId" ]}  // Must match config
                });

                return new GoogleUserInfoDto
                {
                    Email = payload.Email,
                    Name = payload.Name,  // Null if not in token
                    Picture = payload.Picture,  // Null if not in token
                    Sub = payload.Subject
                };
            }
            catch (Exception ex)
            {
                // Log the exception (e.g., using ILogger)
                Console.WriteLine($"Token validation failed: {ex.Message}");
                return null;
            }
        }
    }
}
