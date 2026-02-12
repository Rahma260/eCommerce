using eCommerce.Application.DTOs;
using eCommerce.Application.DTOs.Cart;
using eCommerce.Application.DTOs.Responses;

namespace eCommerce.Application.Services.Interfaces.Cart
{
    public interface ICartService
    {
        Task<ServiceResponse> AddToCartAsync(AddToCartDto dto);
        Task<CartDto?> GetCartAsync(string userId);
        Task<ServiceResponse> UpdateCartAsync(AddToCartDto dto);
        Task<ServiceResponse> RemoveFromCartAsync(string userId, int productId);
        Task<ServiceResponse> Checkout(string userId, int paymentMethodId);
        Task<IEnumerable<OrderDto>> GetUserOrdersAsync(string userId);
        Task<OrderDto?> GetOrderByIdAsync(string userId, int orderId);
        Task CompleteOrderAfterPaymentAsync(string userId, int orderId);
        Task<ServiceResponse> SaveCheckoutAsync(SaveCheckoutDto dto);
    }

}
