using CabFrontend.Models;

namespace CabFrontend.IServices
{
    public interface IStripeService
    {
        Task<PaymentResponse> MakePaymentAsync(PaymentRequest paymentRequest);
    }
}
