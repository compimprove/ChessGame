namespace ChessGame.Models.Chess
{
    public struct MoveHistory
    {
        public Coord CoordFrom { get; set; }
        public Coord CoordTo { get; set; }
        public Piece CoordFromPiece { get; set; }
        public Piece CoordToPiece { get; set; }
    }
}