using AutoMapper;
using eCommerce.Application.DTOs;
using eCommerce.Application.DTOs.Responses;
using eCommerce.Application.DTOs.User;
using eCommerce.Application.Services.Interfaces;
using eCommerce.Application.Services.Interfaces.Logging;
using eCommerce.Application.Services.Interfaces.Profile;
using eCommerce.Application.Validators.ValidationService.ValidationService;
using eCommerce.Domain.Entities.Identity;
using eCommerce.Domain.Interfaces;
using eCommerce.Domain.Interfaces.Identity;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.Application.Services.Implementations.Profile
{
    internal class ProfileService(
        IUserManagement userManagement,
        IAppLogger<ProfileService> logger,
        IMapper mapper,
        IUnitOfWork unitOfWork,
        IValidator<ChangePasswordDto> changePasswordValidator,
        IValidator<UserDto> updateUserValidator,
        IImageManager imageManager,
        IValidationService validationService) : IProfileService
    {

        public async Task<ServiceResponse> ChangePassword(ChangePasswordDto changePasswordDto)
        {
            var validationResult = await validationService.ValidateAsync(changePasswordDto, changePasswordValidator);

            var user = userManagement.GetUserByEmail(changePasswordDto.Email!);
            if (user == null)
                return new ServiceResponse { success = false, message = "User not found." };

            if (validationResult.success)
            {
                await userManagement.UpdateAsyncasswordHash(await user!, changePasswordDto.CurrentPassword!, changePasswordDto.NewPassword!);
                return new ServiceResponse { success = true, message = "Password changed successfully." };
            }
            else
            {
                logger.LogError("Password change validation failed", new Exception(string.Join(", ", validationResult.message!)));
                return new ServiceResponse { success = false, message = "Error occured during changing password." };
            }

        }

        public async Task<ServiceResponse> DeleteUser(string email)
        {
            var user = await userManagement.GetUserByEmail(email);
            if (user == null)
                return new ServiceResponse { success = false, message = "User not found." };
            await imageManager.DeleteAsync(user.Image!);
            await userManagement.RemoveUserByEmail(email);
            return new ServiceResponse { success = true, message = "User deleted successfully." };
        }

        public async Task<IEnumerable<UserDto>> GetAllUsers()
        {
            var mappedData = mapper.Map<IEnumerable<UserDto>>(await userManagement.GetAllUsers());
            if (!mappedData.Any())
                return Enumerable.Empty<UserDto>();
            return mappedData;
        }

        public async Task<UserDto?> GetUserByEmail(string email)
        {
            var mappedData = mapper.Map<UserDto>(await userManagement.GetUserByEmail(email));
            if (mappedData == null)
                return null;
            return mappedData;
        }

        public async Task<ServiceResponse> UpdateImage(
        string userId,
        IFormFile newImage
    )
        {
            if (newImage == null || newImage.Length == 0)
                return new ServiceResponse(false, "Invalid image file");

            var user = await unitOfWork.Users.GetUserWithImageAsync(userId);
            if (user == null)
                return new ServiceResponse(false, "User not found");

            Domain.Entities.Image uploadedImage;
            try
            {
                uploadedImage = await imageManager.UploadAsync(newImage, "ECommerce/Users");
            }
            catch (Exception ex)
            {
                logger.LogError("Failed to upload new image", ex);
                return new ServiceResponse(false, "Error uploading new image");
            }
            if (user.Image != null)
            {
                try
                {
                    await imageManager.DeleteAsync(user.Image);
                    await unitOfWork.Images.DeleteAsync(user.Image.Id);
                }
                catch (Exception ex)
                {
                    logger.LogError("Failed to delete old image", ex);
                }
            }
            uploadedImage.UserId = user.Id;

            user.Image = uploadedImage;

            await unitOfWork.SaveAsync();

            return new ServiceResponse(true, "Image updated successfully");
        }

        public async Task<ServiceResponse> UpdateUser(UserDto userBaseDto)
        {
            var validationResult = await validationService.ValidateAsync(userBaseDto, updateUserValidator);
            if (!validationResult.success)
                return validationResult;

            var existingUser = await userManagement.GetUserByEmail(userBaseDto.Email);
            if (existingUser == null)
                return new ServiceResponse(false, "User not found");

            mapper.Map(userBaseDto, existingUser);

            await unitOfWork.SaveAsync();

            return new ServiceResponse(true, "User updated successfully");
        }

    }
}
