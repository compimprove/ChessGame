using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChessGame.Models.Chess.piece
{
    public class Rook : Piece
    {
        public Rook(Color color, Square square) : base(color, square)
        {
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
            Board board = square.board;
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
            for (int iRow = row + 1; iRow < Board.SIZE; iRow++)
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
            for (int iCol = col + 1; iCol < Board.SIZE; iCol++)
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
