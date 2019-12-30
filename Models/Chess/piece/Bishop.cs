using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChessGame.Models.Chess.piece
{
    public class Bishop : Piece
    {
        public Bishop(Color color, Square square) : base(color, square)
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
                else if (square.piece.color != base.color)
                {
                    possibleMoves.Add(square);
                    break;
                }
                else break;
            }
            // all from this to right up
            for (int iRow = row - 1, iCol = col + 1;
                iRow > -1 && iCol < GameBoard.SIZE;
                iRow--, iCol++)
            {
                Square square = board.GetSquare(new Coord(iRow, iCol));
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
            // all from this to left down
            for (int iRow = row + 1, iCol = col - 1;
                iRow < GameBoard.SIZE && iCol > -1;
                iRow++, iCol--)
            {
                Square square = board.GetSquare(new Coord(iRow, iCol));
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
            // all from this to right down
            for (int iRow = row + 1, iCol = col + 1;
                iRow < GameBoard.SIZE && iCol < GameBoard.SIZE;
                iRow++, iCol++)
            {
                Square square = board.GetSquare(new Coord(iRow, iCol));
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
