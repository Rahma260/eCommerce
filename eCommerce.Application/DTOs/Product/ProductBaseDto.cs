using System.ComponentModel.DataAnnotations;

namespace eCommerce.Application.DTOs.Product
{
    public class ProductBaseDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string Image { get; set; }
        public int Quantity { get; set; }
        public DateTime CreatedAt { get; set; }
        public int CategoryId { get; set; }
    }
}
