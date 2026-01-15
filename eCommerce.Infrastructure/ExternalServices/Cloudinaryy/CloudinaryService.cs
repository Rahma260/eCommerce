using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using eCommerce.Application.DTOs;
using eCommerce.Application.ExternalServices.Interfaces.Cloudinary;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

using ResourceType = CloudinaryDotNet.Actions.ResourceType;

namespace eCommerce.Infrastructure.ExternalServices.Cloudinaryy
{
    public class CloudinaryService : ICloudinaryService
    {
        private readonly Cloudinary _cloudinary;
        private readonly ILogger<CloudinaryService> _logger;
        public CloudinaryService(IOptions<CloudinarySettings> config, ILogger<CloudinaryService> logger)
        {
            var account = new Account(
                config.Value.CloudName,
                config.Value.ApiKey,
                config.Value.ApiSecret);

            _cloudinary = new Cloudinary(account);
            _logger = logger;
        }
       // public Api Api => _cloudinary.Api;
        public async Task<(string Url, string PublicId)> UploadFileAsync(byte[] fileBytes, string fileName, string folderName)
        {
            if (fileBytes == null || fileBytes.Length == 0)
                throw new ArgumentException("Invalid file data.");

            using var stream = new MemoryStream(fileBytes);

            var uploadParams = new RawUploadParams
            {
                File = new FileDescription(fileName, stream),
                PublicId = $"{Guid.NewGuid()}_{Path.GetFileNameWithoutExtension(fileName)}",
                Overwrite = true,
                UseFilename = true,
                UniqueFilename = false,
                Folder = folderName,
            };

            var uploadResult = await _cloudinary.UploadAsync(uploadParams);

            if (uploadResult.Error != null)
                _logger.LogError($"Cloudinary upload error: {uploadResult.Error.Message}");

            return (uploadResult.SecureUrl.AbsoluteUri, uploadResult.PublicId);
        }

        public async Task<bool> DeleteFileAsync(string publicId)
        {
            var deletionParams = new DeletionParams(publicId) { ResourceType = ResourceType.Raw };
            var result = await _cloudinary.DestroyAsync(deletionParams);

            _logger.LogInformation($"Cloudinary Deletion Response: {JsonConvert.SerializeObject(result)}");

            return result.Result == "ok";
        }

        public async Task<byte[]> DownloadFileAsync(string publicId)
        {
            if (string.IsNullOrEmpty(publicId))
                throw new ArgumentException("Public ID cannot be null or empty.");

            var getParams = new GetResourceParams(publicId)
            {
                ResourceType = ResourceType.Raw
            };
            var resource = await _cloudinary.GetResourceAsync(getParams);
            if (resource == null || string.IsNullOrEmpty(resource.SecureUrl))
                throw new Exception("File not found on Cloudinary.");

            using var httpClient = new HttpClient();
            return await httpClient.GetByteArrayAsync(resource.SecureUrl);
        }
    }
}
