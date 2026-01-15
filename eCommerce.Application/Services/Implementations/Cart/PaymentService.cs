using eCommerce.Application.DTOs.Cart;
using eCommerce.Application.DTOs.Responses;
using eCommerce.Application.Services.Interfaces.Cart;
using eCommerce.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.Application.Services.Implementations.Cart
{
    internal class PaymentService : IPaymentService
    {
        public Task<ServiceResponse> Pay(decimal totalAmount, IEnumerable<Product> cartProducts, IEnumerable<ProcessCart> carts)
        {
            throw new NotImplementedException();
        }
    }
}
