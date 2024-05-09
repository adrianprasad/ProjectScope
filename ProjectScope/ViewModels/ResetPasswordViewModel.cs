using System.ComponentModel.DataAnnotations;

namespace ProjectScope.ViewModels
{
    public class ResetPasswordViewModel
    {
        [Display(Name ="Enter Email")]
        public string? Email { get; set; }
        [Required]
        public string TempPassword { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set;}
    }
}
