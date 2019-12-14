using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChessGame.Models.Chess
{
    public struct Coord
    {
        public int row { get; set; }
        public int col { get; set; }
        public Coord(int newRow, int newCol)
        {
            row = newRow;
            col = newCol;
        }

        public override string ToString()
        {
            return $"row: {row}, col: {col}";
        }
    }
    public enum Color
    {
        Black,
        White
    }
    public enum Direction
    {
        WhiteGoup,
        WhiteGodown
    }
}
