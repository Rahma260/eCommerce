using AutoMapper;
using eCommerce.Application.DTOs;
using eCommerce.Application.DTOs.Product;
using eCommerce.Application.Services.Interfaces;
using eCommerce.Domain.Entities;
using eCommerce.Domain.Interfaces;

namespace eCommerce.Application.Services.Implementations
{
    public class ProductService(IUnitOfWork unitOfWork, IMapper mapper) : IProductService
    {
        public async Task<ServiceResponse> AddAsync(CreateProductDto entity)
        {

            var mappedDate = mapper.Map<Product>(entity);
            int result = await unitOfWork.Products.AddAsync(mappedDate);
            if (result > 0)
                return new ServiceResponse(true, "Product added successfully");
            return new ServiceResponse(false, "Product failed to be added.");
        }

        public async Task<ServiceResponse> DeleteAsync(int id)
        {
            int result = await unitOfWork.Products.DeleteAsync(id);
            if (result > 0)
                return new ServiceResponse(true, "Product deleted successfully");
            //it is perefered not to specify the exact reason for security
            return new ServiceResponse(false, "Product not found or failed to be deleted.");
        }

        public async Task<IEnumerable<ProductBaseDto>> GetAllAsync()
        {
            var rawData = await unitOfWork.Products.GetAllAsync();
            //map the (destination)result(product) to (source)ProductBaseDto
            if(!rawData.Any())
                //return empty list [] if there is no products
                return Enumerable.Empty<ProductBaseDto>();
            var mappedData = mapper.Map<IEnumerable<GetProductDto>>(rawData);
            return mappedData;

        }

        public async Task<GetProductDto?> GetByIdAsync(int id)
        {
            var rawData = await unitOfWork.Products.GetByIdAsync(id);
            if(rawData == null)
                return null;
            return mapper.Map<GetProductDto>(rawData);
        }

        public async Task<ServiceResponse> UpdateAsync(UpdateProductDto entity)
        {
            //mapping the source entity to destination product
            var mappedDate = mapper.Map<Product>(entity);
            int result = await unitOfWork.Products.UpdateAsync(mappedDate);
            if (result > 0)
                return new ServiceResponse(true, "Product updated successfully");
            return new ServiceResponse(false, "Product failed to be updated.");
        }
    }
}
