using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Pix.Models
{
    public class ImageUserLike
    {
        [Key]
        public int ImageUserLikeId { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        /* 
        Relationships and navigation properties. Navigation properties are the
        properties that have another model as their data type.
        
        Navigation properties will be null unless you use .Include
        */

        public int UserId { get; set; }
        public User User { get; set; }
        public int ImageId { get; set; }
        public Image Image { get; set; }
    }
}