using CabFrontend.IServices;
using CabFrontend.Models;
using CabFrontend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CabFrontend.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private readonly UserServices _userService;
        private readonly HttpClient _httpClient;
        public AccountController(UserServices userService)
        {
            _userService = userService;
            
        }
        
        public IActionResult Login()
        {
            return View(new LoginViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.Role == "user")
                {
                    var client = new HttpClient();
                    client.BaseAddress = new Uri("https://localhost:7164/api/USERS/");
                    var response = await client.PostAsJsonAsync("login", model);

                    if (response.IsSuccessStatusCode)
                    {
                        // Successful login logic
                        // Redirect to the appropriate page based on the response
                        return RedirectToAction("Index", "Account");
                    }

                    else
                    {
                        ModelState.AddModelError("", "Invalid username, password, or role.");
                    }
                }
                else
                {
                    var client = new HttpClient();
                    client.BaseAddress = new Uri("https://localhost:7164/api/Drivers/");
                    var response = await client.PostAsJsonAsync("login", model);

                    if (!response.IsSuccessStatusCode)
                    {
                        // Successful login logic
                        // Redirect to the appropriate page based on the response
                        return RedirectToAction("UserFirstPage", "User");
                    }

                    else
                    {
                        ModelState.AddModelError("", "Invalid username, password, or role.");
                    }
                }
            }

            // If we reach here, login failed, redisplay the form with errors.
            return View(model);
        }


        public IActionResult Index()
        {
            return View();
        }
    }
}
