using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace UXI.SystemApi
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Point
    {
        /// <summary>
        /// Specifies the X-coordinate of the point. 
        /// </summary>
        public int X;
        /// <summary>
        /// Specifies the Y-coordinate of the point. 
        /// </summary>
        public int Y;

        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }

        public static bool operator ==(Point a, Point b)
        {
            // If both are null, or both are same instance, return true.
            if (Object.ReferenceEquals(a, b))
            {
                return true;
            }

            // If one is null, but not both, return false.
            if (((object)a == null) || ((object)b == null))
            {
                return false;
            }

            // Return true if the fields match:
            return a.X == b.X
                && a.Y == b.Y;
        }

        public static bool operator !=(Point a, Point b)
        {
            return !(a == b);
        }

        public override bool Equals(object obj)
        {
            // If parameter is null return false.
            if (obj == null)
            {
                return false;
            }

            // If parameter cannot be cast to Point return false.
            if ((obj is Point) == false)
            {
                return false;
            }

            var point = (Point)obj;

            // Return true if the fields match:
            return X == point.X
                && Y == point.Y;
        }

        public bool Equals(Point point)
        {
            // If parameter is null return false:
            if ((object)point == null)
            {
                return false;
            }

            // Return true if the fields match:
            return X == point.X
                && Y == point.Y;
        }

        public override int GetHashCode()
        {
            return X.GetHashCode() ^ Y.GetHashCode();
        }
    }
}
