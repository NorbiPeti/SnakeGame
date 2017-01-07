using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame
{
    public static class SquareCoord
    {
        private static int x;
        public static int X
        {
            get
            {
                return x;
            }
            set
            {
                x = value;
                Game.Refresh();
            }
        }
        private static int y;
        public static int Y
        {
            get
            {
                return y;
            }
            set
            {
                y = value;
                Game.Refresh();
            }
        }
        private const int res = 50;

        public static Point SqCoordToPoint(SqCoord coord)
        {
            return new Point(coord.X * res, coord.Y * res);
        }
        public static SqCoord PointToSqCoord(Point point)
        {
            return new SqCoord { X = point.X / res, Y = point.Y / res };
        }
        public static Rectangle SqCoordToRect(SqCoord coord)
        {
            return new Rectangle(coord.X * res, coord.Y * res, coord.X * res + res, coord.Y * res + res);
        }
    }
    public struct SqCoord
    {
        public int X { get; set; }
        public int Y { get; set; }
        /// <summary>
        /// Used to determine snake square "expiration". -1 means wall. Game over if other than zero in next player position.
        /// </summary>
        public int Tick { get; set; }
    }
}
