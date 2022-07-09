using System.Collections.Generic;
using System.Linq;

namespace ChessGame.Models.Chess.piece
{
    public class Bishop : Piece
    {
        private const int BaseValue = 330;

        private static readonly int[][] BishopValue =
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

        private static readonly int[][] BishopValueReverse = Reverse(BishopValue);

        public Bishop(Color color, Square square) : base(color, square)
        {
        }

        public override string ToString()
        {
            return color.ToDescriptionString() + "B";
        }


        public override int GetValue(Color color)
        {
            return base.GetValue(color, BaseValue, BishopValue, BishopValueReverse);
        }

        public override List<Square> PossibleEatingMove()
        {
            GeneratePossibleMove();
            return possibleMoves;
        }

        public override void GeneratePossibleMove()
        {
            Square thisSquare = this.square;
            if (thisSquare == null) return;

            int row = thisSquare.coord.row;
            int col = thisSquare.coord.col;
            GameBoard board = square.board;
            possibleMoves.Clear();

            // all from this to left up
            for (int iRow = row - 1, iCol = col - 1;
                 iRow > -1 && iCol > -1;
                 iRow--, iCol--)
            {
                Square square = board.GetSquare(new Coord(iRow, iCol));
                if (square.isEmpty())
                {
                    possibleMoves.Add(square);
                }
                else if (square.piece.color != color)
                {
                    possibleMoves.Add(square);
                    break;
                }
                else break;
            }

            // all from this to right up
            for (int iRow = row - 1, iCol = col + 1;
                 iRow > -1 && iCol < GameBoard.Size;
                 iRow--, iCol++)
            {
                Square square = board.GetSquare(new Coord(iRow, iCol));
                if (square.isEmpty())
                {
                    possibleMoves.Add(square);
                }
                else if (square.piece.color != color)
                {
                    possibleMoves.Add(square);
                    break;
                }
                else break;
            }

            // all from this to left down
            for (int iRow = row + 1, iCol = col - 1;
                 iRow < GameBoard.Size && iCol > -1;
                 iRow++, iCol--)
            {
                Square square = board.GetSquare(new Coord(iRow, iCol));
                if (square.isEmpty())
                {
                    possibleMoves.Add(square);
                }
                else if (square.piece.color != color)
                {
                    possibleMoves.Add(square);
                    break;
                }
                else break;
            }

            // all from this to right down
            for (int iRow = row + 1, iCol = col + 1;
                 iRow < GameBoard.Size && iCol < GameBoard.Size;
                 iRow++, iCol++)
            {
                Square square = board.GetSquare(new Coord(iRow, iCol));
                if (square.isEmpty())
                {
                    possibleMoves.Add(square);
                }
                else if (square.piece.color != color)
                {
                    possibleMoves.Add(square);
                    break;
                }
                else break;
            }
        }
    }
}