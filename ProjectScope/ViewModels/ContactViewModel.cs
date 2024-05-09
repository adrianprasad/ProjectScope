using System.ComponentModel.DataAnnotations;

namespace ProjectScope.ViewModels
{
    public class ContactViewModel
    {
        [Display(Name = "Enter Name")]
        public string Name { get; set; }
        [Display(Name = "Enter Email Id")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Display(Name = "Enter Subject")]
        [DataType(DataType.Text)]
        public string Subject { get; set; }
        [Display(Name = "Enter Message")]
        [DataType(DataType.Text)]
        public string Message { get; set; }
    }
}
