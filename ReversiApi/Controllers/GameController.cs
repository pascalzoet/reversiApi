using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ReversiApi.Dal;
using ReversiApi.Models;

namespace ReversiApi.Controllers
{
    [ApiController]
    public class GameController : Controller
    {
        private readonly GameContext _context;

        private Game Game { get; set; }

        private UserManager _manger { get; set; }

        public GameController(GameContext context, IConfiguration config)
        {
            _context = context;
            _manger = new UserManager(config);
        }

        /*
         * Get the game details based on the game token
         * return json
         */
        [Route("api/game/{token}")]
        public ActionResult GetGame(string token)
        {
            Message message = new Message();
            //check if there is a user loggedin
            //UserManager manger = new UserManager();
            //var user = manger.IsLoggedIn(HttpContext);

            var game = _context.Game
                .Where(g => g.GameToken == token)
                .FirstOrDefault();

            if (game == null)
            {
                message.Status = "error";
                message.Description = "Invalid token";
            } else if (game.GameStatus ==  "waiting" || game.CanTurn() == true)
            {
                message.Status = "ok";
                message.Description = "game data geladen";
                message.Data = JsonConvert.SerializeObject(game);
            } else
            {
                message.Status = "error";
                message.Description = "Geen zetten meer mogelijk, game is afgelopen";
                message.Data = JsonConvert.SerializeObject(game);
            }
            return Json(message);
        }

        [Route("api/game/{token}/players")]
        public ActionResult GetPlayers(string token)
        {
            Message message = new Message();
            var game = _context.Game.Where(g => g.GameToken == token).FirstOrDefault();

            if (game == null)
            {
                message.Status = "error";
                message.Description = "invalid token";
            } else
            {
                if (game.PlayerBlackToken == null || game.PlayerWhiteToken == null)
                {
                    message.Status = "error";
                    message.Description = "nog geen tegenstander gevonden";
                } else
                {
                    message.Status = "ok";
                    message.Description = "Tegenstander gevonden";
                    message.Data = JsonConvert.SerializeObject(message);
                }
            }
            return Json(message);
        }

        /*
         * Request to set a move
         */
        [HttpPut]
        [Route("api/game/move")]
        public ActionResult SetTurn([FromBody] Move move)
        {
            Message message = new Message();
            this.Game = _context.Game.Where(g => g.GameToken == move.GameToken).FirstOrDefault();
            var user = _manger.GetLoggedinUser(HttpContext);

            if (this.Game == null)
            {
                message.Description = "invalid token";
                message.Status = "error";
            } else
            {
                //check if it is your turn
                string TokenFroWhoIs;
                if (this.Game.OnSet == 1)
                {
                    TokenFroWhoIs = this.Game.PlayerWhiteToken;
                }
                else
                {
                    TokenFroWhoIs = this.Game.PlayerBlackToken;
                }
                if (user.UserToken == TokenFroWhoIs)
                {
                    var turn = this.Game.SetTurn(move);

                    if (turn.SetIsValid == false)
                    {
                        message.Status = "error";
                        message.Description = "zet is niet geldig";
                    } else
                    {
                        message.Status = "ok";
                        message.Description = "Zet gedaan";
                    }
                    message.Data = JsonConvert.SerializeObject(Game);

                    _context.SaveChanges();
                } else
                {
                    message.Status = "ok";
                    message.Description = "het is niet jou zet";
                    move.SetIsValid = false;
                    move.SkippedTurn = false;
                    message.Data = JsonConvert.SerializeObject(Game);
                }
            }
            return Json(message);
        }
    }
}