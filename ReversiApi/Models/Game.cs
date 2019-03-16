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

        public string Name { get; set; }
        public string Description { get; set; }

        public string GameToken { get; set; }

        public string PlayerWhiteToken { get; set; }
        public string PlayerBlackToken { get; set; }

        public int OnSet { get; set; }

        public int? Winner { get; set; }

        public string GameStatus { get; set; }

        public string Board { get; set; }

        private int[,] TempBoard { get; set; }

        public static string CreateBoard()
        {
            //make a grid from 8 by 8
            //set the 4 middle places with black and white so you can start the game
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

            //make the array to a json string so we can store the board in the database
            return JsonConvert.SerializeObject(board);
        }

        public static string CreateGameToken()
        {
            //create a random token that alwasy ands with ==
            string token = Convert.ToBase64String(Guid.NewGuid().ToByteArray());

            //return the token and replace a few characters that will break the url
            return token.Replace(@"\", "").Replace(@"+", "").Replace(@"&", "").Replace(@"/", "");
        }


        /*
         * Deserialize the board object and change the value based on the given input from Move class
         * move the stone and flip stones that are taken by the move 
         * Give back the board afterwards
         * Return the move object with the message if the pass is applyed or it was an invalid move
         */
        public Move SetTurn(Move move)
        {
            var board = JsonConvert.DeserializeObject<int[,]>(this.Board);
            //store the board temporary in an array so other methods can change the board
            this.TempBoard = board;

            //check around for stone from oponent
            if (this.CheckIfCanMove(move.MoveX, move.MoveY) == true)
            {
                move.SkippedTurn = false;
                //set the stone that got requested by the player
                this.TempBoard[move.MoveX, move.MoveY] = this.OnSet;

                //check and steal stones from your oponent
                CheckToFlip(move.MoveX, move.MoveY);

                //set the temporary board back to a string
                this.Board = JsonConvert.SerializeObject(this.TempBoard);

                //give the turn to the oponent and check if he can move
                GiveTurn();

                //if the oponent cant place a move give the turn back and check if the previous player can make a move
                move.SkippedTurn = false;
                if (!CanTurn())
                {
                    //give the turn back to original player
                    GiveTurn();
                    move.SkippedTurn = true;

                    //if he can, he will be onset otherwise we end the game
                    if (!CanTurn())
                    {
                        //end the game and calculate the scores
                        move.SkippedTurn = true;
                        this.GameStatus = "finished";
                    }
                }
                move.SetIsValid = true;
            } else
            {
                if (!CanTurn())
                {
                    this.GameStatus = "finished";
                    this.Winner = 1;
                }
                move.SetIsValid = false;
            }
            return move;
        }

        /*
         * Check if there is atleast 1 stone around the desired place of move and that is the oponents color
         * if there is atleast 1 stone available check if the stone next to that one is your color
         * loop will continue until each spot is checked
         * return True | False
         */
        private bool CheckIfCanMove(int row, int col)
        {
            int ToCheck = this.WhoIsOponent();
            //check who is onset and who is the oponent
           

            //collect the temporary board and store it in a variable we do not need to modify
            var board = JsonConvert.DeserializeObject<int[,]>(this.Board);

            if (!this.IsValidPosition(row, col) || this.IsVisible(row, col) )
            {
                return false;
            }

            //loop through the rows
            for (int rowDir = -1; rowDir <= +1; rowDir++)
            {
                for (int colDir = -1; colDir <= +1; colDir++)
                {
                    if (rowDir == 0 && colDir == 0)
                    {
                        continue;
                    }
                    //set the current x and y coordinates
                    int rowCheck = row + rowDir;
                    int colCheck = col + colDir;

                    bool ItemFound = false;

                    //place is inside the board boundaries
                    while (this.IsValidPosition(rowCheck, colCheck) == true && board[rowCheck, colCheck] == ToCheck && this.IsVisible(rowCheck, colCheck))
                    {
                            //move a place to the right and down
                            rowCheck += rowDir;
                            colCheck += colDir;

                            //we found a item that is from the oponent
                            ItemFound = true;
                    }
                    //if we have a found a stone from the oponent
                    if (ItemFound)
                    {
                        //check if the place next to the found stone is inside the board and is from the curren onset player
                        if (this.IsValidPosition(rowCheck, colCheck) && board[rowCheck, colCheck] == this.OnSet && this.IsVisible(rowCheck, colCheck))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        /*
         * Validate if the given position is in the board
         * Negative numbers or numbers higher than 7 are outside of the board
         */
        private bool IsValidPosition(int row, int col)
        {
            return (row >= 0 && row <= 7) && (col >= 0 && col <= 7);
        }

        private bool IsVisible(int row, int col)
        {
            var board = JsonConvert.DeserializeObject<int[,]>(this.Board);

            if (board[row, col] == 0)
            {
                return false;
            }
            return true;
        }

        /*
         * Loop through the entire board starting from the place where the stone got placed
         * Each stone that is closed in by the current players color gets changed to the color
         * from the current player
         * Loop will chech horizontal, vertical and diagonal
         */
        private void CheckToFlip(int row, int col)
        {
            //list to store all the stones in that got flipped
            List<Move> finalItems = new List<Move>();

            //check again who is
            int ToCheck = this.WhoIsOponent();

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

                    while (this.IsValidPosition(rowCheck, colCheck) == true && this.TempBoard[rowCheck, colCheck] == ToCheck && this.IsVisible(rowCheck, colCheck))
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

                        if (this.IsValidPosition(rowCheck, colCheck) && this.TempBoard[rowCheck, colCheck] == this.OnSet && this.IsVisible(rowCheck, colCheck))
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
        
        /*
         * Change the value of the x/y position to the current players color
         */
        private void Flip(Move move)
        {
            //check if the place is inside the field boundaries
            if (IsValidPosition(move.MoveX, move.MoveY)) {

                //set the place to the current onset player
                this.TempBoard[move.MoveX, move.MoveY] = this.OnSet;
            }
        }

        /*
         * Check if the next player can actualy take a turn
         * If no turns are available
         * the turn wil not be givin to the next player
         */
        public bool CanTurn()
        {
            for (int row = 0; row <= 7; row++)
            {
                for (int col = 0; col <= 7; col++)
                {
                    if (CheckIfCanMove(row, col)) {
                        return true;
                    }
                }
            }
            return false;
        }

        /*
         * Method the check who is the current oponent
         */
        private int WhoIsOponent()
        {
            if (this.OnSet == 1)
            {
                return 2;
            }
            else
            {
                return 1;
            }
        }

        /*
         * Give the turn to the oponent
         */
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

        public Score CalculateScore()
        {
            Score score = new Score();
            int ScoreWhite = 0;
            int ScoreBlack = 0;
            var board = JsonConvert.DeserializeObject<int[,]>(this.Board);

            for (int row = 0; row < 8; row++)
            {
                for (int col = 0; col < 8; col++)
                {
                    if (board[row, col] == 1)
                    {
                        ScoreWhite++;
                    } else if (board[row, col] == 2)
                    {
                        ScoreBlack++;
                    } else
                    {
                        //no score
                    }
                }
            }
            score.PlayerBlackScore = ScoreBlack;
            score.PlayerWhiteScore = ScoreWhite;
            score.GameToken = this.GameToken;

            if (ScoreWhite > ScoreBlack)
            {
                score.WinnerToken = this.PlayerWhiteToken;
            }
            else if (ScoreWhite == ScoreBlack)
            {
                score.WinnerToken = null;
            }
            else
            {
                score.WinnerToken = this.PlayerBlackToken;
            }

            return score;
        }
    }

}
    