using eCommerce.Application.DTOs;
using eCommerce.Application.DTOs.Cart;
using eCommerce.Application.Services.Interfaces.Cart;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stripe;

namespace eCommerce.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CartController (ICartService _cartService, IConfiguration _configuration) : ControllerBase
    {
        // ============================
        // 1️⃣ Add product to Cart
        // ============================
        [HttpPost("add")]
        public async Task<IActionResult> AddToCart([FromBody] AddToCartDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _cartService.AddToCartAsync(dto);
            return result.success ? Ok(result) : BadRequest(result);
        }

        // ============================
        // 2️⃣ View Cart
        // ============================
        [HttpGet("view/{userId}")]
        public async Task<IActionResult> ViewCart(string userId)
        {
            var cart = await _cartService.GetCartAsync(userId);
            if (cart == null || !cart.Items.Any())
                return NotFound(new { message = "Cart is empty" });

            return Ok(cart);
        }

        // ============================
        // 3️⃣ Update Cart (change quantity/remove)
        // ============================
        [HttpPut("update")]
        public async Task<IActionResult> UpdateCart([FromBody] AddToCartDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _cartService.UpdateCartAsync(dto);
            return result.success ? Ok(result) : BadRequest(result);
        }

        // ============================
        // 4️⃣ Remove product from cart
        // ============================
        [HttpDelete("remove/{userId}/{productId}")]
        public async Task<IActionResult> RemoveFromCart(string userId, int productId)
        {
            var result = await _cartService.RemoveFromCartAsync(userId, productId);
            return result.success ? Ok(result) : BadRequest(result);
        }

        // ============================
        // 5️⃣ Checkout: process payment + create order
        // ============================
        [HttpPost("checkout")]
        public async Task<IActionResult> Checkout([FromBody] Application.DTOs.CheckoutDto checkout)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _cartService.Checkout(checkout.UserId, checkout.PaymentMethodId);
            return result.success ? Ok(result) : BadRequest(result);
        }
        [HttpPost("save-checkout")]
        public async Task<IActionResult> SaveCheckout([FromBody] SaveCheckoutDto dto)
        {
            var result = await _cartService.SaveCheckoutAsync(dto);
            return result.success ? Ok(result) : BadRequest(result);
        }

        // =========================
        // Stripe Webhook
        // =========================
        [HttpPost("webhook")]
        public async Task<IActionResult> StripeWebhook()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();

            try
            {
                var stripeSignature = Request.Headers["Stripe-Signature"];
                var webhookSecret = _configuration["Stripe:WebhookSecret"];

                var stripeEvent = Stripe.EventUtility.ConstructEvent(
                    json,
                    stripeSignature,
                    webhookSecret
                );

                // ✅ Payment completed
                if (stripeEvent.Type == "checkout.session.completed")
                {
                    var session = stripeEvent.Data.Object as Stripe.Checkout.Session;

                    if (session != null)
                    {
                        var userId = session.Metadata["userId"];
                        var orderId = int.Parse(session.Metadata["orderId"]);

                        await _cartService.CompleteOrderAfterPaymentAsync(userId, orderId);
                    }
                }


                return Ok();
            }
            catch (StripeException ex)
            {
                return BadRequest(ex.Message);
            }
        }
        // ============================
        // 6️⃣ View all user orders
        // ============================
        [HttpGet("orders/{userId}")]
        public async Task<IActionResult> GetUserOrders(string userId)
        {
            var orders = await _cartService.GetUserOrdersAsync(userId);
            if (!orders.Any()) return NotFound(new { message = "No orders found" });

            return Ok(orders);
        }

        // ============================
        // 7️⃣ View specific order
        // ============================
        [HttpGet("orders/{userId}/{orderId}")]
        public async Task<IActionResult> GetOrderById(string userId, int orderId)
        {
            var order = await _cartService.GetOrderByIdAsync(userId, orderId);
            if (order == null) return NotFound(new { message = "Order not found" });

            return Ok(order);
        }
    }


}
