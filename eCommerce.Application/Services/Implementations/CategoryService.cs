using AutoMapper;
using eCommerce.Application.DTOs;
using eCommerce.Application.DTOs.Category;
using eCommerce.Application.DTOs.Product;
using eCommerce.Application.Services.Interfaces;
using eCommerce.Domain.Entities;
using eCommerce.Domain.Interfaces;

namespace eCommerce.Application.Services.Implementations
{
    public class CategoryService(IUnitOfWork unitOfWork, IMapper mapper) : ICategoryService
    {
        public async Task<ServiceResponse> AddAsync(CreateCategoryDto entity)
        {

            var mappedDate = mapper.Map<Category>(entity);
            int result = await unitOfWork.Categories.AddAsync(mappedDate);
            if (result > 0)
                return new ServiceResponse(true, "Category added successfully");
            return new ServiceResponse(false, "Category failed to be added.");
        }

        public async Task<ServiceResponse> DeleteAsync(int id)
        {
            int result = await unitOfWork.Categories.DeleteAsync(id);
            if (result > 0)
                return new ServiceResponse(true, "Category deleted successfully");
            //it is perefered not to specify the exact reason for security
            return new ServiceResponse(false, "Category not found or failed to be deleted.");
        }

        public async Task<IEnumerable<CategoryBaseDto>> GetAllAsync()
        {
            var rawData = await unitOfWork.Categories.GetAllAsync();
            //map the (destination)result(Category) to (source)CategoryBaseDto
            if (!rawData.Any())
                //return empty list if there is no Categorys
                return Enumerable.Empty<CategoryBaseDto>();
            var mappedData = mapper.Map<IEnumerable<GetCategoryDto>>(rawData);
            return mappedData;

        }

        public async Task<GetCategoryDto?> GetByIdAsync(int id)
        {
            var rawData = await unitOfWork.Categories.GetByIdAsync(id);
            if (rawData == null)
                return null;
            return mapper.Map<GetCategoryDto>(rawData);
        }

        public async Task<ServiceResponse> UpdateAsync(UpdateCategoryDto entity)
        {
            //mapping the source entity to destination Category
            var mappedDate = mapper.Map<Category>(entity);
            int result = await unitOfWork.Categories.UpdateAsync(mappedDate);
            if (result > 0)
                return new ServiceResponse(true, "Category updated successfully");
            return new ServiceResponse(false, "Category failed to be updated.");
        }
    }
}
