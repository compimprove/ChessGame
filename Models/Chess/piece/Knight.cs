using System.Collections.Generic;

namespace ChessGame.Models.Chess.piece
{
    public class Knight : Piece
    {
        private readonly int BaseValue = 320;

        private static readonly int[][] KnightValue =
        {
            new[] { -50, -40, -30, -30, -30, -30, -40, -50 },
            new[] { -40, -20, 0, 0, 0, 0, -20, -40 },
            new[] { -30, 0, 10, 15, 15, 10, 0, -30 },
            new[] { -30, 5, 15, 20, 20, 15, 5, -30 },
            new[] { -30, 0, 15, 20, 20, 15, 0, -30 },
            new[] { -30, 5, 10, 15, 15, 10, 5, -30 },
            new[] { -40, -20, 0, 5, 5, 0, -20, -40 },
            new[] { -50, -40, -30, -30, -30, -30, -40, -50 }
        };

        private static readonly int[][] KnightValueReverse = Reverse(KnightValue);

        public Knight(Color color, Square square) : base(color, square)
        {
        }

        public override int GetValue(Color color)
        {
            return base.GetValue(color, BaseValue, KnightValue, KnightValueReverse);
        }

        public override void GeneratePossibleMove()
        {
            Square thisSquare = this.square;
            if (thisSquare == null) return;

            int row = thisSquare.coord.row;
            int col = thisSquare.coord.col;
            GameBoard board = square.board;
            possibleMoves.Clear();

            List<Coord> possibleCoordMoves = new List<Coord>
            {
                new Coord(row - 2, col + 1),
                new Coord(row - 1, col + 2),
                new Coord(row + 1, col + 2),
                new Coord(row + 2, col + 1),
                new Coord(row + 2, col - 1),
                new Coord(row + 1, col - 2),
                new Coord(row - 1, col - 2),
                new Coord(row - 2, col - 1),
            };

            // Square have nothing or square have piece with its color different
            // than King's color
            foreach (Coord c in possibleCoordMoves)
            {
                Square square = board.GetSquare(c);
                if (square == null)
                    continue;
                if (square.isEmpty())
                    possibleMoves.Add(square);
                else if (square.piece.color != color)
                    possibleMoves.Add(square);
            }
        }

        public override string ToString()
        {
            return color.ToDescriptionString() + "N";
        }
    }
}