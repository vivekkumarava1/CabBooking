using System.ComponentModel.DataAnnotations;

namespace CabFrontend.Models
{

    
    public class ForgotPasswordViewModel
    {
        private const string EmailRegexPattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";


        [Required(ErrorMessage = "Email is required.")]
        [RegularExpression(EmailRegexPattern, ErrorMessage = "Invalid email address.")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address")]
        [Display(Name = "Email Address")]
        public string Email { get; set; }
    }
}
