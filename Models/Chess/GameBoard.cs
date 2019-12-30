using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;
using System;
using System.Collections.Generic;

namespace ChessGame.Models.Chess
{
    public class GameBoard
    {
        public Square[][] squares { get; set; }
        public Direction direction { get; set; }
        public List<Piece> WhitePieces { get; } = new List<Piece>();
        public List<Piece> BlackPieces { get; } = new List<Piece>();
        public GameBoard(Direction direction)
        {
            this.direction = direction;
        }
        public const int SIZE = 8;
        /// <summary>
        /// Return null if coord has wrong value
        /// </summary>
        public Square GetSquare(Coord coord)
        {
            if (coord.row >= 0 && coord.row <= 7 &&
                coord.col >= 0 && coord.col <= 7)
            {
                return this.squares[coord.row][coord.col];
            }
            else
            {
                return null;
            }
        }

        /// <param name="board">the string[][] contains pieceCode; 
        /// Must have size 8x8</param>
        public static GameBoard GetBoard(string[][] board, Direction direction)
        {
            // board must have size 8x8
            if (board.Length == 8)
            {
                GameBoard result = new GameBoard(direction);
                Square[][] squares = new Square[8][];
                for (int row = 0; row < 8; row++)
                {
                    squares[row] = new Square[8];
                    if (board[row].Length == 8)
                    {
                        for (int col = 0; col < 8; col++)
                        {// each element in board
                            squares[row][col] = new Square(
                                board[row][col],
                                new Coord(row, col),
                                result
                                );
                        }
                    }
                    else
                    {
                        return null;
                    }
                }
                result.squares = squares;
                //Console.WriteLine("Fine");
                return result;
            }
            else
            {
                return null;
            }
        }
        public List<Piece> getOpponentPieces(Color ofColor)
        {
            if (ofColor == Color.Black) return WhitePieces;
            else if (ofColor == Color.White) return BlackPieces;
            else
            {
                throw new Exception("getOpponentPieces has wrong param");
            }
        }
    }
}
