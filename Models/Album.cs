using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Pix.Models
{
    public class Album
    {
        [Key]
        public int AlbumId {get;set;}


        [Display(Name ="Album Name:")]
        public string AlbumName {get; set;}

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        //Relationships

        //1 user: many albums
        public int UserId { get; set; }

        //!!!!!!!!!!!!!!!!!NOT SURE 
        public User Uploader {get;set;}

        
        //ManyToMany

        public List<AlbumImageJoin> AlbumImageJoins {get;set;}

    }
}