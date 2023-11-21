using System.ComponentModel.DataAnnotations;

namespace CabFrontend.Models
{
    public class User
    {
        private const string EmailRegexPattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
        public string? Id { get; set; }             // Unique identifier for the user


        public string FirstName { get; set; }      // First name of the user


        public string LastName { get; set; }       // Last name of the user

        [Required(ErrorMessage = "Email is required.")]
        //[RegularExpression(EmailRegexPattern, ErrorMessage = "Invalid email address.")]
        public string Email { get; set; }          // Email address of the user


        [Required(ErrorMessage = "Phone number is required.")]
        //[RegularExpression(@"^\d{10}$", ErrorMessage = "Invalid phone number. Please enter 10 digits.")]
        public string PhoneNumber { get; set; }    // Phone number of the user


        [Required(ErrorMessage = "Password is required.")]
        // [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$", ErrorMessage = "Password must be at least 8 characters long and contain at least one uppercase letter, one lowercase letter, one digit, and one special character.")]
        public string Password { get; set; }
    }
}
