using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

        public GameController(GameContext context)
        {
            _context = context;
        }
       

        /*
         * Get the game details based on the game token
         * return json
         */
        [Route("api/game/{token}")]
        public ActionResult GetGame(string token)
        {
            var game = _context.Game
                .Where(g => g.GameToken == token)
                .Select(g => new { g.GameToken, g.Description, g.Board, g.PlayerBlackToken, g.PlayerWhiteToken, g.Winner})
                .FirstOrDefault();

            if (game == null)
            {
                return Json("Deze gametoken is niet geldig");
            }
            return Json(game);
        }

        [HttpPut]
        [Route("api/game/move")]
        public ActionResult SetTurn([FromBody] Move move)
        {
            this.Game = _context.Game.Where(g => g.GameToken == move.GameToken).FirstOrDefault();
          
            if (this.Game == null)
            {
                return Json("invallid token");
            } else
            {
                Json(this.Game.SetTurn(move));

                _context.SaveChanges();
                return Json(this.Game);
            }
        }
    }
}