using System;

namespace ChessGame.Models.Chess
{
    public class Square
    {
        public Piece piece { get; private set; } = null;
        public Coord coord { get; }
        public GameBoard board { get; }
        
        public Square(){}

        public Square(string pieceCode, Coord coord, GameBoard board)
        {
            this.piece = Piece.GetPiece(pieceCode, this);
            this.coord = coord;
            this.board = board ?? throw new Exception("Square's Board is null");
            
        }

        public override string ToString()
        {
            return piece != null ? piece.ToString() : "--";
        }

        public bool isEmpty()
        {
            return (this.piece == null);
        }

        /// <summary>
        /// Remove the piece and return it
        /// </summary>
        public Piece RemovePiece()
        {
            Piece piece = this.piece;
            this.piece = null;
            if (piece != null) piece.square = null;
            return piece;
        }

        public void SetPiece(Piece piece)
        {
            this.piece = piece;
            if (this.piece != null)
            {
                this.piece.square = this;
            }
        }
    }
}