using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ReversiApi.Models
{
    public class Game
    {
        [Key]
        public int GameId { get; set; }
        public string Description { get; set; }
        public string GameToken { get; set; }

        public string PlayerWhiteToken { get; set; }
        public string PlayerBlackToken { get; set; }

        public int OnSet { get; set; }

        public int? Winner { get; set; }

        public enum Status { inProgress, Finished };

        public string Board { get; set; }

        public static string CreateBoard()
        {
            int[,] board = new int[8, 8]
            {
                {0,0,0,0,0,0,0,0 },

                {0,0,0,0,0,0,0,0 },
                {0,0,0,0,0,0,0,0 },

                {0,0,0,1,2,0,0,0 },
                {0,0,0,2,1,0,0,0 },

                {0,0,0,0,0,0,0,0 },
                {0,0,0,0,0,0,0,0 },

                {0,0,0,0,0,0,0,0 },
            };

            return JsonConvert.SerializeObject(board);
        }

        public static string CreateGameToken()
        {
            string token = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
            return token.Replace(@"\", "");
        }

        /*
         * Deserialize the board object and change the value based on the given input from Move class
         * @TODO: apply logic
         */
        public int[,] SetTurn(Move move)
        {
            var board = JsonConvert.DeserializeObject<int[,]>(this.Board);
            board[move.MoveX, move.MoveY] = this.OnSet;
            this.Board = JsonConvert.SerializeObject(board);
            if (this.OnSet == 1)
            {
                this.OnSet = 2;
            } else
            {
                this.OnSet = 1;
            }
            return board;
        }
    }
}
