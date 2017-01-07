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
        /*private static int x;
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
        }*/
        //private const int res = 20;

        /*public static Point SqCoordToPoint(SqCoord coord)
        {
            return new Point(coord.X * res, coord.Y * res);
        }*/
        /*public static SqCoord PointToSqCoord(Point point)
        {
            return new SqCoord { X = point.X / res, Y = point.Y / res };
        }*/
        public static Rectangle SqCoordToRect(Point coord, Size size)
        {
            int resx = size.Width / Game.GameSize.X;
            int resy = size.Height / Game.GameSize.Y;
            return new Rectangle(coord.X * resx, coord.Y * resy, resx, resy);
        }
    }
    public struct SqCoord
    {
        //public int X { get; set; } - using it in two-dimensional array; replaced with Point where needed
        //public int Y { get; set; }
        /// <summary>
        /// Used to determine snake square "expiration". Game over if other than zero or there is a wall.
        /// </summary>
        public int Tick { get; set; }
        public SquareType Type { get; set; }
        public int PlayerID { get; set; }
    }
    /// <summary>
    /// Only consider if Tick>0
    /// </summary>
    public enum SquareType
    {
        Wall,
        Point,
        Player
    }
}
