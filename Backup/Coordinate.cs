using System;
using System.Collections.Generic;
using System.Linq;

namespace _2048.Core
{
    public class Coordinate
    {
        public Coordinate()
        { }
        public Coordinate(int x, int y)
        {
            X = x;
            Y = y;
        }
        public int X;
        public int Y;

        public override int GetHashCode()
        {
            return X.GetHashCode() + Y.GetHashCode();
        }

        public override string ToString()
        {
            return string.Format("X={0}; Y={1}", X, Y);
        }
    }

    public class CoordinateEqualityComparer : IEqualityComparer<Coordinate>
    {
        public bool Equals(Coordinate x, Coordinate y)
        {
            return x.X == y.X && x.Y == y.Y;
        }

        public int GetHashCode(Coordinate obj)
        {
            return obj.GetHashCode();
        }
    }
}
