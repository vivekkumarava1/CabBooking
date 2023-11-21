using CabFrontend.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;

namespace CabFrontend.Controllers
{
    public class ReservationController : Controller
    {
        private readonly HttpClient _httpClient;
        public ReservationController()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:7164/api/") // Base API URL
            };
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        }

        public IActionResult Index()
        {
            return View();
        }


        public static string CABID;
        [HttpPost]
        public async Task<IActionResult> Booking(BookingViewModel model)
        {
            string destination = TempData["Destination"] as string;
            try
            {

                _httpClient.BaseAddress = new Uri("https://localhost:7164/"); // Replace with your API URL.
               // string email = TempData["Email"] as string;
                string? email = HttpContext.Session.GetString("email");
                var response = await _httpClient.GetAsync($"api/User/userIdByEmail?email={email}");

                if (response.IsSuccessStatusCode)
                {
                    var userId = await response.Content.ReadAsStringAsync();
                    bool IsAvailable = true;

                    CABID = model.CabId;

                    var reservation = new Reservation
                    {

                        UserId = userId,
                        CabId = model.CabId,
                        Destination = destination,
                        tripDistance= model.tripDistance,
                        isPaid = model.isPaid,
                        isRated = false,
                        tripAmount= model.tripAmount,
                        BookingTime = DateTime.Now, // Use the desired booking time.
                        Status = 0
                    };
                    //var reservationJson = JsonConvert.SerializeObject(reservation);
                    StringContent content = new StringContent(JsonConvert.SerializeObject(reservation), Encoding.UTF8, "application/json");

                    // Replace with your API URL.

                    var response1 = await _httpClient.PostAsync("api/Reservation/create", content);
                    if (response1.IsSuccessStatusCode)
                    {
                        return RedirectToAction("ConfirmBooking",model); // Redirect to a list of reservations or another page.
                    }
                    else
                    {
                        return View("Error");
                    }

                }
                else
                {
                    return View("Error"); // Handle errors appropriately.
                }
            }
            catch (HttpRequestException)
            {

            }
            return View();
        }

        public async Task<IActionResult> ListReservations()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:7164");
                try
                {
                    string email = TempData["Email"] as string;

                    var response1 = await _httpClient.GetAsync($"/api/User/userIdByEmail?email={email}");


                    var userId = await response1.Content.ReadAsStringAsync();

                    var response = await client.GetAsync($"/api/Reservation/user/{userId}");

                    if (response.IsSuccessStatusCode)
                    {
                        var reservations = await response.Content.ReadAsAsync<List<Reservation>>();
                        // Assuming you have a Reservation model.

                        return View("ConfirmBooking", reservations);
                    }
                    else
                    {
                        return View("Error"); // Handle errors appropriately.
                    }
                }
                catch (HttpRequestException)
                {
                    return View("Error"); // Handle network or API-related errors.
                }
            }
        }


        public async Task<IActionResult> ConfirmBooking(BookingViewModel model)
        {

            try
            {

                TempData["success"] = "Cab Booking Confirm";
                return View(model);



            }
            catch (Exception ex)
            {
                // Handle exception, e.g., network issues.
                Console.WriteLine("Exception: " + ex.Message);
            }
            return View();

        }

        public async Task<IActionResult> CancelCab(string reservationId)
        {
            try
            {
               

                    // Send an HTTP DELETE request to cancel the reservation
                    HttpResponseMessage response = await _httpClient.PutAsync($"Reservation/{reservationId}/cancel",null);

                    if (response.IsSuccessStatusCode)
                    {
                        // The reservation was successfully canceled
                        // You can add a success message to TempData or handle it as needed
                        TempData["sucess"] = "Reservation canceled successfully.";
                    }
                    else
                    {
                        // Handle the case where the cancellation was not successful
                        // You can add an error message to TempData or handle it as needed
                        TempData["error"] = "Failed to cancel the reservation.";
                    }
                
            }
            catch (Exception ex)
            {
                // Handle exceptions that occurred during the HTTP request
                // Log the exception or add an error message to TempData
                TempData["error"] = "An error occurred while canceling the reservation.";
            }

            return RedirectToAction("Booking","User");
        }


    }
}
