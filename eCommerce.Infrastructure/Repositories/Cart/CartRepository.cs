using eCommerce.Domain.Entities;
using eCommerce.Domain.Interfaces;
using eCommerce.Domain.Interfaces.Cart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.Infrastructure.Repositories.Cart
{
    public class CartRepository(IUnitOfWork unitOfWork) : ICart
    {
        public Task<int> SaveCheckoutHistory(IEnumerable<Order> checkouts)
        {
            foreach(var checkout in checkouts)
                unitOfWork.Orders.AddAsync(checkout);

            return unitOfWork.SaveAsync();
        }
    }
}
