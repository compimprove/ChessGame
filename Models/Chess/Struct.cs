using System.ComponentModel;

namespace ChessGame.Models.Chess
{
    public class Coord
    {
        public int row { get; set; }
        public int col { get; set; }

        public Coord()
        {
        }

        public Coord(int newRow, int newCol)
        {
            row = newRow;
            col = newCol;
        }

        public Coord getMindSymmetry()
        {
            return new Coord(7 - this.row, 7 - this.col);
        }

        public override string ToString()
        {
            return $"row: {row}, col: {col}";
        }
    }

    public enum Color
    {
        [Description("b")] Black = -1,
        [Description("w")] White = 1,
    }

    public enum Direction
    {
        WhiteGoup,
        WhiteGodown
    }

    public static class MyEnumExtensions
    {
        public static string ToDescriptionString(this Color val)
        {
            DescriptionAttribute[] attributes = (DescriptionAttribute[])val
                .GetType()
                .GetField(val.ToString())
                ?.GetCustomAttributes(typeof(DescriptionAttribute), false);
            return attributes != null && attributes.Length > 0 ? attributes[0].Description : string.Empty;
        }
    }
}