namespace CabFrontend.Models
{
    public class PaymentResponse
    {
        public bool Success { get; set; }
        public string PaymentIntentId { get; set; }
        public string ErrorMessage { get; set; }
    }
}
