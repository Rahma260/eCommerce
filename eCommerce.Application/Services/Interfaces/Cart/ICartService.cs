using eCommerce.Application.DTOs;
using eCommerce.Application.DTOs.Cart;

namespace eCommerce.Application.Services.Interfaces.Cart
{
    public interface ICartService
    {
        Task<ServiceResponse> SaveCheckoutHistory(IEnumerable<CreateOrderDto> orders);
        Task<ServiceResponse> Checkout(Checkout checkout);
    }
}
