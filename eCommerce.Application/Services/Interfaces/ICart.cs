using eCommerce.Application.DTOs;
using eCommerce.Domain.Entities;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.Domain.Interfaces.Cart
{
    public interface ICart
    {
        Task<CartDto?> GetCartAsync(string userId);
        Task SaveCartAsync(CartDto cart, TimeSpan ttl);
        Task ClearCartAsync(string userId);
        //Task<int> SaveCheckoutHistory(IEnumerable<Checkout> checkouts);
    }
}
