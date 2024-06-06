using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using Talabat.APIs.Errors;
using Talabat.Core.Entities.Basket;
using Talabat.Core.Entities.Order_Aggregate;
using Talabat.Core.Repositories.Contract;

namespace Talabat.APIs.Controllers
{
    public class PaymentController : BaseApiController
    {
        private readonly IPaymentService _paymentService;
        private readonly ILogger<PaymentController> _logger;
        private readonly string endpointSecret = "whsec_591d49518278afca675651d566e92293593f517fe1ff125d3a8613599eb3b030";

        public PaymentController(IPaymentService paymentService, ILogger<PaymentController> logger)
        {
            _paymentService = paymentService;
            _logger = logger;

        }

        [HttpPost("{basketid}")]
        [Authorize]
        [ProducesResponseType(typeof(CustomerBasket), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<CustomerBasket>> CreateOrUpdatePaymentIntent(string basketId)
        {
            var basket = await _paymentService.CreateOrUpdatePaymentIntent(basketId);
            if (basket is null) return BadRequest(new ApiResponse(400, "basket not found"));
            return Ok(basket);
        }

        [HttpPost("webhook")]
        public async Task<IActionResult> Webhook()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            var stripeEvent = EventUtility.ConstructEvent(json,
                                Request.Headers["Stripe-Signature"], endpointSecret);

            Order? order;
            var paymentIntent = stripeEvent.Data.Object as PaymentIntent;
            // Handle the event
            if (stripeEvent.Type == Events.PaymentIntentSucceeded)
            {
                order = await _paymentService.UpdateOrderStatus(paymentIntent.Id, true);
                _logger.LogInformation("Order is Succeded: {0}", order?.PaymentIntentId);
            }
            else if (stripeEvent.Type == Events.PaymentIntentPaymentFailed)
            {
                order = await _paymentService.UpdateOrderStatus(paymentIntent.Id, false);
                _logger.LogInformation("Order is Failed: {0}", order?.PaymentIntentId);

            }


            return Ok();
        }
    }
}
