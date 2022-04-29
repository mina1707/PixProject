
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

        /* 
        Relationships and navigation properties. Navigation properties are the
        properties that have another model as their data type.
        
        Navigation properties will be null unless you use .Include
        */


        [NotMapped]
        [Display(Name ="Upload File:")]
        public IFormFile ImageFile { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        // Navigational PROPs

        // 1 User : to Many Images for creating images
        public int UserId { get; set; }
        public User Uploader { get; set; }

        //ManyToMany

        public List<AlbumImageJoin> AlbumImageJoins {get;set;}
        // Many User to Many Image for LIKING images
        public List<ImageUserLike> ImageUserLikes { get; set; }
    }
}