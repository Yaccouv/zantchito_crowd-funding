using Microsoft.AspNetCore.Mvc;
using Stripe;
using Unit_Trust_Backend.Investor.DTOs;
using Unit_Trust_Backend.Investor.Models;
using Unit_Trust_Backend.Investor.Services;

namespace Unit_Trust_Backend.Controllers
{
    [ApiController]
    [Route("api/payment")]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpPost("create-intent")]
        public IActionResult CreatePaymentIntent([FromBody] PaymentRequest request)
        {
           

            var options = new PaymentIntentCreateOptions
            {
                Amount = request.Amount,
                Currency = "mwk",
                AutomaticPaymentMethods = new PaymentIntentAutomaticPaymentMethodsOptions
                {
                    Enabled = true
                }
            };

            var service = new PaymentIntentService();
            var intent = service.Create(options);

            return Ok(new { client_secret = intent.ClientSecret });
        }

        [HttpPost("save")]
        public async Task<IActionResult> SavePayment([FromBody] PaymentDTO paymentDto)
        {
            var result = await _paymentService.SavePaymentAsync(paymentDto);
            return Ok(result);
        }
    }
}
