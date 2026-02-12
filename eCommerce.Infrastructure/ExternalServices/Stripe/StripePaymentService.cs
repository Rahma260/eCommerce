using eCommerce.Application.DTOs;
using eCommerce.Application.DTOs.Cart;
using eCommerce.Application.DTOs.Responses;
using eCommerce.Application.Services.Interfaces.Cart;
using eCommerce.Domain.Entities;
using Stripe.Checkout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.Infrastructure.ExternalServices.Stripe
{
    public class StripePaymentService : IPaymentService
    {
        public async Task<ServiceResponse> Pay(decimal totalAmount, IEnumerable<Product> cartProducts, List<CartItemDto> carts)
        {
            try
            {
                var lineItems = new List<SessionLineItemOptions>();
                foreach (var item in cartProducts)
                {
                    var quantity = carts.FirstOrDefault(c => c.ProductId == item.Id)?.Quantity ?? 1;
                    lineItems.Add(new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            UnitAmount = (long)(item.Price! * 100),
                            Currency = "usd",
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = item.Name,
                                Description = item.Description
                            }
                        },
                        Quantity = quantity
                    });
                }
                var options = new SessionCreateOptions
                {
                    PaymentMethodTypes = new List<string> { "card" },
                    LineItems = lineItems,
                    Mode = "payment",
                    SuccessUrl = "https://localhost:7221/payment-success",
                    CancelUrl = "https://localhost:7221/payment-cancel",
                };
                var service = new SessionService();
                Session session = await service.CreateAsync(options);
                return new ServiceResponse
                {
                    success = true,
                    message = session.Url!
                };
            }
            catch (Exception ex)
            {
                return new ServiceResponse
                {
                    success = false,
                    message = ex.Message
                };
            }
        }
    }
}
