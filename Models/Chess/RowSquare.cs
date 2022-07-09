using System.Collections.Generic;
using System.Linq;

namespace ChessGame.Models.Chess
{
    public class RowSquare
    {
        private Square[] squares { get; set; } = new Square[8];

        public Square this[int i]
        {
            get => squares[i];
            set => squares[i] = value;
        }

        public int GetValue(Color color)
        {
            return squares.Sum(square => square.piece?.GetValue(color) ?? 0);
        }

        public override string ToString()
        {
            return string.Join(" ", squares.Select(s => s.ToString()));
        }

        public List<Piece> getColorPieces(Color ofColor)
        {
            return squares.Where(s => s.piece?.color == ofColor).Select(s => s.piece).ToList();
        }
    }
}