using AutoMapper;
using eCommerce.Application.DTOs.Responses;
using eCommerce.Application.DTOs.User;
using eCommerce.Application.ExternalServices.Interfaces.Jobs;
using eCommerce.Application.Services.Interfaces;
using eCommerce.Application.Services.Interfaces.Identity;
using eCommerce.Application.Services.Interfaces.Logging;
using eCommerce.Application.Validators.ValidationService.ValidationService;
using eCommerce.Domain.Entities.Identity;
using eCommerce.Domain.Interfaces.eCommerce.Domain.Interfaces;
using eCommerce.Domain.Interfaces.Identity;
using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace eCommerce.Application.Services.Implementations.Identity
{
    public class AuthenticationService(
        ITokenManagement tokenManagement,
        IUserManagement userManagement,
        IRoleManagement roleManagement,
        IAppLogger<AuthenticationService> logger,
        IMapper mapper,
        IValidator<CreateUserDto> createUserValidator,
        IValidator<LoginUserDto> loginUserValidator,
        IValidationService validationService,
        IImageManager _imageManager,
        IBackgroundJobService backgroundJobService,
        IPasswordResetOtpCache passwordResetOtpCache
        ) : IAuthenticationService
    {

        public async Task<ServiceResponse> CreateUser(CreateUserDto user)
        {
            var validationResult = await validationService.ValidateAsync(user, createUserValidator);
            if (!validationResult.success) return validationResult;
            
            var mappedModel = mapper.Map<User>(user);
            await HandleImageUpdate(mappedModel, user.Image);
            //map it because their name in user class is different
            mappedModel.UserName = user.Email;
            mappedModel.PasswordHash = user.Password;

            var result = await userManagement.CreateUser(mappedModel);
            if (!result)
                return new ServiceResponse { message = "Email address might be already in use or unknown error occured" };

            var _user = await userManagement.GetUserByEmail(user.Email);
            var users = await userManagement.GetAllUsers();
            bool assignedResult = await roleManagement.AddUserToRole(_user!, users!.Count() > 1 ? "User" : "Admin");

            if(!assignedResult)
            {
                bool removeUserResult = await userManagement.RemoveUserByEmail(_user!.Email!);
                if (!removeUserResult)
                {
                    logger.LogError(
                        "User could not be assigned role",
                        new Exception($"User with email as {_user.Email} failed to be remove as a result of role assignment failure")
                        ); 
                    return new ServiceResponse { message = "Error occured in creating account." };
                }
            }
            backgroundJobService.EnqueueEmail(
                user.Email,
                "Welcome 🎉",
                "<h2>Welcome to eCommerce</h2>"
            );

            return new ServiceResponse { success = true, message = "Account created!" };

        }
        public async Task<ForgetPasswordResponse> ForgetPassword(ForgetPasswordDto dto)
        {
            var user = await userManagement.GetUserByEmail(dto.email);

            if (user == null)
                return new ForgetPasswordResponse { success = false };

            // generate otp
            var otp = GenerateOTP();
            //set 5 minutes for expiration
            await passwordResetOtpCache.SetAsync(
                user.Email!,
                otp,
                TimeSpan.FromMinutes(5)
            );

            backgroundJobService.EnqueueEmail(
                user.Email!,
                "Reset Password OTP",
                $"<h2>Your OTP</h2><b>{otp}</b>"
            );

            return new ForgetPasswordResponse { success = true };
        }
        private string GenerateOTP() => new Random().Next(100000, 999999).ToString();
        public async Task<LoginResponse> LoginUser(LoginUserDto user)
        {
            var validationResult = await validationService.ValidateAsync(user, loginUserValidator);
            if (!validationResult.success) 
                return new LoginResponse { message = validationResult .message};

            var mappedModel = mapper.Map<User>(user);
            //map it because their name in user class is different
            mappedModel.PasswordHash = user.Password;

            bool loginResult = await userManagement.LoginUser(mappedModel);
            if (!loginResult)
                return new LoginResponse { message = "Email not found or invalid credentials" };
            
            var _user = await userManagement.GetUserByEmail(user.Email);
            var claims = await userManagement.GetUserClaims(_user!.Email!);

            string token = tokenManagement.GenerateToken(claims);
            string refreshToken = tokenManagement.GetRefreshToken();

            int saveTokenResult = 0;
            bool userTokenCheck = await tokenManagement.ValidateRefreshToken(refreshToken);
            if(userTokenCheck)
                saveTokenResult = await tokenManagement.UpdateRefreshToken(_user.Id, refreshToken);
            saveTokenResult = await tokenManagement.AddRefreshToken(refreshToken, _user.Id);
            return saveTokenResult <= 0 ?
                new LoginResponse { message = "Error occured while logging in." } :
                new LoginResponse { success = true, token = token, refreshToken = refreshToken};

        }

        public async Task<ServiceResponse> ResetPassword(ResetPasswordDto dto)
        {
            var user = await userManagement.GetUserByEmail(dto.email);
            if (user == null)
                return new ServiceResponse { success = false, message = "User not found" };

            // validate OTP from cache (single source of truth)
            var isValidOtp = await passwordResetOtpCache.ValidateOtpAsync(
                user.Email!,
                dto.otp
            );

            if (!isValidOtp)
                return new ServiceResponse { success = false, message = "Invalid or expired OTP" };

            // Generate reset token internally to be sent for ResetPasswordAsync method
            var token = await userManagement.GenerateResetPasswordToken(user);

            var result = await userManagement.ResetPasswordAsync(
                user,
                token,
                dto.newPassword
            );

            if (!result.Succeeded)
                return new ServiceResponse
                {
                    success = false,
                    message = string.Join(" | ", result.Errors.Select(e => e.Description))
                };

            await passwordResetOtpCache.RemoveAsync(user.Email!);

            return new ServiceResponse
            {
                success = true,
                message = "Password reset successfully"
            };
        }

        public async Task<LoginResponse> RevokeToken(string refreshToken)
        {
            bool validateTokenResult = await tokenManagement.ValidateRefreshToken(refreshToken);
            if (!validateTokenResult)
                return new LoginResponse { message = "Invalid token" };
            var userId = await tokenManagement.GetUserIdByRefreshToken(refreshToken);
            User? user = await userManagement.GetUserById(userId);
            var claims = await userManagement.GetUserClaims(user!.Email!);
            string Token = tokenManagement.GenerateToken(claims);
            string newRefreshToken = tokenManagement.GetRefreshToken();
            await tokenManagement.UpdateRefreshToken(userId, newRefreshToken);
            return new LoginResponse
            {
                success = true,
                token = Token,
                refreshToken = newRefreshToken
            };

        }
        private async Task HandleImageUpdate(User user, IFormFile? image)
        {
            if (image == null || image.Length == 0)
                return;

            if (user.Image != null)
                await _imageManager.DeleteAsync(user.Image);

            user.Image = await _imageManager.UploadAsync(image, "ECommerce/Users");

        }

    }
}
