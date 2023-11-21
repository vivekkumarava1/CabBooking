using CabFrontend.Models;
using Newtonsoft.Json;
using System.Text.Json;

namespace CabFrontend.Services
{
    public class UserServices
    {
        private readonly HttpClient _httpClient;
        public UserServices(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://localhost:7164/api/USERS/"); // Base URL of your API
        }
        
        public async Task<string> LoginAsync(LoginViewModel model)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("login", model);

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsStringAsync();
                }
                else
                {
                    // Handle unsuccessful login based on status code
                    // You might want to log or throw an exception based on the status code
                    return $"Error: {response.StatusCode}";
                }
            }
            catch (Exception ex)
            {
                // Handle other exceptions that might occur during the request
                // Log or throw an exception based on your error handling strategy
                return $"Exception: {ex.Message}";
            }
        }



    }
}
