using AutoMapper;
using eCommerce.Application.DTOs.Category;
using eCommerce.Application.DTOs.Product;
using eCommerce.Domain.Entities;

namespace eCommerce.Application.Mapping
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {
            //<source, destination>
            CreateMap<CreateCategoryDto, Category>();
            CreateMap<CreateProductDto, Product>();

            CreateMap<Category, GetCategoryDto>();
            CreateMap<Product, GetProductDto>();

            CreateMap<UpdateCategoryDto, Category>();
            CreateMap<UpdateProductDto, Product>();

        }
    }
}
