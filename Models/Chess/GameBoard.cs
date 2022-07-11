using System.Linq;
using System;
using System.Collections.Generic;
using ChessGame.Models.Chess.piece;

namespace ChessGame.Models.Chess
{
    public class GameBoard
    {
        private RowSquare[] boardSquares { get; set; }
        public King whiteKing { get; set; }
        public King blackKing { get; set; }

        public Direction direction { get; set; }

        public Board boardInfo { get; set; }

        public Stack<MoveHistory[]> MovingHistories { get; } = new Stack<MoveHistory[]>();

        public int GetTotalValue(Color color)
        {
            return boardSquares.Sum(rowSquares => rowSquares.GetValue(color));
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
                return this.boardSquares[coord.row][coord.col];
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
                            if (board[row][col] == "whiteK")
                            {
                                result.whiteKing = (King)square.piece;
                            }

                            if (board[row][col] == "blackK")
                            {
                                result.blackKing = (King)square.piece;
                            }

                            squares[row][col] = square;
                        }
                    }
                    else
                    {
                        return null;
                    }
                }

                result.boardSquares = squares;
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
            foreach (var rowSquare in boardSquares)
            {
                pieces = pieces.Concat(rowSquare.getColorPieces(ofColor)).ToList();
            }

            return pieces;
        }

        public bool IsKingLive(Color color)
        {
            if (color == Color.Black) return blackKing?.square != null;
            return whiteKing?.square != null;
        }

        public override string ToString()
        {
            return string.Join("\n", boardSquares.Select(squares => squares));
        }

        public void Undo()
        {
            try
            {
                var lastMoves = MovingHistories.Pop();
                foreach (var lastMove in lastMoves)
                {
                    GetSquare(lastMove.CoordFrom).RemovePiece();
                    GetSquare(lastMove.CoordTo).RemovePiece();
                    GetSquare(lastMove.CoordFrom).SetPiece(lastMove.CoordFromPiece);
                    GetSquare(lastMove.CoordTo).SetPiece(lastMove.CoordToPiece);
                }
            }
            catch (InvalidOperationException e)
            {
                Console.WriteLine(e);
            }
        }

        public void MovePiece(Coord from, Coord to)
        {
            MovePieces(new (Coord, Coord)[] { (from, to) });
        }

        public void MovePieces((Coord, Coord)[] moves)
        {
            MovingHistories.Push(moves.Select(move => new MoveHistory
            {
                CoordFrom = move.Item1,
                CoordTo = move.Item2,
                CoordFromPiece = GetSquare(move.Item1).piece,
                CoordToPiece = GetSquare(move.Item2).piece
            }).ToArray());
            foreach (var (from, to) in moves)
            {
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
}