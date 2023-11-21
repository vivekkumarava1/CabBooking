namespace CabFrontend.Models
{
    public class PaymentRequest
    {
        
        public double Amount { get; set; } // Payment amount
        public string Currency { get; set; } // Currency code (e.g., "USD")
        public string cardNumber { get; set; } // Credit card number
        public string expiry { get; set; } // Expiry month (e.g., "12")
        public string ExpiryYear { get; set; } // Expiry year (e.g., "23")
        public string cvc { get; set; }
        public string upiId { get; set; }
    }
}
