using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Pix.Models;
using Microsoft.AspNetCore.Http;

namespace Pix.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private PixContext db;

        public HomeController(PixContext context, ILogger<HomeController> logger)
        {
            db = context;
            _logger = logger;
        }

        public IActionResult Index()
        {
            if (HttpContext.Session.GetInt32("UserId") != null )
            {
                return RedirectToAction("AddImg", "ImageUploads");
            }
            return View("Index");
        }

        [HttpGet("/success")]
        public IActionResult Success()
        {
            if (HttpContext.Session.GetInt32("UserId") == null )
            {
                return RedirectToAction("Index");
            }
            int loggedInUser = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            User user = db.Users.FirstOrDefault(u => u.UserId == loggedInUser);
            return View(user);
        }

        /* ---------------------------------------------------------------------+
        |                   POST REQUESTS                                       |
        +----------------------------------------------------------------------*/
        [HttpPost("/register")]
        public IActionResult Register(User newUser)
        {
            bool isEmailTaken = db.Users.Any( u => u.Email == newUser.Email);

            if (isEmailTaken)
            {
                ModelState.AddModelError("Email", "Email already in use!");
            }
            // MORE VALIDATIONS IF NEEDED   
            

            if (ModelState.IsValid ==  false)
            {
                // Display any errors
                return View("Index");
            }

            PasswordHasher<User> hasher = new PasswordHasher<User>();
            newUser.Password = hasher.HashPassword(newUser, newUser.Password);

            db.Users.Add(newUser);
            db.SaveChanges();

            HttpContext.Session.SetInt32("UserId", newUser.UserId);
            HttpContext.Session.SetString("FullName", newUser.FullName());
            return RedirectToAction("AddImg","ImageUploads");
        }

        [HttpPost("/login")]
        public IActionResult Login(LoginUser loginUser)
        {
            if (ModelState.IsValid == false)
            {
                return View("Index");
            }
            User dbUser = db.Users.FirstOrDefault( u => u.Email == loginUser.LoginEmail);
            if (dbUser == null)
            {
                ModelState.AddModelError("LoginEmail", "not a valid email!");
                return View("Index");
            }

            PasswordHasher<LoginUser> hasher = new PasswordHasher<LoginUser>();
            PasswordVerificationResult comparePasswords = hasher.VerifyHashedPassword(loginUser, dbUser.Password, loginUser.LoginPassword);

            if (comparePasswords == 0)
            {
                ModelState.AddModelError("LoginPassword", "is not valid!");
                return View("Index");
            }

            HttpContext.Session.SetInt32("UserId", dbUser.UserId);
            HttpContext.Session.SetString("FullName", dbUser.FullName());
            return RedirectToAction("AddImg", "ImageUploads");

        }

        [HttpPost("/logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
