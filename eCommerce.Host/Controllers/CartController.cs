using eCommerce.Application.DTOs.Cart;
using eCommerce.Application.Services.Interfaces.Cart;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stripe;

namespace eCommerce.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController(ICartService cartService) : ControllerBase
    {
        [HttpPost("checkout")]
        public async Task<IActionResult> Checkout(CheckoutDto checkout)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var result = await cartService.Checkout(checkout);
            return result.success ? Ok(result) : BadRequest(result);
        }
        [HttpPost("save-checkout")]
        public async Task<IActionResult> SaveCheckout(IEnumerable<CreateCheckoutDto> orders)
        {
            var result = await cartService.SaveCheckoutHistory(orders);
            return result.success ? Ok(result) : BadRequest(result);
        }
        //[HttpPost("webhook")]
        //public async Task<IActionResult> StripeWebhook()
        //{
        //    var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();

        //    try
        //    {
        //        var stripeSignature = Request.Headers["Stripe-Signature"];
        //        var webhookSecret = configuration["Stripe:WebhookSecret"];

        //        var stripeEvent = EventUtility.ConstructEvent(
        //            json,
        //            stripeSignature,
        //            webhookSecret
        //        );

        //        // ✅ Payment completed successfully
        //        if (stripeEvent.Type == Stripe.Events.CheckoutSessionCompleted)
        //        {
        //            var session = stripeEvent.Data.Object as Stripe.Checkout.Session;

        //            if (session is not null)
        //            {
        //                // metadata لازم تكوني باعتاها من StripePaymentService
        //                var userId = session.Metadata["userId"];

        //                // هنا ممكن:
        //                // 1️⃣ تجيبي الكارت من Redis
        //                // 2️⃣ تحفظي Order في DB
        //                // 3️⃣ تمسحي الكارت من Redis

        //                await cartService.CompleteOrderAfterPayment(userId);
        //            }
        //        }

        //        return Ok();
        //    }
        //    catch (StripeException e)
        //    {
        //        return BadRequest(e.Message);
        //    }
        //}
    }
}
