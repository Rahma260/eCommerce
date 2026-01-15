using eCommerce.Application.ExternalServices.Interfaces.Cloudinary;
using eCommerce.Application.Services.Interfaces;
using Microsoft.AspNetCore.Http;


namespace eCommerce.Infrastructure.Repositories.Image
{    public class CloudinaryImageManager : IImageManager
    {
        private readonly ICloudinaryService _cloudinary;

        public CloudinaryImageManager(ICloudinaryService cloudinary)
        {
            _cloudinary = cloudinary;
        }


        public async Task<Domain.Entities.Image> UploadAsync(IFormFile file, string folder)
        {
            using var ms = new MemoryStream();
            await file.CopyToAsync(ms);

            var (url, publicId) =
                await _cloudinary.UploadFileAsync(ms.ToArray(), file.FileName, folder);

            var image = new Domain.Entities.Image
            {
                Url = url,
                PublicId = publicId
            };
            return image;
        }

        public async Task DeleteAsync(Domain.Entities.Image image)
        {
            await _cloudinary.DeleteFileAsync(image.PublicId);
        }
    }


}
