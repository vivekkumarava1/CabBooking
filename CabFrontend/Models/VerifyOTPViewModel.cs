using System.ComponentModel.DataAnnotations;

namespace CabFrontend.Models
{
    public class VerifyOTPViewModel
    {
        [Required(ErrorMessage = "Please enter the OTP")]
        public string OTP { get; set; }
    }
}
