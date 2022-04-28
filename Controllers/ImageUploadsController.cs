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
            if (HttpContext.Session.GetInt32("UserId") != null )
            {
                var result = db.Images.ToList();
                // HttpContext.Session.GetInt32("UserFir") = User.FirstName;
                return View("Dashboard", result);
            }
            
            return View("Index");

        }


        [HttpGet("/images/add")]
        public IActionResult AddImg()
        {
            return View();
        }


        /* ---------------------------------------------------------------------+
        |                   POST REQUESTS                                       |
        +----------------------------------------------------------------------*/

        [HttpPost("/images/create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ImageId,Title, Comment ,ImageFile")]Image image)
        {
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
                return RedirectToAction("AddImg");
            }

            return RedirectToAction("AllImages");
        }








        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}