using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChessGame.Models.Chess.piece
{
    public class Queen : Piece
    {
        private readonly int BaseValue = 900;

        public Queen(Color color, Square square) : base(color, square)
        {
        }

        public override string ToString()
        {
            return base.color.ToDescriptionString() + "Q";
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

            Rook rook = new Rook(base.color, base.square);
            Bishop bishop = new Bishop(base.color, base.square);
            rook.GeneratePossibleMove();
            bishop.GeneratePossibleMove();

            base.possibleMoves.AddRange(rook.possibleMoves);
            base.possibleMoves.AddRange(bishop.possibleMoves);
        }

        public override int getValue(Color color)
        {
            return color == this.color ? BaseValue : -BaseValue;
        }
    }
}