using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.Domain.Interfaces
{
    namespace eCommerce.Domain.Interfaces
    {
        public interface IPasswordResetOtpCache
        {
            Task SetAsync(string email, string otp, TimeSpan ttl);
            Task<string?> GetAsync(string email);
            Task RemoveAsync(string email);
            Task<bool> ValidateOtpAsync(string email, string otp);
        }
    }


}
