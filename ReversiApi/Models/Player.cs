using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ReversiApi.Models
{
    public class Player
    {
        [Key]
        public int PlayerId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }

        public string Password { get; set; }
        public string UserRole { get; set; }
        public string UserToken { get; set; }
    }
}
