using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChessGame.Models.Chess.piece
{
    public class Knight : Piece
    {
        public Knight(Color color, Square square) : base(color, square)
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

            List<Coord> possibleCoordMoves = new List<Coord>{
                new Coord(row-2, col+1),
                new Coord(row-1, col+2),
                new Coord(row+1, col+2),
                new Coord(row+2, col+1),
                new Coord(row+2, col-1),
                new Coord(row+1, col-2),
                new Coord(row-1, col-2),
                new Coord(row-2, col-1),
            };

            // Square have nothing or square have piece with its color different
            // than King's color
            foreach (Coord c in possibleCoordMoves)
            {
                Square square = board.GetSquare(c);
                if (square == null)
                    continue;
                else
                {
                    if (square.isEmpty())
                        possibleMoves.Add(square);
                    else if (square.piece.color != base.color)
                        possibleMoves.Add(square);
                }
            }
        }
    }
}
