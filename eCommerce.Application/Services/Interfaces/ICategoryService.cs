using eCommerce.Application.DTOs.Category;
using eCommerce.Application.DTOs.Responses;

namespace eCommerce.Application.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryBaseDto>> GetAllAsync();
        Task<GetCategoryDto?> GetByIdAsync(int id);
        Task<ServiceResponse> AddAsync(CreateCategoryDto entity);
        Task<ServiceResponse> UpdateAsync(UpdateCategoryDto entity);
        Task<ServiceResponse> DeleteAsync(int id);
    }
}

