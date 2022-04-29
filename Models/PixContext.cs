using Microsoft.EntityFrameworkCore;

namespace Pix.Models
{
    public class PixContext : DbContext
    {
        public PixContext(DbContextOptions options) : base(options) { }

        // for every model / entity that is going to be part of the db
        // the names of these properties will be the names of the tables in the db
        public DbSet<User> Users { get; set; }

        public DbSet<Image> Images { get; set; }

        public DbSet<Album> Albums {get;set;}

        public DbSet<AlbumImageJoin> AlbumImageJoins {get;set;}


        public DbSet<ImageUserLike> ImageUserLikes { get; set; }

        // public DbSet<Widget> Widgets { get; set; }
        // public DbSet<Item> Items { get; set; }
    }

}