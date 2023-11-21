using CabFrontend.IServices;
using CabFrontend.Models;
using Microsoft.AspNetCore.Mvc;

namespace CabFrontend.Controllers
{
    public class StripeController : Controller
    {
        private readonly IStripeService _stripeService;
        private readonly HttpClient _httpClient;
        public StripeController(IStripeService stripeService)
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:7164/api/") // Base API URL
            };
            _stripeService = stripeService;
        }

        public static double amu;
       
        public IActionResult Checkout(double tripAmount,string id)
        {
            TempData["id"] = id;
            HttpContext.Session.SetString("amount",tripAmount.ToString());
            TempData["TripAmount"] = tripAmount;
            amu = tripAmount;
            
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ProcessPayment(PaymentRequest paymentRequest)
        {
            
            if (amu != null)
            {
                
                    paymentRequest.Amount = Double.Parse(HttpContext.Session.GetString("amount"));
                    string id = TempData["id"] as string;
                    var response = await _httpClient.GetAsync($"Reservation/{id}");

                    if (response.IsSuccessStatusCode)
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();

                        var paymentResponse = await _stripeService.MakePaymentAsync(paymentRequest);

                        if (paymentResponse.Success)
                        {
                            // Payment was successful
                            return RedirectToAction("PaymentSuccess", new { paymentIntentId = paymentResponse.PaymentIntentId });
                        }
                        else
                        {
                            // Payment failed, handle the error
                            return View("PaymentError", paymentResponse.ErrorMessage);
                        }

                    }
                
            }
            
            return View();
        }

        public IActionResult PaymentSuccess(string paymentIntentId)
        {
            // Display a success message or order summary
            return View("PaymentSuccess", paymentIntentId);
        }

        public IActionResult PaymentError(string errorMessage)
        {
            // Display an error message with the provided errorMessage
            return View("PaymentError", errorMessage);
        }
    }
}
