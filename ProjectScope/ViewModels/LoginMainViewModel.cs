using System.ComponentModel.DataAnnotations;

namespace ProjectScope.ViewModels
{
    public class LoginMainViewModel
    {
        [Required(ErrorMessage = "Email is required")]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@gmail\.com$", ErrorMessage = "Invalid Email")]
        [EmailAddress(ErrorMessage = "Invalid Email")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Enter Password")]
        [Display(Name = "Enter Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        
        public bool KeepLoggedIn { get; set; }
    }
}
