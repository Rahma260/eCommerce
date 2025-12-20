using eCommerce.Application.DTOs.Product;

namespace eCommerce.Application.DTOs.Category
{
    public class GetCategoryDto : CategoryBaseDto
    {
        public int Id { get; set; }
        public ICollection<GetProductDto>? Products { get; set; }
    }
}
