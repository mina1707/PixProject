using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pix.Models
{
    
    public class User
    {
        [Key]  //  NOT NEEDED IF PROP NAME IS SAME AS ModelName + Id
        public int UserId { get; set; }
        
        [Required(ErrorMessage ="is required!")]
        [MinLength(2,ErrorMessage ="must be at least 2 characters!")]
        [Display(Name ="First Name:")]
        public string FirstName { get; set; }

        [Required(ErrorMessage ="is required!")]
        [MinLength(2,ErrorMessage ="must be at least 2 characters!")]
        [Display(Name ="Last Name:")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "must be a valid email!")]
        [EmailAddress]
        [Display(Name = "Email:")]
        public string Email { get; set; }

        [Required(ErrorMessage = "is required")]
        [MinLength(8, ErrorMessage = "must be at least 8 characters!")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [NotMapped]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "doesn't match password!")]
        [Display(Name = "Confirm Password:")]
        public string ConfirmPassword { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        public string FullName()
        {
            return FirstName + " " + LastName;
        }

        /* 
        Relationships and navigation properties. Navigation properties are the
        properties that have another model as their data type.
        
        Navigation properties will be null unless you use .Include
        */

        public List<Image> LikedImages { get; set; }

        public List<ImageUserLike> ImageUserLikes { get; set; }

    }
}