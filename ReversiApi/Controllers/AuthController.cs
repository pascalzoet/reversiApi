using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ReversiApi.Dal;
using ReversiApi.Models;

namespace ReversiApi.Controllers
{
    public class AuthController : Controller
    {
        private readonly PlayerContext _context;

        public const string SessionString = "UserLoggedIn";
        public const string SessionInteger = "ReversiSessionInteger";
        public const string SessionByte = "ReversieSessionByte";

        public AuthController(PlayerContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("login")]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [Route("login")]
        public IActionResult Login(string username, string password)
        {
            if (ModelState.IsValid)
            {
                var user = _context.Player.Where(p => p.Username.Equals(username)).First();

                if (user == null)
                {
                    return View(nameof(Login));
                }
                bool verifyPwd = BCrypt.Net.BCrypt.Verify(password, user.HashedPwd);

                if (verifyPwd == true)
                {
                    //set session
                    HttpContext.Session.SetString(SessionString, user.Username);
                    return Redirect("/test");

                } else
                {
                    return View(nameof(Login));
                }
            }
            return View(nameof(Login));
        }

        [HttpGet]
        [Route("register")]
        public IActionResult Register()
        {
            return View();
        }


        [HttpPost]
        [Route("register")]
        public IActionResult Register(string username, string email, string password)
        {
            //check if user exist
            var user = _context.Player.Where(p => p.Email == email);
            if (user.Count() > 0)
            {
                ViewData["message"] = "Email bestaat al";
                return View();
            }
            else
            {
                _context.Player.Add(new Player
                {
                    Email = email,
                    Username = username,
                    HashedPwd = HassPwd(password)
                });
                _context.SaveChanges();
            }
            return View(nameof(Login));
        }

        private string HassPwd(string password)
        {
            string hash = BCrypt.Net.BCrypt.HashPassword(password, BCrypt.Net.BCrypt.GenerateSalt(12));
            return hash;
        }
    }
}