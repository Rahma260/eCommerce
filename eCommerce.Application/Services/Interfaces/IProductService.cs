using eCommerce.Application.DTOs;
using eCommerce.Application.DTOs.Product;

namespace eCommerce.Application.Services.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<ProductBaseDto>> GetAllAsync();
        Task<GetProductDto?> GetByIdAsync(int id);
        Task<ServiceResponse> AddAsync(CreateProductDto entity);
        Task<ServiceResponse> UpdateAsync(UpdateProductDto entity);
        Task<ServiceResponse> DeleteAsync(int id);
    }
}

