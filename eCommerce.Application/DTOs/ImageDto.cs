using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.Application.DTOs
{
    public class UpdateImageDto
    {
        public IFormFile Image { get; set; } = null!;
       
    }
}
