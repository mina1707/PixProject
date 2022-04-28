using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Pix.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System.ComponentModel.DataAnnotations;

namespace Pix.Controllers
{

    public class AlbumsController : Controller
    {
        private readonly IWebHostEnvironment _hostEnvironment;

        private PixContext db;
        
        public AlbumsController(PixContext context, IWebHostEnvironment hostEnvironment)
        {
            db = context;
            this._hostEnvironment = hostEnvironment;
        }

        private int? uid
        {
            get
            {
                return HttpContext.Session.GetInt32("UserId");
            }
        }

        /* ---------------------------------------------------------------------+
        |                   GET REQUESTS                                       |
        +----------------------------------------------------------------------*/


        [HttpGet("/albums")]
        public IActionResult AllAlbums()
        {
            if (HttpContext.Session.GetInt32("UserId") == null )
            {
               return RedirectToAction("Index", "Home");
            }
            
            List<Album> allalbums= db.Albums
            .Where( a => a.UserId == HttpContext.Session.GetInt32("UserId"))
            .OrderBy(a => a.CreatedAt)
            .ToList(); 
           return View("AllAlbums", allalbums); 

        }

        [HttpGet("/albums/new")]

        public IActionResult NewAlbum()
        {
             if (uid == null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View("NewAlbum");
        }


        [HttpPost("/albums/create")]

        public IActionResult CreateAlbum(Album album) //--> Wedding  is the model and "wedding" is the new trip we are submitting and passing.
        {
             if (uid == null) 
             {
                return RedirectToAction("Index", "Home");
            }

            //Here, we have to make the relationhips between the Uploader and the album.
            album.UserId = (int)uid;
            db.Albums.Add(album);
            db.SaveChanges();
            return RedirectToAction("AllAlbums");
            }

             [HttpPost("/images/{imageId}/join")]
           public IActionResult Join(int albumId)
           {
                if (uid == null)
               {
               return RedirectToAction("Index", "Home");
               }
 
               AlbumImageJoin existingJoin = db.AlbumImageJoins
               .FirstOrDefault(j => j.AlbumId == albumId && j.UserId == uid );
 
               if( existingJoin == null)
               {
                   AlbumImageJoin newJoin = new AlbumImageJoin()
                   {
                       UserId = (int)uid,
                       AlbumId = albumId
                   };
 
               db.Album.Add(newJoin);
                  
               }
               else{
 
                   db.Remove(existingJoin);
               }
 
               db.SaveChanges();
               //RedirectToAction("Details", new{paramName1= paramValue1, paramName2= paramValue2 })
               return RedirectToAction("AllAlbums", new {albumId= albumId});
           }

    }

}

  