using System.ComponentModel.DataAnnotations;

namespace ProjectScope.ViewModels
{
    public class RegistrationViewModel
    {

        [Required(ErrorMessage = "Enter the First Name")]
        [Display(Name = "Enter First Name")]
        [DataType(DataType.Text)]
        [RegularExpression(@"^[A-Za-z ]*$", ErrorMessage = "Invalid Name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Enter the First Name")]
        [DataType(DataType.Text)]
        [RegularExpression(@"^[A-Za-z ]*$", ErrorMessage = "Invalid Name")]
        public string LastName { get; set; }

        [Required(ErrorMessage="Select Gender")]
        public string Gender { get; set; }

        [Required(ErrorMessage = "Date of Birth is required")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime DateofBirth { get; set; }

        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@gmail\.com$", ErrorMessage = "Invalid Email")]
        [EmailAddress(ErrorMessage = "Invalid Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Enter Confirm Password")]
        [MaxLength(10, ErrorMessage = "Enter 10 digit Number")]
        [MinLength(10, ErrorMessage = "Enter 10 Digit Number")]
        public string Phone { get; set; }

        [Required(ErrorMessage="Select Country")]
        public string Country { get; set; }

        [Required(ErrorMessage = "Select State")]
        public string State { get; set; }

        [Required(ErrorMessage = "Select City")]
        public string City { get; set; }

        [Required(ErrorMessage = "Select Hobbies")]
        public string Hobbies { get; set; }        
        public string? Avatar { get; set; }
        public string? Password { get; set; }
        public int Id { get; set; }
    }
}
