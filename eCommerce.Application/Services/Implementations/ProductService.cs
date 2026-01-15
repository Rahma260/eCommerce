using AutoMapper;
using eCommerce.Application.DTOs.Category;
using eCommerce.Application.DTOs.Product;
using eCommerce.Application.DTOs.Responses;
using eCommerce.Application.Services.Interfaces;
using eCommerce.Application.Validators.ValidationService.ValidationService;
using eCommerce.Domain.Entities;
using eCommerce.Domain.Interfaces;
using FluentValidation;

namespace eCommerce.Application.Services.Implementations
{
    public class ProductService(IUnitOfWork _unitOfWork, IMapper _mapper, IValidator<CreateProductDto> createProductValidator,
        IValidator<UpdateProductDto> updateProductValidator,
        IValidationService validationService) : IProductService
    {
        public async Task<ServiceResponse> AddAsync(CreateProductDto entity)
        {
            var validationResult = await validationService.ValidateAsync(entity, createProductValidator);
            if (!validationResult.success) return validationResult;
            
            var mappedData = _mapper.Map<Product>(entity);

            await _unitOfWork.Products.AddAsync(mappedData);
            await _unitOfWork.SaveAsync();

            return new ServiceResponse(true, "Product added successfully");
        }

        public async Task<ServiceResponse> DeleteAsync(int id)
        {
            await _unitOfWork.Products.DeleteAsync(id);
            //if (result == 0)
            //    return new ServiceResponse(false, "Product not found");

            await _unitOfWork.SaveAsync();
            return new ServiceResponse(true, "Product deleted successfully");
        }

        public async Task<IEnumerable<ProductBaseDto>> GetAllAsync()
        {
            var rawData = await _unitOfWork.Products.GetAllAsync();

            if (!rawData.Any())
                return Enumerable.Empty<ProductBaseDto>();

            return _mapper.Map<IEnumerable<GetProductDto>>(rawData);
        }

        public async Task<GetProductDto?> GetByIdAsync(int id)
        {
            var rawData = await _unitOfWork.Products.GetByIdAsync(id);
            if (rawData == null)
                return null;

            return _mapper.Map<GetProductDto>(rawData);
        }

        public async Task<ServiceResponse> UpdateAsync(UpdateProductDto entity)
        {
            var validationResult = await validationService.ValidateAsync(entity, updateProductValidator);
            if (!validationResult.success) return validationResult;

            var existingProduct = await _unitOfWork.Products.GetByIdAsync(entity.Id);

            if (existingProduct == null)
                return new ServiceResponse(false, "Product not found");
          
            _mapper.Map(entity, existingProduct);

            await _unitOfWork.SaveAsync();
            return new ServiceResponse(true, "Product updated successfully");
        }
    }

}
