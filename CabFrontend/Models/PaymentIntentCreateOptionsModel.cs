namespace CabFrontend.Models
{
    public class PaymentIntentCreateOptionsModel
    {
        public double Amount { get; set; } // The payment amount in cents
        public string Currency { get; set; } // The currency code (e.g., "usd")
        public string PaymentMethodTypes { get; set; }
    }
}
