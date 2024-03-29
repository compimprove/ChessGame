﻿using System.Collections.Generic;
using System.Linq;

namespace ChessGame.Models.Chess.piece
{
    public class King : Piece
    {
        private readonly int BaseValue = 2000;

        public King(Color color, Square square) : base(color, square)
        {
        }


        public override int GetValue(Color color)
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

            List<Coord> possibleCoordMoves = new List<Coord>
            {
                new Coord(row + 1, col),
                new Coord(row - 1, col),
                new Coord(row, col + 1),
                new Coord(row, col - 1),
                new Coord(row - 1, col + 1),
                new Coord(row - 1, col - 1),
                new Coord(row + 1, col + 1),
                new Coord(row + 1, col - 1),
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

        public bool canMove(Coord coord)
        {
            Square square = base.square.board.GetSquare(coord);
            List<Piece> opponentPieces = base.square.board.getOpponentPieces(base.color);
            if (square.isEmpty())
            {
                foreach (Piece p in opponentPieces)
                {
                    if (p.PossibleEatingMove().Contains(square)) return false;
                    else
                    {
                    }

                    ;
                }

                return true;
            }
            else // Contains opponent's
            {
                return true;
            }
        }

        public void FilterPossibleMoves()
        {
            // if (base.possibleMoves == null) return;
            // else
            // {
            //     List<Piece> opponentPieces = base.square.board.getOpponentPieces(base.color);
            //     foreach (Piece p in opponentPieces)
            //     {
            //         p.GeneratePossibleMove();
            //         foreach (Move move in base.possibleMoves)
            //         {
            //             if (square.isEmpty())
            //                 if (p.possibleMoves.Contains(square))
            //                     base.possibleMoves.Remove(square);
            //                 else { }
            //             else { }
            //         }
            //
            //     }
            //foreach (Square square in base.possibleMoves)
            //{
            //    if (!square.isEmpty())
            //    {
            //        Piece piece = square.removePiece();
            //        opponentPieces.Remove(piece);
            //        foreach (Piece p in opponentPieces)
            //        {
            //            p.GeneratePossibleMove();
            //            if (p.possibleMoves.Contains(square))
            //                base.possibleMoves.Remove(square);
            //        }
            //        opponentPieces.Add(piece);
            //        square.piece = piece;
            //    }
            //    else { }
            //}
            // }
        }

        public override string ToString()
        {
            return base.color.ToDescriptionString() + "K";
        }

        public bool InDanger()
        {
            var pieces = square?.board?.getOpponentPieces(color);
            if (pieces == null) return false;
            foreach (var piece in pieces)
            {
                if (piece.PossibleEatingMove().Contains(square))
                {
                    return true;
                }
            }

            return false;
        }

        public Coord[] GenerateDangerMove()
        {
            var dangerMove = new List<Square>();
            GeneratePossibleMove();
            var possibleMoves = this.possibleMoves.Concat(new[] { square }).ToList();
            var oldBoard = square.board;
            var pieces = oldBoard.getOpponentPieces(color);

            foreach (var square in possibleMoves)
            {
                if (square.piece != null)
                {
                    var piece = square.RemovePiece();
                    piece.square = square;
                }
            }

            foreach (var square in possibleMoves)
            {
                foreach (var piece in pieces)
                {
                    if (piece.PossibleEatingMove().Contains(square))
                    {
                        dangerMove.Add(square);
                    }
                }
            }

            return dangerMove.Select(square => square.coord).ToArray();
        }
    }
}