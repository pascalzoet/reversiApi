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

        public string GameStatus { get; set; }

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
            return token.Replace(@"\", "").Replace(@"+", "").Replace(@"&", "").Replace(@"/", "");
        }

        private int[,] TempBoard { get; set; }

        /*
         * Deserialize the board object and change the value based on the given input from Move class
         * @TODO: apply logic
         */
        public int[,] SetTurn(Move move)
        {
            var board = JsonConvert.DeserializeObject<int[,]>(this.Board);
            this.TempBoard = board;
            if (this.CheckAround(move.MoveX, move.MoveY) == true)
            {
                this.TempBoard[move.MoveX, move.MoveY] = this.OnSet;

                CheckToFlip(move.MoveX, move.MoveY);
                this.Board = JsonConvert.SerializeObject(this.TempBoard);
                if (CanTurn())
                {
                    GiveTurn();
                }
            }
            return board;
        }

        /*
         * Check if there is atleast 1 stone near the desired place that is the oposist color
         */
        private bool CheckAround(int row, int col)
        {
            int ToCheck;
            if (this.OnSet == 1)
            {
                ToCheck = 2;
            } else
            {
                ToCheck = 1;
            }
            var board = this.TempBoard;

            //loop through the rows
            for (int rowDir = -1; rowDir <= +1; rowDir++)
            {
                for (int colDir = -1; colDir <= +1; colDir++)
                {
                    if (rowDir == 0 && colDir == 0)
                    {
                        continue;
                    }

                    int rowCheck = row + rowDir;
                    int colCheck = col + colDir;

                    bool ItemFound = false;

                    if (this.IsValidPosition(rowCheck, colCheck) == true)
                    {
                        if (board[rowCheck, colCheck] == ToCheck)
                        {
                            rowCheck += rowDir;
                            colCheck += colDir;
                            ItemFound = true;
                        }
                    }

                    if (ItemFound)
                    {
                        if (this.IsValidPosition(rowCheck, colCheck) && board[rowCheck, colCheck] == this.OnSet)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        private bool IsValidPosition(int row, int col)
        {
            return (row >= 0 && row <= 7) && (col >= 0 && col <= 7);
        }

        private void CheckToFlip(int row, int col)
        {
            List<Move> finalItems = new List<Move>();
            int ToCheck;
            if (this.OnSet == 1)
            {
                ToCheck = 2;
            }
            else
            {
                ToCheck = 1;
            }

            for (int rowDir = -1; rowDir <= +1; rowDir++)
            {
                for (int colDir = -1; colDir <= +1; colDir++)
                {
                    if (rowDir == 0 && colDir == 0)
                    {
                        continue;
                    }
                    int rowCheck = row + rowDir;
                    int colCheck = col + colDir;

                    List<Move> possibleItems = new List<Move>();

                    if (this.IsValidPosition(rowCheck, colCheck) == true && this.TempBoard[rowCheck, colCheck] == ToCheck)
                    {
                        possibleItems.Add(new Move()
                        {
                            MoveX = rowCheck,
                            MoveY = colCheck
                        });

                        rowCheck += rowDir;
                        colCheck += colDir;
                    }
                    if (possibleItems.Count() > 0)
                    {

                        if (this.IsValidPosition(rowCheck, colCheck) && this.TempBoard[rowCheck, colCheck] == this.OnSet)
                        {
                            finalItems.Add(new Move()
                            {
                                MoveY = colCheck,
                                MoveX = rowCheck
                            });

                            foreach (var item in possibleItems)
                            {
                                finalItems.Add(item);
                            }
                        }
                    }
                }
            }

            if (finalItems.Count() > 0)
            {
                foreach (var item in finalItems)
                {
                    this.Flip(item);
                }
            }
        }

        private void Flip(Move move)
        {
            if (IsValidPosition(move.MoveX, move.MoveY)) {

                this.TempBoard[move.MoveX, move.MoveY] = this.OnSet;
            }
        }

        private bool CanTurn()
        {
            for (int row = 0; row <= 7; row++)
            {
                for (int col = 0; col <= 7; col++)
                {
                    if (CheckAround(row, col)) {
                        return true;
                    }
                }
            }
            return false;
        }

        private void GiveTurn()
        {
            if (this.OnSet == 1)
            {
                this.OnSet = 2;
            } else
            {
                this.OnSet = 1;
            }
        }
    }

}
    