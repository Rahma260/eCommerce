using AutoMapper;
using eCommerce.Application.DTOs;
using eCommerce.Application.DTOs.Product;
using eCommerce.Application.Services.Interfaces;
using eCommerce.Domain.Entities;
using eCommerce.Domain.Interfaces;

namespace eCommerce.Application.Services.Implementations
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ServiceResponse> AddAsync(CreateProductDto entity)
        {
            var mappedData = _mapper.Map<Product>(entity);

            await _unitOfWork.Products.AddAsync(mappedData);
            await _unitOfWork.SaveAsync();

            return new ServiceResponse(true, "Product added successfully");
        }

        public async Task<ServiceResponse> DeleteAsync(int id)
        {
            var result = await _unitOfWork.Products.DeleteAsync(id);
            if (result == 0)
                return new ServiceResponse(false, "Product not found");

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
            var mappedData = _mapper.Map<Product>(entity);

            var result = await _unitOfWork.Products.UpdateAsync(mappedData);
            if (result == 0)
                return new ServiceResponse(false, "Product not found");

            await _unitOfWork.SaveAsync();
            return new ServiceResponse(true, "Product updated successfully");
        }
    }

}
