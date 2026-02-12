using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.Application.DTOs.Cart
{
    public class CheckoutDto
    {
        public required int PaymentMethodId { get; set; }
        public required string UserID { get; set; }
        public required IEnumerable<ProcessCart> Carts { get; set; }
    }
}
