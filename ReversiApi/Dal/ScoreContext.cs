using Microsoft.EntityFrameworkCore;
using ReversiApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReversiApi.Dal
{
    public class ScoreContext : DbContext
    {
        public ScoreContext(DbContextOptions<ScoreContext> options) : base(options) { }

        public DbSet<Score> Score { get; set; }
    }
}
