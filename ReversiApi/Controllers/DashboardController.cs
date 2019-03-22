using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using ReversiApi.Dal;
using ReversiApi.Models;

namespace ReversiApi.Controllers
{
    public class DashboardController : Controller
    {
        private readonly GameContext _context;

        UserManager _userManger { get; set; }
        private Player Player { get; set; }

        public DashboardController(GameContext context, IConfiguration config)
        {
            _context = context;
            _userManger = new UserManager(config);
        }

        [Authorize]
        [HttpGet]
        [Route("/dashboard")]
        public IActionResult Dashboard()
        {
            Player = _userManger.GetLoggedinUser(HttpContext);
            //get player games
            Game game = _context.Game.Where(g => g.PlayerBlackToken == Player.UserToken || g.PlayerWhiteToken == Player.UserToken).Where(g => g.GameStatus != "finished").FirstOrDefault();
            if (game != null)
            {
                return RedirectToAction("Enter", "Dashboard", new { token = game.GameToken });
            }
            List<Game> games = _context.Game.Where(g => g.GameStatus == "waiting" && g.PlayerBlackToken != Player.UserToken && g.PlayerWhiteToken != Player.UserToken).ToList();
            ViewData["user"] = Player.UserName;
            ViewData["games"] = games;
            return View(game);
        }

        [Authorize]
        [HttpGet]
        [Route("/game/create")]
        public ActionResult Create()
        {
            Player = _userManger.GetLoggedinUser(HttpContext);
            //you can only create a new game if the are no other active games on your name
            var games = _context.Game.Where(g => g.PlayerBlackToken == Player.UserToken || g.PlayerWhiteToken == Player.UserToken).Where(g => g.GameStatus == "waiting" || g.GameStatus == "inprogress").FirstOrDefault();

            if (games == null)
            {
                //return the player to the pages for the new game
                return View();
            } else
            {
                return Redirect("/dashboard");
                //send the player to the game pages
            }
        }

        [Authorize]
        [HttpPost]
        [Route("/game/create")]
        public ActionResult Create(string Name, string Description)
        {
            Player = _userManger.GetLoggedinUser(HttpContext);
            _context.Game.Add(new Game()
            {
                Name = Name,
                Board = Game.CreateBoard(),
                GameStatus = "waiting",
                OnSet = 1,
                PlayerWhiteToken = Player.UserToken,
                Winner = null,
                Description = Description,
                GameToken = Game.CreateGameToken(),
                PlayerBlackToken = null
            });
            _context.SaveChanges();
            return Redirect("/dashboard");
        }

        [Authorize]
        [HttpGet]
        [Route("/game/remove/{token}")]
        public ActionResult Remove(string token)
        {
            var game = _context.Game.Where(g => g.GameToken == token).FirstOrDefault();

            if (game == null)
            {
                return Redirect("/dasbhoard");
            } else
            {
                _context.Remove(game);
                _context.SaveChanges();
                return Redirect("/dashboard");
            }
        }

        [Authorize]
        [HttpGet]
        [Route("/game/enter/{token}")]
        public ActionResult Enter(string token)
        {
            var game = _context.Game.Where(g => g.GameToken == token).FirstOrDefault();
            if (game == null)
            {
                //token does not exist
                return Redirect("/dashboard");
            }
            if (game.GameStatus == "waiting")
            {
                return View("Waiting");
            }
            else
            {
                return Redirect("/#access_token=" + game.GameToken + "&");
            }
        }


        [Authorize]
        [HttpGet]
        [Route("/game/join/{token}")]
        public ActionResult Join(string token)
        {
            var game = _context.Game.Where(g => g.GameToken == token).FirstOrDefault();
            if (game == null)
            {
                //token does not exist
                return Redirect("/dashboard");
            }
            if (game.GameStatus == "waiting")
            {
                //join the game
                game.GameStatus = "inprogress";
                game.PlayerBlackToken = _userManger.GetLoggedinUser(HttpContext).UserToken;
                _context.SaveChanges();
                return Redirect("/#access_token=" + game.GameToken + "&");
            }
            if (game.GameStatus == "inprogress" && game.PlayerBlackToken == _userManger.GetLoggedinUser(HttpContext).UserToken || game.PlayerWhiteToken == _userManger.GetLoggedinUser(HttpContext).UserToken)
            {
                //game has started and its your game
                return Redirect("/#access_token=" + game.GameToken + "&");
            }
            return Redirect("/dashboard");
        }
    }
}