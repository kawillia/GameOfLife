using System;

namespace GameOfLife.Core
{
    public class Coordinate
    {
        public Int32 X { get; set; }
        public Int32 Y { get; set; }

        public Coordinate(Int32 x, Int32 y)
        {
            X = x;
            Y = y;
        }

        public override Boolean Equals(object obj)
        {
            var coordinate = obj as Coordinate;
            return coordinate.X == X && coordinate.Y == Y;
        }

        public override Int32 GetHashCode()
        {
            return X ^ Y;
        }
    }
}
