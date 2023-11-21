using CabFrontend.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Text;
using System.Net.Http;
using System.Linq;
using CabFrontend.Services;
using Microsoft.AspNetCore.Authentication;
using System.Reflection;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Microsoft.AspNetCore.Authorization;
using System.Net.Mail;
using System.Net;

namespace CabFrontend.Controllers
{
    [ServiceFilter(typeof(AuthFilter))]
  
    public class UserController : Controller
    {
        private readonly HttpClient _httpClient;
        public const string email ="_Email";

        public UserController()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:7164/api/") // Base API URL
            };
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        }


        [AllowAnonymous]
        public ActionResult Index()
        {
           
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Index([Bind("Name","Email","Message")]ContactViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Send email
                SendEmail(model);

                // You can add more logic here, such as saving the form data to a database

                // Redirect to a thank you page or show a success message
                TempData["success"] = "Admin will contact you soon..!";

                return RedirectToAction("Index");
                


            }

            // If the model is not valid, return to the contact page with validation errors
            return View(model);
        }
        private void ShowNotification(string type, string message)
        {
            // Use SweetAlert to show a notification
            ViewBag.Notification = $"Swal.fire('{type}', '{message}', '{type}')";
        }

        [AllowAnonymous]
        private void SendEmail(ContactViewModel model)
        {
            // Configure and send email
            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587, // Specify the SMTP port (usually 587 or 465)
                Credentials = new System.Net.NetworkCredential("vivekkumarava2@gmail.com", "bhfwynifivngffqs"),
                EnableSsl = true, // Enable SSL if required by your SMTP server
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress("vivekkumarava2@gmail.com"),
                Subject = "New Contact Form Submission",
                Body = $"Name: {model.Name}\nEmail: {model.Email}\nMessage: {model.Message}",
                IsBodyHtml = false,
            };

            mailMessage.To.Add("vivekkumarava1@gmail.com"); 

            smtpClient.Send(mailMessage);
        }

        
       

        public ActionResult LoginandSignup()
        {
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        private async Task<bool> IsValidUserAsync(string email, string password)
        {
            List<LoginRequest> userList = new List<LoginRequest>();
            using (var httpClient = new HttpClient())//handler
            {
                using (var response = await httpClient.GetAsync("https://localhost:7164/api/User/getAllUsers"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    userList = JsonConvert.DeserializeObject<List<LoginRequest>>(apiResponse);
                }
            }


            LoginRequest emp = userList.FirstOrDefault(c => c.Email == email && c.Password == password);
            

            return emp != null && emp.Password == password; // This is a simplified example
        }


        [HttpPost]
        public async Task<IActionResult> Login([Bind("Email", "Password")] LoginRequest model)
        {
            if (await IsValidUserAsync(model.Email, model.Password))
            {
                var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, model.Email),
                // Add more claims as needed
            };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                return RedirectToAction("UserFirstPage");
            }
            else
            {
                TempData["error"] = "Invalid credentials. Please try again.";
                return RedirectToAction("Login");
            }
        }


        public static string LoginEmail;
        [HttpPost]

        [ActionName("LoginandSignup")]
        public async Task<IActionResult> LoginandSignup([Bind("Email", "Password")] LoginRequest model)
        {
			if (await IsValidUserAsync(model.Email, model.Password))
			{
				HttpContext.Session.SetString("email", model.Email);
				var claims = new List<Claim>
			{
				new Claim(ClaimTypes.Name, model.Email),
                // Add more claims as needed
            };

				var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
				var principal = new ClaimsPrincipal(identity);

				await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
				TempData["success"] = "Login Successfully";
                var response = await _httpClient.GetAsync($"User/userNameByEmail?email={model.Email}");
                var Name = await response.Content.ReadAsStringAsync();
                string name = Name.Replace("\"", "");
                HttpContext.Session.SetString("Name", name) ;

                return RedirectToAction("UserFirstPage");
			}
			else
			{
				TempData["error"] = "Invalid credentials. Please try again.";
				return RedirectToAction("Login");
			}



			/*if (ModelState.IsValid)
            {
                List<LoginRequest> userList = new List<LoginRequest>();
                using (var httpClient = new HttpClient())//handler
                {
                    using (var response = await httpClient.GetAsync("https://localhost:7164/api/User/getAllUsers"))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        userList = JsonConvert.DeserializeObject<List<LoginRequest>>(apiResponse);
                    }
                }


                LoginRequest emp = userList.FirstOrDefault(c => c.Email == user.Email && c.Password == user.Password);

                if (emp != null)
                {
                    HttpContext.Session.SetString("email", emp.Email);
                    var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, emp.Email),
                // Add more claims as needed
            };

                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var principal = new ClaimsPrincipal(identity);

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                    LoginEmail = emp.Email;
                    TempData["Email"] = emp.Email;
                    TempData["success"] = "Login Succesfully";
                    return RedirectToAction("UserFirstPage");
                }
            }
            TempData["error"] = "Invalid Password or email id";
            return View("Login");*/

		}
        

        public async Task<IActionResult> Logout()
        {

            HttpContext.Session.Clear();
			await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            if (!Response.Headers.ContainsKey("Cache-Control"))
            {
                Response.Headers.Add("Cache-Control", "no-cache, no-store, must-revalidate");
            }

            if (!Response.Headers.ContainsKey("Pragma"))
            {
                Response.Headers.Add("Pragma", "no-cache");
            }

            if (!Response.Headers.ContainsKey("Expires"))
            {
                Response.Headers.Add("Expires", "0");
            }


            return RedirectToAction("Index");
             
        }




        public ActionResult UserFirstPage()
        {
            return View();
        }


        public ActionResult Signup()
        {
            return View();
        }

        // GET: UserController1/Details/5
        public ActionResult Details()
        {
            return View();
        }

        // GET: UserController1/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: UserController1/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: UserController1/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: UserController1/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: UserController1/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: UserController1/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
		[AllowAnonymous]
		public async Task<IActionResult> SignUp([Bind("FirstName,LastName,Email,PhoneNumber,Password")] User usr)
        {
            if (ModelState.IsValid)
            {
                User user = new User();
                using (var httpClient = new HttpClient())
                {
                    StringContent content = new StringContent(JsonConvert.SerializeObject(usr), Encoding.UTF8, "application/json");

                    using (var response = await httpClient.PostAsync("https://localhost:7164/api/User/register", content))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            TempData["success"] = "User registered successfully.";
                            return RedirectToAction(nameof(Login));
                        }
                        else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                        {
                            TempData["error"] = "User with the provided email is already registered.";
                        }
                        else
                        {
                            TempData["error"] = "Error registering user. Please try again.";
                        }
                    }
                }

            }
            return View(usr);
        }




        public static string selectedDest;
        
        [HttpGet]
        public async Task<ActionResult> GetNearestCabs(double Longitude, double Latitude, string selectedDestination)
        {
            TempData["Destination"] = selectedDestination;
            string selectedDest = selectedDestination;
            HttpResponseMessage response = await _httpClient.GetAsync($"cab/nearest?latitude={Latitude}&longitude={Longitude}");
            string responseData = await response.Content.ReadAsStringAsync();



            if (responseData == "\"Their is no nearby Cab\"")
            {
                /* TempData["error"] = "Their is no near by Cab to your Location";*/
                return View();
            }

            
            var requestUri = $"Distance/api/calculate-trip?userLat={Latitude}&userLng={Longitude}&destinationName={selectedDestination}";

            // Make the GET request to the API
            var responses = await _httpClient.GetAsync(requestUri);
            var dp = await responses.Content.ReadAsStringAsync();
            DP dP = JsonConvert.DeserializeObject<DP>(dp);

            
            List<CabDistance> apiresponse = JsonConvert.DeserializeObject<List<CabDistance>>(responseData);
            var res = new DistanceandPrice
            {
                cabDistances = apiresponse,
                Distance = dP.Distance,
                Price = dP.Price,

            };
            return View(res);


        }




        [AllowAnonymous]
		public ActionResult ForgetPasswordPage()
        {
            if (TempData["Message"] != null)
            {
                ViewBag.mes = TempData["Message"];
            }
            return View();
        }
        public static string UserEmail;
		[AllowAnonymous]
		[HttpPost]
        public async Task<ActionResult> ForgetPasswordPage1(ForgotPasswordViewModel model)
        {
            UserEmail = model.Email;
            if (ModelState.IsValid)
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("https://localhost:7164/");

                    // Send a POST request to the API to initiate the password reset
                    HttpResponseMessage response = await client.PostAsJsonAsync("api/OTP/send", model);

                    if (response.IsSuccessStatusCode)
                    {
                        // Password reset initiated; provide a success message
                        ViewBag.Message = "Otp Send Successfully";
                        TempData["Success"] = ViewBag.Message;
                        return RedirectToAction("VerifyOTP");

                    }
                    else
                    {
                        // Handle API error, display error message
                        ViewBag.Message = "User is Not Registered";
                        TempData["Message"] = ViewBag.Message;
                        return RedirectToAction("ForgetPasswordPage",model);
                    }
                }
            }

            return View("ForgetPasswordPage",model);

        }

		[AllowAnonymous]
		public ActionResult VerifyOTP()
        {
            return View();
        }



        [HttpPost]
		[AllowAnonymous]
		public async Task<ActionResult> VerifyOTP(VerifyOTPViewModel model)
        {
            if (ModelState.IsValid)
            {
                using (var client = new HttpClient())
                {
                    var apiUrl = "https://localhost:7164/api/OTP/verify-otp";
                    var data = new
                    {
                        email = UserEmail,
                        otp = model.OTP
                    };

                    var response = client.PostAsJsonAsync(apiUrl, data).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        var apiResponseContent = await response.Content.ReadAsStringAsync();
                        TempData["Success"] = apiResponseContent;
                        return RedirectToAction("ResetPassword");
                    }
                    else
                    {
                        ModelState.AddModelError("OTP", "Invalid OTP. Please try again.");
                    }
                }
            }
            return View(model);
        }






		[AllowAnonymous]
		public ActionResult ResetPassword()
        {
            // Return the view to display the password reset form
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
		[AllowAnonymous]
		public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Verify that the new password matches the confirmation password
                if (model.NewPassword != model.ConfirmPassword)
                {
                    ModelState.AddModelError("ConfirmPassword", "The new password and confirmation password do not match.");
                    return View(model);
                }

                // Make an API request to reset the user's password
                bool passwordResetSuccessful = await ResetPasswordWithAPI(model.NewPassword);

                if (passwordResetSuccessful)
                {
                    // Password reset was successful
                    // You can redirect to a login page or display a success message
                    TempData["Success"] = "Password Reset Successfully";
                    return RedirectToAction("LoginandSignup"); // Replace with the appropriate action
                }
                else
                {
                    ModelState.AddModelError("", "Password reset failed. Please try again.");
                }
            }

            // If the form submission is invalid or password reset fails, return to the reset password form
            return View(model);
        }


		[AllowAnonymous]
		private async Task<bool> ResetPasswordWithAPI(string newPassword)
        {
            using (var httpClient = new HttpClient())
            {
                var apiUrl = "https://localhost:7164/api/OTP/change-password";

                var data = new
                {
                    email = UserEmail,
                    newPassword = newPassword,
                };


                var response = await httpClient.PostAsJsonAsync(apiUrl, data);

                if (response.IsSuccessStatusCode)
                {
                    // Password reset was successful
                    return true;
                }
                else
                {
                    // Password reset failed
                    return false;
                }
            }
        }



        
        public async Task<IActionResult> CalculateDistanceAndFindNearestCabs(UserLocation userLocation, DestinationLocation destinationLocation)
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri("https://localhost:7164/");
                try
                {
                    // Fetch data from CalculateDistance API


                    var calculateDistanceResponse = await httpClient.PostAsJsonAsync("calculate-distance", new { UserLocation = userLocation, DestinationLocation = destinationLocation });
                    calculateDistanceResponse.EnsureSuccessStatusCode();

                    var distanceResponse = await calculateDistanceResponse.Content.ReadAsAsync<DistanceResponse>();
                    var calculatedDistance = distanceResponse.Distance;

                    // Fetch data from FindNearestCabs API
                    var findNearestCabsResponse = await httpClient.PostAsJsonAsync("find-nearest-cabs", new { UserLocation = userLocation });
                    findNearestCabsResponse.EnsureSuccessStatusCode();

                    var nearestCabs = await findNearestCabsResponse.Content.ReadAsAsync<List<CabDistance>>();

                    if (nearestCabs == null)
                    {
                        TempData["error"] = "Their is no near by cab to your location at this moment";
                        return View("GetNearestCabs");
                    }

                    // Create an instance of the model with the fetched data
                    var model = new CalculateDistanceAndNearestCabsModel
                    {
                        CalculatedDistance = calculatedDistance,
                        NearestCabs = nearestCabs
                    };

                    // Pass the model to the view
                    return View(model);
                }
                catch (HttpRequestException ex)
                {
                    // Handle any API request errors and return an error view or message.
                    return View("ErrorView", ex.Message);
                }
            }
        }

       
       


        [HttpPost]
        public async Task<IActionResult>  MyBookingAsync()
        {
            _httpClient.BaseAddress = new Uri("https://localhost:7164/");
            //string email = LoginEmail;
            string? email = HttpContext.Session.GetString("email");
            var response = await _httpClient.GetAsync($"api/User/userIdByEmail?email={email}");

            if (response.IsSuccessStatusCode)
            {
                var userId = await response.Content.ReadAsStringAsync();
                string apiEndpoint = $"api/Reservation/user/{userId}";

                // Send an HTTP GET request to the API
                HttpResponseMessage response1 = await _httpClient.GetAsync(apiEndpoint);

                // Check if the response is successful
                if (response1.IsSuccessStatusCode)
                {
                    // Read the response content as a string
                    string responseData = await response1.Content.ReadAsStringAsync();

                    // Deserialize the JSON response into a model (assuming you have a Reservation model)
                    List<Reservation> reservations = JsonConvert.DeserializeObject<List<Reservation>>(responseData);

                    // Pass the data to the view
                    return View(reservations);
                }
                
            }
            return null;
            
        }



        public async Task<IActionResult> Booking()
        {
            _httpClient.BaseAddress = new Uri("https://localhost:7164/");
			string? email = HttpContext.Session.GetString("email");
			var response = await _httpClient.GetAsync($"api/User/userIdByEmail?email={email}");

            if (response.IsSuccessStatusCode)
            {
                var userId = await response.Content.ReadAsStringAsync();
                string apiEndpoint = $"api/Reservation/user/{userId}";

                
                HttpResponseMessage response1 = await _httpClient.GetAsync(apiEndpoint);

                
                if (response1.IsSuccessStatusCode)
                {
                    // Read the response content as a string
                    string responseData = await response1.Content.ReadAsStringAsync();

                    // Deserialize the JSON response into a model (assuming you have a Reservation model)
                    List<Reservation> reservations = JsonConvert.DeserializeObject<List<Reservation>>(responseData);

                    // Pass the data to the view
                    return View(reservations);
                }

            }
            return View();
        }
        public IActionResult AboutUs()
        {
            return View();
        }

        }
    
}
