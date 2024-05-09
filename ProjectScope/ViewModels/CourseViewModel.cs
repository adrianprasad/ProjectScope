using System.ComponentModel.DataAnnotations;

namespace ProjectScope.ViewModels
{
    public class CourseViewModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string CourseName { get; set; }

        [Required]
        [StringLength(150)]
        public string Duration { get; set; }

        [Required]
        public int Fee { get; set; }

       
    }
}
