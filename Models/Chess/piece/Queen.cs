using System.Collections.Generic;

namespace ChessGame.Models.Chess.piece
{
    public class Queen : Piece
    {
        private readonly int BaseValue = 900;

        private static readonly int[][] QueenValue =
        {
            new[] { -20, -10, -10, -5, -5, -10, -10, -20 },
            new[] { -10, 0, 0, 0, 0, 0, 0, -10 },
            new[] { -10, 0, 5, 5, 5, 5, 0, -10 },
            new[] { -5, 0, 5, 5, 5, 5, 0, -5 },
            new[] { 0, 0, 5, 5, 5, 5, 0, -5 },
            new[] { -10, 5, 5, 5, 5, 5, 0, -10 },
            new[] { -10, 0, 5, 0, 0, 0, 0, -10 },
            new[] { -20, -10, -10, -5, -5, -10, -10, -20 }
        };

        private static readonly int[][] QueenValueReverse = Reverse(QueenValue);

        public override int GetValue(Color color)
        {
            return base.GetValue(color, BaseValue, QueenValue, QueenValueReverse);
        }

        public Queen(Color color, Square square) : base(color, square)
        {
        }

        public override string ToString()
        {
            return base.color.ToDescriptionString() + "Q";
        }

        public override void GeneratePossibleMove()
        {
            Square thisSquare = base.square;
            if (thisSquare == null) return;

            possibleMoves.Clear();
            Rook rook = new Rook(base.color, base.square);
            Bishop bishop = new Bishop(base.color, base.square);
            rook.GeneratePossibleMove();
            bishop.GeneratePossibleMove();

            base.possibleMoves.AddRange(rook.possibleMoves);
            base.possibleMoves.AddRange(bishop.possibleMoves);
        }
    }
}