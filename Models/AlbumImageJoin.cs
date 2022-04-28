using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Pix.Models
{
    public class AlbumImageJoin
    {
        [Key] //primary key 

        public int AlbumImageJoinId {get; set;}

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
 


        //Foreing Keys
        public int ImageId {get; set;}

        public int AlbumId {get;set;}

        //Navigation Properties 

        public Image Image {get;set;}

        public Album Album {get;set;}
    }
}