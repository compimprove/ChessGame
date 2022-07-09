using System;
using System.Collections.Generic;
using System.Diagnostics;
using ChessGame.Models.Chess.piece;

namespace ChessGame.Models.Chess
{
    public class Piece
    {
        public List<Square> possibleMoves { get; } = new List<Square>();

        public Coord Coord => square?.coord;

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
            GeneratePossibleMove();
            return possibleMoves;
        }

        public virtual int GetValue(Color color)
        {
            return 0;
        }

        protected bool IsGoingUp()
        {
            GameBoard board = square.board;
            return (board.direction == Direction.WhiteGoUp && color == Color.White) ||
                   (board.direction == Direction.WhiteGoDown && color == Color.Black);
        }

        public virtual int GetValue(Color color, int baseValue, int[][] boardValue, int[][] boardValueReverse)
        {
            var direction = square.board.direction;
            var extraValue = 0;
            if (direction == Direction.WhiteGoUp && color == Color.White ||
                direction == Direction.WhiteGoDown && color == Color.Black)
            {
                extraValue = boardValue[square.coord.row][square.coord.col];
            }
            else
            {
                extraValue = boardValueReverse[square.coord.row][square.coord.col];
            }

            var totalValue = baseValue + extraValue;

            return color == this.color ? totalValue : -totalValue;
        }

        public Color opponentColor()
        {
            if (this.color == Color.White) return Color.Black;
            else if (this.color == Color.Black) return Color.White;
            else throw new Exception("Color has other value ?");
        }

        public static int[][] Reverse(int[][] A)
        {
            int[][] B = new int[8][];
            for (int i = 0; i < 8; i++)
            {
                B[i] = new int[8];
                for (int j = 0; j < 8; j++)
                {
                    B[i][j] = A[7 - i][7 - j];
                }
            }

            return B;
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