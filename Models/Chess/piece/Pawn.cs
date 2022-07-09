using System.Collections.Generic;

namespace ChessGame.Models.Chess.piece
{
    public class Pawn : Piece
    {
        private readonly int BaseValue = 100;

        private static readonly int[][] PawnValue =
        {
            new[] { 0, 0, 0, 0, 0, 0, 0, 0 },
            new[] { 50, 50, 50, 50, 50, 50, 50, 50 },
            new[] { 10, 10, 20, 30, 30, 20, 10, 10 },
            new[] { 5, 5, 10, 25, 25, 10, 5, 5 },
            new[] { 0, 0, 0, 20, 20, 0, 0, 0 },
            new[] { 5, -5, -10, 0, 0, -10, -5, 5 },
            new[] { 5, 10, 10, -20, -20, 10, 10, 5 },
            new[] { 0, 0, 0, 0, 0, 0, 0, 0 }
        };

        private static readonly int[][] PawnValueReverse = Reverse(PawnValue);

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
            if ((board.direction == Direction.WhiteGoUp && base.color == Color.White) ||
                (board.direction == Direction.WhiteGoDown && base.color == Color.Black))
            {
                Coord c1 = new Coord(row - 1, col - 1);
                Coord c2 = new Coord(row - 1, col + 1);
                possibleMoves.Add(board.GetSquare(c1));
                possibleMoves.Add(board.GetSquare(c2));
            }
            else // This color is going down
            {
                Coord c1 = new Coord(row + 1, col - 1);
                Coord c2 = new Coord(row + 1, col + 1);
                possibleMoves.Add(board.GetSquare(c1));
                possibleMoves.Add(board.GetSquare(c2));
            }

            return possibleMoves;
        }

        public override int GetValue(Color color)
        {
            return base.GetValue(color, BaseValue, PawnValue, PawnValueReverse);
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
            if ((board.direction == Direction.WhiteGoUp && base.color == Color.White) ||
                (board.direction == Direction.WhiteGoDown && base.color == Color.Black))
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
            else // This color is going down
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