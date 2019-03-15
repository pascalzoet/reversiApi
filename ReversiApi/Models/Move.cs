using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReversiApi.Models
{
    public class Move
    {
        public int MoveX { get; set; }

        public int MoveY { get; set; }

        public string GameToken { get; set; }

        public string PlayerToken { get; set; }

        public bool Pass { get; set; }

        public bool SetIsValid { get; set; }

        public bool SkippedTurn { get; set; }
    }
}
