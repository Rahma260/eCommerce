using eCommerce.Domain.Entities;

namespace eCommerce.Domain.Interfaces.Cart
{
    public interface IPaymentMethod
    {
        Task<IEnumerable<PaymentMethod>> GetPaymentMethods();
    }
    
}
