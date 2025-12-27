using eCommerce.Application.DTOs.Cart;
using eCommerce.Application.Services.Interfaces.Cart;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eCommerce.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController(ICartService cartService) : ControllerBase
    {
        [HttpPost("checkout")]
        public async Task<IActionResult> Checkout(Checkout checkout)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var result = await cartService.Checkout(checkout);
            return result.success ? Ok(result) : BadRequest(result);
        }
        [HttpPost("save-checkout")]
        public async Task<IActionResult> SaveCheckout(IEnumerable<CreateOrderDto> orders)
        {
            var result = await cartService.SaveCheckoutHistory(orders);
            return result.success ? Ok(result) : BadRequest(result);
        }
    }
}
