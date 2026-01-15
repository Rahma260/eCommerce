using AutoMapper;
using eCommerce.Application.DTOs;
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

            CreateMap<CreateUserDto, User>()
                 .ForMember(dest => dest.Image, opt => opt.Ignore());
                // .ForMember(dest => dest.ImageId, opt => opt.Ignore());


            CreateMap<LoginUserDto, User>();
            CreateMap<User, UserDto>().ReverseMap();
         //  CreateMap<UserDto, User>();

            CreateMap<Address, AddressBaseDto>();
            CreateMap<PaymentMethod, GetPaymentMethod>();
            CreateMap<CreateCheckoutDto, Domain.Entities.Checkout>();

 //           CreateMap<ImageDto, Image>().ReverseMap();
        }
    }
}
