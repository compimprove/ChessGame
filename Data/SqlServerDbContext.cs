using Microsoft.EntityFrameworkCore;
using ChessGame.Models;

namespace ChessGame.Data
{
    public class SqlServerDbContext : DbContext
    {
        public SqlServerDbContext(DbContextOptions<SqlServerDbContext> options)
            : base(options)
        {
        }
        public DbSet<Board> boards { get; set; }
    }
}
