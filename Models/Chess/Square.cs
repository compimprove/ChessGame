using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChessGame.Models.Chess
{
    public class Square
    {
        public Piece piece { get; set; } = null;
        public Coord coord { get; }
        public GameBoard board { get; }
        public Square(string pieceCode, Coord coord, GameBoard board)
        {
            this.piece = Piece.GetPiece(pieceCode, this);
            this.coord = coord;
            if (board == null)
            {
                throw new Exception("Square's Board is null");
            }
            this.board = board;
            if (piece?.color == Color.Black) board.BlackPieces.Add(piece);
            else if (piece?.color == Color.White) board.WhitePieces.Add(piece);
        }

        public bool isEmpty()
        {

            return (this.piece == null);
        }
        /// <summary>
        /// Remove the piece and return it
        /// </summary>
        public Piece removePiece()
        {
            Piece piece = this.piece;
            this.piece = null;
            return piece;
        }
    }
}
