using eCommerce.Domain.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.Application.Services.Interfaces
{
    public interface IImageManager
    {
        Task<Image> UploadAsync(IFormFile file, string folder);
        Task DeleteAsync(Image image);
    }

}
