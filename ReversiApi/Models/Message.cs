using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReversiApi.Models
{
    public class Message
    {
        public string Status { get; set; }
        public string Description { get; set; }

        public string Data { get; set; }
    }
}
