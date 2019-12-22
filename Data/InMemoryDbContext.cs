using Microsoft.EntityFrameworkCore;
using ChessGame.Models;

namespace ChessGame.Data
{
    public class InMemoryDbContext : DbContext
    {
        public InMemoryDbContext(DbContextOptions<InMemoryDbContext> options)
            : base(options)
        {
        }

        public DbSet<Board> boards { get; set; }
    }
}
