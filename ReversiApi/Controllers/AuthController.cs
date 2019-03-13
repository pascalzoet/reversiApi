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
        UserManager _userManger = new UserManager();

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

        [HttpPost, ActionName("login")]
        [Route("login")]
        public async Task<IActionResult> LoginAsync(Player player)
        {
            if (!ModelState.IsValid) return View(player);
            try
            {
                bool ingelogd = await _userManger.SignIn(HttpContext, player);
                if (ingelogd)
                    return RedirectToAction("Dashboard", "Dashboard");
                else return RedirectToAction("Dashboard", "Dashboard");

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("summary", ex.Message);
                return View(User);
            }
        }

        [HttpGet]
        [Route("register")]
        public IActionResult Register()
        {
            return View();
        }


        [HttpPost, ActionName("register")]
        [Route("register")]
        public ActionResult Register(Player player)
        {
            if (!ModelState.IsValid)
            {
                return View(player);
            }
            try
            {
                //check if the user allready exist in our database
                bool exist = _userManger.UserExist(HttpContext, player);
                if(exist)
                {
                    return RedirectToAction("login", "Auth");
                }
                else
                {
                    bool registerd = _userManger.RegisterAsync(HttpContext, player);
                    if (registerd)
                    {
                        return RedirectToAction("login", "Auth");
                    } else
                    {
                        return View(player);
                    }
                }
            } catch (Exception ex)
            {
                ModelState.AddModelError("summary", ex.Message);
                return View(player);
            }
        }

        [HttpGet]
        [Route("logout")]
        public IActionResult Logout()
        {
            _userManger.SignOut(HttpContext);
            return RedirectToAction("login", "Auth");
        }

        private string HassPwd(string password)
        {
            string hash = BCrypt.Net.BCrypt.HashPassword(password, BCrypt.Net.BCrypt.GenerateSalt(12));
            return hash;
        }
    }
}