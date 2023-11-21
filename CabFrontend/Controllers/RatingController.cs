using CabFrontend.Models;
using Microsoft.AspNetCore.Mvc;

namespace CabFrontend.Controllers
{
    public class RatingController : Controller
    {
        public static string cabID;
        public static string USERID;
        public static string ReservationID;
        public ActionResult PostRating(string cabId,string UserId,string reservationId)
        {
            cabID = cabId;
            USERID = UserId;
            ReservationID = reservationId;
            return View();
        }


        [HttpPost]
        public async Task<ActionResult> RatingPost(int SelectedRating)
        {
            var ratingValue = Request.Form["SelectedRating"];
            using (var client = new HttpClient())
            {
                var content = new CabRating()
                {
                    UserId = USERID.Trim('"'),
                    Rating = int.Parse(ratingValue),
                    CabId = cabID


                };
                client.BaseAddress = new Uri("https://localhost:7164/");

                // Make sure to set the appropriate API endpoint and headers
                var response = await client.PostAsJsonAsync("api/CabRating/api/cabs/rate", content);

                if (response.IsSuccessStatusCode)
                {
                    var response1 = await client.PutAsync($"api/Reservation/{ReservationID}/UpdateisRated", null);
                    if (response1.IsSuccessStatusCode)
                    {



                        TempData["success"] = "Rating submitted successfully.";
                    }
                }
                else
                {
                    TempData["error"] = "Failed to submit the rating.";
                }
            }

            return RedirectToAction("UserFirstPage","User");
        }



    }
}
