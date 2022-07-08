using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChessGame.Models.Chess.piece
{

    public class Pawn : Piece
    {
        private readonly int BaseValue = 100;

        public Pawn(Color color, Square square) : base(color, square)
        {
        }
        public override List<Square> PossibleEatingMove()
        {
            List<Square> possibleMoves = new List<Square>();
            Square thisSquare = base.square;
            if (thisSquare == null) return null;

            int row = thisSquare.coord.row;
            int col = thisSquare.coord.col;
            GameBoard board = square.board;

            // Square have nothing or square have piece with its color different
            // than Pawn's color

            // This color is going up
            if ((board.direction == Direction.WhiteGoup && base.color == Color.White) || (board.direction == Direction.WhiteGodown && base.color == Color.Black))
            {
                Coord c1 = new Coord(row - 1, col - 1);
                Coord c2 = new Coord(row - 1, col + 1);
                possibleMoves.Add(board.GetSquare(c1));
                possibleMoves.Add(board.GetSquare(c2));
            }
            else   // This color is going down
            {
                Coord c1 = new Coord(row + 1, col - 1);
                Coord c2 = new Coord(row + 1, col + 1);
                possibleMoves.Add(board.GetSquare(c1));
                possibleMoves.Add(board.GetSquare(c2));
            }
            return possibleMoves;
        }
        public override int getValue(Color color)
        {
            return color == this.color ? BaseValue : -BaseValue;
        }
        public override void GeneratePossibleMove()
        {
            Square thisSquare = base.square;
            if (thisSquare == null) return;

            int row = thisSquare.coord.row;
            int col = thisSquare.coord.col;
            GameBoard board = square.board;
            possibleMoves.Clear();

            // Square have nothing or square have piece with its color different
            // than Pawn's color

            // This color is going up
            if ((board.direction == Direction.WhiteGoup && base.color == Color.White) || (board.direction == Direction.WhiteGodown && base.color == Color.Black))
            {
                Square ahead = board.GetSquare(new Coord(row - 1, col));

                if (ahead != null)
                {
                    if (ahead.isEmpty())
                    {
                        possibleMoves.Add(ahead);
                        if (thisSquare.coord.row == 6 && board.GetSquare(new Coord(4, col)).isEmpty())
                        {
                            possibleMoves.Add(board.GetSquare(new Coord(4, col)));
                        }
                    }
                    // left or right ahead
                    Coord c1 = new Coord(row - 1, col - 1);
                    Coord c2 = new Coord(row - 1, col + 1);
                    if (board.GetSquare(c1)?.piece?.opponentColor() == base.color)
                    {
                        possibleMoves.Add(board.GetSquare(c1));
                    }
                    if (board.GetSquare(c2)?.piece?.opponentColor() == base.color)
                    {
                        possibleMoves.Add(board.GetSquare(c2));
                    }
                }
            }
            else   // This color is going down
            {
                Square ahead = board.GetSquare(new Coord(row + 1, col));
                if (ahead != null)
                {
                    if (ahead.isEmpty())
                    {
                        possibleMoves.Add(ahead);
                        if (thisSquare.coord.row == 1 && board.GetSquare(new Coord(3, col)).isEmpty())
                        {
                            possibleMoves.Add(board.GetSquare(new Coord(3, col)));
                        }
                    }
                    // left or right ahead
                    Coord c1 = new Coord(row + 1, col - 1);
                    Coord c2 = new Coord(row + 1, col + 1);
                    if (board.GetSquare(c1)?.piece?.opponentColor() == base.color)
                    {
                        possibleMoves.Add(board.GetSquare(c1));
                    }
                    if (board.GetSquare(c2)?.piece?.opponentColor() == base.color)
                    {
                        possibleMoves.Add(board.GetSquare(c2));
                    }
                }
            }
        }
        
        public override string ToString()
        {
            return base.color.ToDescriptionString() + "P";
        }
    }
}
