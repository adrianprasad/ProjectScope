using System.ComponentModel.DataAnnotations;

namespace ProjectScope.ViewModels
{
    public class LoginViewModel
    {
        //[RegularExpression(@"^[a-zA-Z0-9._%+-]+@gmail\.com$", ErrorMessage = "Invalid Email")]
        //[EmailAddress(ErrorMessage = "Invalid Email")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Enter OTP")]
        [Display(Name = "Enter Otp")]
        public string Otp { get; set; }

        [Required(ErrorMessage = "Enter New Password")]
        [Display(Name = "Enter New Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Enter Confirm Password")]
        [Display(Name = "Enter Confirm Password")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; }

    }
}
