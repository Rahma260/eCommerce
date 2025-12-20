using eCommerce.Application.DTOs.Category;

namespace eCommerce.Application.DTOs.Product
{
    public class GetProductDto : ProductBaseDto
    {
        public int Id { get; set; }
        public GetCategoryDto Category { get; set; }
    }
}
