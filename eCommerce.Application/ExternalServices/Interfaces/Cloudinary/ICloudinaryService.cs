using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.Application.ExternalServices.Interfaces.Cloudinary
{
    public interface ICloudinaryService
    {
        Task<(string Url, string PublicId)> UploadFileAsync(byte[] fileBytes, string fileName, string folderName);
        Task<bool> DeleteFileAsync(string publicId);
        Task<byte[]> DownloadFileAsync(string publicId);
    }
}
