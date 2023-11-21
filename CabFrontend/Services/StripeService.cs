using CabFrontend.IServices;
using CabFrontend.Models;
using Microsoft.Extensions.Options;
using Stripe;

namespace CabFrontend.Services
{
    public class StripeService : IStripeService
    {
        private readonly StripeSettings _stripeSettings;

        private readonly ILogger<StripeService> _logger;
        public StripeService(IOptions<StripeSettings> stripeSettings, ILogger<StripeService> logger)
        {
            _stripeSettings = stripeSettings.Value;
            StripeConfiguration.ApiKey = _stripeSettings.SecretKey;
            _logger = logger;
        }

        public async Task<PaymentResponse> MakePaymentAsync(PaymentRequest paymentRequest)
        {
            try
            {
                var options = new PaymentIntentCreateOptions
                {
                    Amount = (long?)paymentRequest.Amount,
                    Currency = "usd",
                    PaymentMethodTypes = new List<string> { "card" },
                };

                var service = new PaymentIntentService();
                var paymentIntent = await service.CreateAsync(options);

                // Log successful payment details
                _logger.LogInformation("Payment successful. PaymentIntentId: {PaymentIntentId}", paymentIntent.Id);

                var paymentResponse = new PaymentResponse
                {
                    Success = true,
                    PaymentIntentId = paymentIntent.Id,
                    // Add more properties if needed
                };

                return paymentResponse;
            }
            catch (Exception ex)
            {
                // Log the exception for debugging
                _logger.LogError(ex, "Error making payment.");

                // Return an error response with a more meaningful error message
                return new PaymentResponse { Success = false, ErrorMessage = "An error occurred while processing the payment." };
            }
        }

    }
}
