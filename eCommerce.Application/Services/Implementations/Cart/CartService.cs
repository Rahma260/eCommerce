using AutoMapper;
using eCommerce.Application.DTOs.Cart;
using eCommerce.Application.DTOs.Responses;
using eCommerce.Application.Services.Interfaces.Cart;
using eCommerce.Domain.Entities;
using eCommerce.Domain.Interfaces;
using eCommerce.Domain.Interfaces.Cart;
using Stripe.Climate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.Application.Services.Implementations.Cart
{
    public class CartService(
        ICart cart,
        IMapper mapper,
        IPaymentMethodService paymentMethodService,
        IPaymentService paymentService,
        IUnitOfWork unitOfWork) : ICartService
    {
        public async Task<ServiceResponse> Checkout(DTOs.Cart.CheckoutDto checkout)
        {
            var(products, totalAmount) = await GetTotalAmount(checkout.Carts);
            var paymentMethods = await paymentMethodService.GetPaymentMethods();
            if(checkout.PaymentMethodId == paymentMethods.FirstOrDefault()!.Id)
                return await paymentService.Pay(totalAmount, products, checkout.Carts);
          
            return new ServiceResponse { success = false, message = "invalid method" };
        }

        public async Task<ServiceResponse> SaveCheckoutHistory(IEnumerable<CreateCheckoutDto> orders)
        {
            var mappedData = mapper.Map<IEnumerable<DTOs.Cart.CheckoutDto>>(orders);
            var result = await cart.SaveCheckoutHistory((IEnumerable<Domain.Entities.Checkout>)mappedData);
            return result > 0 ? new ServiceResponse { success = true, message = "Order placed successfully" } :
                new ServiceResponse { message = "Failed to place order" };
        }
        private async Task<(IEnumerable<Domain.Entities.Product>, decimal)> GetTotalAmount(IEnumerable<ProcessCart> carts)
        {
            if (!carts.Any()) return ([], 0);
            var products = await unitOfWork.Products.GetAllAsync();
            if (!products.Any()) return ([], 0);

            var cartProducts = carts
                .Join(products,
                    cart => cart.ProductId,
                    product => product.Id,
                    (cart, product) => new { cart, product })
                .ToList();

            var totalAmount = cartProducts
                .Sum(x => x.cart.Quantity * (x.product.Price ?? 0m));

            return (cartProducts.Select(x => x.product), totalAmount);
        }
    }
}
