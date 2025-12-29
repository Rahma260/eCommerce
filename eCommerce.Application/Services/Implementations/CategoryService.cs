using AutoMapper;
using eCommerce.Application.DTOs;
using eCommerce.Application.DTOs.Category;
using eCommerce.Application.DTOs.Product;
using eCommerce.Application.DTOs.User;
using eCommerce.Application.Services.Interfaces;
using eCommerce.Application.Validators;
using eCommerce.Domain.Entities;
using eCommerce.Domain.Entities.Identity;
using eCommerce.Domain.Interfaces;
using FluentValidation;

namespace eCommerce.Application.Services.Implementations
{
    public class CategoryService(IUnitOfWork _unitOfWork, IMapper _mapper, IValidator<CreateCategoryDto> createCategoryValidator,
        IValidator<UpdateCategoryDto> updateCategoryValidator,
        IValidationService validationService) : ICategoryService
    {   

        public async Task<ServiceResponse> AddAsync(CreateCategoryDto entity)
        {
            var validationResult = await validationService.ValidateAsync(entity, createCategoryValidator);
            if (!validationResult.success) return validationResult;

            var mappedData = _mapper.Map<Category>(entity);

            await _unitOfWork.Categories.AddAsync(mappedData);
            await _unitOfWork.SaveAsync();

            return new ServiceResponse(true, "Category added successfully");
        }

        public async Task<ServiceResponse> DeleteAsync(int id)
        {
            await _unitOfWork.Categories.DeleteAsync(id);
            //if (result == 0)
            //    return new ServiceResponse(false, "Category not found");

            await _unitOfWork.SaveAsync();
            return new ServiceResponse(true, "Category deleted successfully");
        }

        public async Task<IEnumerable<CategoryBaseDto>> GetAllAsync()
        {
            var rawData = await _unitOfWork.Categories.GetAllAsync();

            if (!rawData.Any())
                return Enumerable.Empty<CategoryBaseDto>();

            return _mapper.Map<IEnumerable<GetCategoryDto>>(rawData);
        }

        public async Task<GetCategoryDto?> GetByIdAsync(int id)
        {
            var rawData = await _unitOfWork.Categories.GetByIdAsync(id);
            if (rawData == null)
                return null;

            return _mapper.Map<GetCategoryDto>(rawData);
        }

        public async Task<ServiceResponse> UpdateAsync(UpdateCategoryDto entity)
        {
            var validationResult = await validationService.ValidateAsync(entity, updateCategoryValidator);
            if (!validationResult.success) return validationResult;

            var mappedData = _mapper.Map<Category>(entity);

            await _unitOfWork.Categories.UpdateAsync(mappedData);
            //if (result == 0)
            //    return new ServiceResponse(false, "Category not found");

            await _unitOfWork.SaveAsync();
            return new ServiceResponse(true, "Category updated successfully");
        }
    }
}
