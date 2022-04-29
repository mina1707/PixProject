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
using Microsoft.EntityFrameworkCore;

namespace Pix.Controllers
{

    public class ImageUploadsController : Controller
    {
        private readonly IWebHostEnvironment _hostEnvironment;

        private PixContext db;
        
        public ImageUploadsController(PixContext context, IWebHostEnvironment hostEnvironment)
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


        [HttpGet("/dashboard")]
        public IActionResult AllImages()
        {
            if (uid != null )
            {
                var result = db.Images
                .Include(u => u.Uploader)
                .Include(i => i.ImageUserLikes)
                .OrderByDescending(t => t.CreatedAt)
                .ToList();
                // ViewBag.result = result;

                var user = db.Users
                .FirstOrDefault(a => a.UserId == HttpContext.Session.GetInt32("UserId"));
                ViewBag.user = user;

                 List<Album> allalbums = db.Albums
                .Where(a => a.UserId == HttpContext.Session.GetInt32("UserId"))
                .ToList();
                ViewBag.AllAlbums = allalbums;
                
                return View("Dashboard",result);
            }
            
            return View("Index");

        }


        [HttpGet("/images/add")]
        public IActionResult AddImg()
        {
            if (uid == null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }


        /* ---------------------------------------------------------------------+
        |                   POST REQUESTS                                       |
        +----------------------------------------------------------------------*/

        [HttpPost("/images/create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ImageId,Title, Comment ,ImageFile")]Image image)
        {
            if (uid == null)
            {
                return RedirectToAction("Index", "Home");
            }
            if (ModelState.IsValid == false)
            {
                return View("AddImg");
            }

            if (ModelState.IsValid)
            {
                string wwwRootPath = _hostEnvironment.WebRootPath;
                string fileName = Path.GetFileNameWithoutExtension(image.ImageFile.FileName);
                string extension = Path.GetExtension(image.ImageFile.FileName);
                image.ImageName = fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                string path = Path.Combine(wwwRootPath + "/Images/", fileName);
                using (var fileStream = new FileStream(path,FileMode.Create))
                {
                    await image.ImageFile.CopyToAsync(fileStream);
                }

                image.UserId = (int)uid;
                db.Add(image);
                await db.SaveChangesAsync();
                return RedirectToAction("AllImages");
            }

            return RedirectToAction("AddImg");
        }

            [HttpPost("/images/{imageId}/addImageToAlbum")]
           public IActionResult AddImageToAlbum(int imageId, [FromForm] int albumId)
           {
               AlbumImageJoin join = new AlbumImageJoin();
               join.ImageId = imageId;
               join.AlbumId = albumId;

               db.AlbumImageJoins.Add(join);
               db.SaveChanges();
               
               return RedirectToAction("AllImages");

           }
           
        [HttpPost("/images/{imageId}/like")]
        public IActionResult Like(int imageId)
        {
            if (uid == null)
            {
                return RedirectToAction("Index","Home");
            }

            ImageUserLike existingLike = db.ImageUserLikes
            .FirstOrDefault( l => l.ImageId == imageId && l.UserId == uid);

            if (existingLike == null)
            {
                ImageUserLike newLike = new ImageUserLike()
                {
                    UserId = (int)uid,
                    ImageId = imageId
                };
                db.ImageUserLikes.Add(newLike);
            }
            else
            {
                db.Remove(existingLike);
            }
            db.SaveChanges();
            return RedirectToAction("AllImages");

        }





        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}