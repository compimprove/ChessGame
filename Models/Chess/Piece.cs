using System;
using System.Collections.Generic;
using ChessGame.Models.Chess.piece;

namespace ChessGame.Models.Chess
{

    public class Piece
    {
        public List<Square> possibleMoves { get; } = new List<Square>();
        public Color color { get; set; }
        public Square square { get; set; }

        public Piece(Color color, Square square)
        {
            this.color = color;
            //if (square == null)
            //{
            //    throw new Exception("Piece's Square is null ?");
            //}
            //else
            //{
            this.square = square;
            //    if (color == Color.Black)
            //        square.board.BlackPieces.Add(this);
            //    else if (color == Color.White)
            //        square.board.WhitePieces.Add(this);
            //    else throw new Exception("Piece color is wrong");
            //}

        }

        public virtual void GeneratePossibleMove()
        {
        }

        public virtual List<Square> PossibleEatingMove()
        {
            return null;
        }

        public virtual int getValue(Color color)
        {
            return 0;
        }

        public virtual bool isKing()
        {
            return false;
        }
        public Color opponentColor()
        {
            if (this.color == Color.White) return Color.Black;
            else if (this.color == Color.Black) return Color.White;
            else throw new Exception("Color has other value ?");
        }
        public static Piece GetPiece(string code, Square square)
        {
            switch (code)
            {
                case "blackK":
                    return new King(Color.Black, square);
                case "whiteK":
                    return new King(Color.White, square);
                case "blackN":
                    return new Knight(Color.Black, square);
                case "whiteN":
                    return new Knight(Color.White, square);
                case "blackQ":
                    return new Queen(Color.Black, square);
                case "whiteQ":
                    return new Queen(Color.White, square);
                case "blackR":
                    return new Rook(Color.Black, square);
                case "whiteR":
                    return new Rook(Color.White, square);
                case "blackB":
                    return new Bishop(Color.Black, square);
                case "whiteB":
                    return new Bishop(Color.White, square);
                case "blackP":
                    return new Pawn(Color.Black, square);
                case "whiteP":
                    return new Pawn(Color.White, square);
            }
            return null;
        }
    }
}
