
using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Web;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pix.Models
{
    public class Image
    {
        [Key]
        public int ImageId { get; set; }

        [Display(Name ="Title:")]
        public string Title { get; set; }
        [Display(Name ="Comment:")]
        public string Comment { get; set; }
        
        [Display(Name ="Image Name:")]
        public string ImageName { get; set; }

        // public string ImagePath { get; set; }

        public int UserId { get; set; }

        [NotMapped]
        [Display(Name ="Upload File:")]
        public IFormFile ImageFile { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        // Navigational PROPs

        public User Uploader { get; set; }
    }
}