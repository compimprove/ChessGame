using System.Linq;
using System;
using System.Collections.Generic;

namespace ChessGame.Models.Chess
{
    public class GameBoard
    {
        public RowSquare[] squares { get; set; }
        public Direction direction { get; set; }

        public Board boardInfo { get; set; }
        // public List<Piece> WhitePieces { get; } = new List<Piece>();
        // public List<Piece> BlackPieces { get; } = new List<Piece>();

        public Stack<History> Histories { get; } = new Stack<History>();


        public int getTotalValue(Color color)
        {
            return squares.Sum(rowSquares => rowSquares.GetValue(color));
        }

        public const int Size = 8;


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

        public static GameBoard GetBoard(string[][] board, Direction direction)
        {
            // board must have size 8x8
            if (board.Length == 8)
            {
                GameBoard result = new GameBoard
                {
                    direction = direction
                };
                RowSquare[] squares = new RowSquare[8];
                for (int row = 0; row < 8; row++)
                {
                    squares[row] = new RowSquare();
                    if (board[row].Length == 8)
                    {
                        for (int col = 0; col < 8; col++)
                        {
                            // each element in board
                            var square = new Square(
                                board[row][col],
                                new Coord(row, col),
                                result
                            );
                            squares[row][col] = square;
                            // if (square.piece?.color == Color.Black) result.BlackPieces.Add(square.piece);
                            // else if (square.piece?.color == Color.White) result.WhitePieces.Add(square.piece);
                        }
                    }
                    else
                    {
                        return null;
                    }
                }

                result.squares = squares;
                return result;
            }
            else
            {
                return null;
            }
        }

        public List<Piece> getOpponentPieces(Color ofColor)
        {
            return getColorPieces((Color)(-(int)ofColor));
        }

        public List<Piece> getColorPieces(Color ofColor)
        {
            var pieces = new List<Piece>();
            foreach (var rowSquare in squares)
            {
                pieces = pieces.Concat(rowSquare.getColorPieces(ofColor)).ToList();
            }

            return pieces;
        }

        public override string ToString()
        {
            return string.Join("\n", squares.Select(squares => squares));
        }

        public void undo()
        {
            try
            {
                var lastMove = Histories.Pop();
                GetSquare(lastMove.CoordFrom).RemovePiece();
                GetSquare(lastMove.CoordTo).RemovePiece();
                GetSquare(lastMove.CoordFrom).SetPiece(lastMove.CoordFromPiece);
                GetSquare(lastMove.CoordTo).SetPiece(lastMove.CoordToPiece);
            }
            catch (InvalidOperationException e)
            {
                Console.WriteLine(e);
            }
        }

        public void MovePiece(Coord from, Coord to)
        {
            Histories.Push(new History
            {
                CoordFrom = from,
                CoordTo = to,
                CoordFromPiece = GetSquare(from).piece,
                CoordToPiece = GetSquare(to).piece
            });
            var chosenPiece = GetSquare(from)?.RemovePiece();
            if (chosenPiece == null)
            {
                throw new NullReferenceException("chosenPiece is null");
            }
            GetSquare(to).RemovePiece();
            GetSquare(from).RemovePiece();
            GetSquare(to).SetPiece(chosenPiece);
        }
    }
}