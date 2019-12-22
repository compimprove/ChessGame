using System.ComponentModel.DataAnnotations;

namespace ChessGame.Models
{
    public class Board
    {
        public long Id { get; set; }
        
        public string? User1Identifier { get; set; }
        public string? User1Name { get; set; }
        public string? User2Identifier { get; set; }
        public string? User2Name { get; set; }
    }
}
