using AutoMapper;
using eCommerce.Application.DTOs.Cart;
using eCommerce.Application.DTOs.Category;
using eCommerce.Application.DTOs.Product;
using eCommerce.Application.DTOs.User;
using eCommerce.Domain.Entities;
using eCommerce.Domain.Entities.Identity;

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

            CreateMap<CreateUserDto, User>();
            CreateMap<LoginUserDto, User>();

            CreateMap<Address, AddressBaseDto>();
            CreateMap<PaymentMethod, GetPaymentMethod>();
            CreateMap<CreateOrderDto, Order>();
        }
    }
}
