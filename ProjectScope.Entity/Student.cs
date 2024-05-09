using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Http;

namespace ProjectScope.Entity
{
    public class Student
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id {  get; set; }

        [Required]
        [StringLength(100)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(100)]
        public string LastName { get; set; }

        [Required]
        [StringLength(100)]
        public string Gender { get; set; }

        [Required]
        public DateTime DateofBirth { get; set; }

        public string Email { get; set; }

        [Required]
        [StringLength (20)]
        public string Phone { get; set; }

        [Required]
        [StringLength(100)]
        public string Country { get; set; }

        [Required]
        [StringLength(100)]
        public string State { get; set; }

        [Required]
        [StringLength(100)]
        public string City { get; set; }

        [Required]
        [StringLength(100)]
        public string Hobbies { get; set; }

        public string? Avatar { get; set; }
        
        public string? Password { get; set; }
        public string? TemporaryPassword {  get; set; }
        public string? CourseId { get; set; }
    }
}
