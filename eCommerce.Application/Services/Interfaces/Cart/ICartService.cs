using eCommerce.Application.DTOs.Cart;
using eCommerce.Application.DTOs.Responses;

namespace eCommerce.Application.Services.Interfaces.Cart
{
    public interface ICartService
    {
        Task<ServiceResponse> SaveCheckoutHistory(IEnumerable<CreateCheckoutDto> orders);
        Task<ServiceResponse> Checkout(CheckoutDto checkout);
    }
}
