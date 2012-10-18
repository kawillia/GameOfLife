using System;

namespace GameOfLife.SharpGlWpfApplication
{
    public class Vector3
    {
        public Double X { get; set; }
        public Double Y { get; set; }
        public Double Z { get; set; }

        public Vector3(Double x, Double y, Double z)
        {
            X = x;
            Y = y;
            Z = z;
        }
    }
}
