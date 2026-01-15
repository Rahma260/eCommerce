using eCommerce.Domain.Interfaces.eCommerce.Domain.Interfaces;
using Microsoft.Extensions.Caching.Distributed;

namespace eCommerce.Infrastructure.ExternalServices.Caching
{
    public class PasswordResetOtpCache : IPasswordResetOtpCache
    {
        private readonly IDistributedCache cache;
        private const string Prefix = "password-reset-otp:";

        public PasswordResetOtpCache(IDistributedCache cache)
        {
            this.cache = cache;
        }

        private static string GetKey(string email)
            => $"{Prefix}{email}";
        //ttl ==> time to live
        public async Task SetAsync(string email, string otp, TimeSpan ttl)
        {
            await cache.SetStringAsync(
                GetKey(email),
                otp,
                new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = ttl
                });
        }

        public async Task<string?> GetAsync(string email)
        {
            return await cache.GetStringAsync(GetKey(email));
        }

        public async Task RemoveAsync(string email)
        {
            await cache.RemoveAsync(GetKey(email));
        }

        public async Task<bool> ValidateOtpAsync(string email, string otp)
        {
            var cachedOtp = await GetAsync(email);

            if (string.IsNullOrEmpty(cachedOtp))
                return false;

            return cachedOtp == otp;
        }
    }


}
