using System.ComponentModel.DataAnnotations;

namespace CabFrontend.Models
{
    public class LoginRequest
    {
        [Required(ErrorMessage = "Email address is required")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address")]
        [Display(Name = "Email Address")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Please enter a new password")]
        [DataType(DataType.Password)]
        [Display(Name = "New Password")]
        public string Password { get; set; }
    }
}
