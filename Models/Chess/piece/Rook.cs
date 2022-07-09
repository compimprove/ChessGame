using System.Collections.Generic;

namespace ChessGame.Models.Chess.piece
{
    public class Rook : Piece
    {
        private readonly int BaseValue = 500;

        private static readonly int[][] RookValue =
        {
            new[] { 0, 0, 0, 0, 0, 0, 0, 0 },
            new[] { 5, 10, 10, 10, 10, 10, 10, 5 },
            new[] { -5, 0, 0, 0, 0, 0, 0, -5 },
            new[] { -5, 0, 0, 0, 0, 0, 0, -5 },
            new[] { -5, 0, 0, 0, 0, 0, 0, -5 },
            new[] { -5, 0, 0, 0, 0, 0, 0, -5 },
            new[] { -5, 0, 0, 0, 0, 0, 0, -5 },
            new[] { 0, 0, 0, 5, 5, 0, 0, 0 }
        };

        private static readonly int[][] RookValueReverse = Reverse(RookValue);

        public override int GetValue(Color color)
        {
            return base.GetValue(color, BaseValue, RookValue, RookValueReverse);
        }

        public Rook(Color color, Square square) : base(color, square)
        {
        }

        public override string ToString()
        {
            return base.color.ToDescriptionString() + "R";
        }

        public override List<Square> PossibleEatingMove()
        {
            this.GeneratePossibleMove();
            return this.possibleMoves;
        }

        public override void GeneratePossibleMove()
        {
            Square thisSquare = base.square;
            if (thisSquare == null) return;

            int row = thisSquare.coord.row;
            int col = thisSquare.coord.col;
            GameBoard board = square.board;
            possibleMoves.Clear();

            // all from this to the up
            for (int iRow = row - 1; iRow > -1; iRow--)
            {
                Square square = board.GetSquare(new Coord(iRow, col));
                if (square.isEmpty())
                {
                    possibleMoves.Add(square);
                }
                else if (square.piece.color != base.color)
                {
                    possibleMoves.Add(square);
                    break;
                }
                else break;
            }

            // all from this to the down
            for (int iRow = row + 1; iRow < GameBoard.Size; iRow++)
            {
                Square square = board.GetSquare(new Coord(iRow, col));
                if (square.isEmpty())
                {
                    possibleMoves.Add(square);
                }
                else if (square.piece.color != base.color)
                {
                    possibleMoves.Add(square);
                    break;
                }
                else break;
            }

            // all from this to the right
            for (int iCol = col + 1; iCol < GameBoard.Size; iCol++)
            {
                Square square = board.GetSquare(new Coord(row, iCol));
                if (square.isEmpty())
                {
                    possibleMoves.Add(square);
                }
                else if (square.piece.color != base.color)
                {
                    possibleMoves.Add(square);
                    break;
                }
                else break;
            }

            // all from this to the left
            for (int iCol = col - 1; iCol > -1; iCol--)
            {
                Square square = board.GetSquare(new Coord(row, iCol));
                if (square.isEmpty())
                {
                    possibleMoves.Add(square);
                }
                else if (square.piece.color != base.color)
                {
                    possibleMoves.Add(square);
                    break;
                }
                else break;
            }
        }
    }
}