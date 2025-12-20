using System.ComponentModel.DataAnnotations;

namespace eCommerce.Application.DTOs.Category
{
    public class UpdateCategoryDto 
    {
        public int Id { get; set; }
        public string? Name { get; set; }
    }
}
