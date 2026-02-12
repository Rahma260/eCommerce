using eCommerce.Application.DTOs;
using eCommerce.Domain.Entities;
using eCommerce.Domain.Interfaces;
using eCommerce.Domain.Interfaces.Cart;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace eCommerce.Infrastructure.Repositories.Cart
{
    public class CartRepository : ICart
    {
        private readonly IDistributedCache _cache;
        private const string CartPrefix = "cart:";

        public CartRepository(IDistributedCache cache) => _cache = cache;

        private static string GetKey(string userId) => $"{CartPrefix}{userId}";

        public async Task<CartDto?> GetCartAsync(string userId)
        {
            var cartJson = await _cache.GetStringAsync(GetKey(userId));
            if (string.IsNullOrEmpty(cartJson)) return null;
            return JsonSerializer.Deserialize<CartDto>(cartJson);
        }

        public async Task SaveCartAsync(CartDto cart, TimeSpan ttl)
        {
            await _cache.SetStringAsync(
                GetKey(cart.UserId),
                JsonSerializer.Serialize(cart),
                new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = ttl });
        }

        public async Task ClearCartAsync(string userId)
        {
            await _cache.RemoveAsync(GetKey(userId));
        }
    }


}
