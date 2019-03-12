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
        public string Username { get; set; }
        public string Email { get; set; }

        private string Password { get; set; }
        public string HashedPwd { get; set; }

        public string Usertoken { get; set; }

        //public string RemeberToken { get; set; }

    }
}
