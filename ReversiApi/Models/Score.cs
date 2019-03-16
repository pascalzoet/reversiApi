using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ReversiApi.Models
{
    public class Score
    {
        [Key]
        public int ScoreId { get; set; }

        public string GameToken { get; set; }
        public string WinnerToken { get; set; }

        public int PlayerWhiteScore { get; set; }
        public int PlayerBlackScore { get; set; }
    }
}
