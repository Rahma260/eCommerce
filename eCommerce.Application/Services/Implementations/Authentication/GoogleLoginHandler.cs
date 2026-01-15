using eCommerce.Application.DTOs.Responses;
using eCommerce.Application.Services.Interfaces.Identity;
using eCommerce.Domain.Entities.Identity;
using eCommerce.Domain.Interfaces.Identity;
using Google.Apis.Auth;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.Application.Services.Implementations.Authentication
{
    public class GoogleLoginHandler
    {
        private readonly IGoogleAuthService googleAuthService;
        private readonly IUserManagement userRepository;
        private readonly ITokenManagement tokenService;
        private readonly IConfiguration configuration;
        public GoogleLoginHandler(
            IGoogleAuthService googleAuthService,
            IUserManagement userRepository,
            ITokenManagement tokenService,
            IConfiguration configuration)
        {
            this.googleAuthService = googleAuthService;
            this.userRepository = userRepository;
            this.tokenService = tokenService;
            this.configuration = configuration;
        }

        //public async Task<LoginResponse> HandleAsync(string idToken)
        //{
        //    var googleUser = await googleAuthService.ValidateTokenAsync(idToken); if (googleUser == null) { return new LoginResponse { message = "Invalid Google token." }; }
        //    var user = await userRepository.GetUserByEmail(googleUser.Email); if (user == null)
        //    {
        //        user = User.CreateGoogleUser(googleUser.Email, googleUser.Name, googleUser.Picture, googleUser.Sub // Pass the sub
        //          ); 
        //        await userRepository.CreateUser(user);
        //    } 
        //    var _user = await userRepository.GetUserByEmail(user.Email);
        //    if (_user == null) 
        //    { 
        //        return new LoginResponse 
        //        { 
        //            message = "User creation failed." 
        //        }; 
        //    } 
        //    var claims = await userRepository.GetUserClaims(_user.Email!); 
        //    if (claims == null) 
        //    {
        //        return new LoginResponse 
        //        {
        //            message = "Failed to retrieve user claims."
        //        };
        //    } 
        //    string token = tokenService.GenerateToken(claims); 
        //    string refreshToken = tokenService.GetRefreshToken();
        //    int saveTokenResult = 0; 
        //    bool userTokenCheck = await tokenService.ValidateRefreshToken(refreshToken); 
        //    if (userTokenCheck) saveTokenResult = await tokenService.UpdateRefreshToken(_user.Id, refreshToken); 
        //    else saveTokenResult = await tokenService.AddRefreshToken(refreshToken, _user.Id); 
        //    return saveTokenResult <= 0 
        //        ? new LoginResponse { message = "Error occurred while logging in." } 
        //        : new LoginResponse { success = true, token = token, refreshToken = refreshToken }; 
        //}
        public async Task<LoginResponse> HandleAsync(string idToken)
        {
            var googleUser = await googleAuthService.ValidateTokenAsync(idToken);
            if (googleUser == null)
                return new LoginResponse { message = "Invalid Google token." };

            var user = await userRepository.GetUserByEmail(googleUser.Email);
            if (user == null)
            {
                user = User.CreateGoogleUser(googleUser.Email, googleUser.Name, googleUser.Picture, googleUser.Sub);
                await userRepository.CreateUser(user);
            }

            var _user = await userRepository.GetUserByEmail(user.Email);
            if (_user == null)
                return new LoginResponse { message = "User creation failed." };

            var claims = await userRepository.GetUserClaims(_user.Email!);
            if (claims == null)
                return new LoginResponse { message = "Failed to retrieve claims." };

            string token = tokenService.GenerateToken(claims);
            string refreshToken = tokenService.GetRefreshToken();

            int saveTokenResult = await tokenService.AddRefreshToken(refreshToken, _user.Id);
            if (await tokenService.ValidateRefreshToken(refreshToken))
                saveTokenResult = await tokenService.UpdateRefreshToken(_user.Id, refreshToken);

            return saveTokenResult <= 0 ?
                new LoginResponse { message = "Error occurred while logging in." } :
                new LoginResponse { success = true, token = token, refreshToken = refreshToken };
        }
    }
}
