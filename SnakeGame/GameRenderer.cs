using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SnakeGame
{
    public static class GameRenderer
    {
        public static Panel Panel;
        public static void Render()
        {
            foreach(var coord in Game.GameField)
            {
                if (coord.Tick == -1)
                    RenderSquare(coord, Color.Red);
                else if (coord.Tick == 0)
                    RenderSquare(coord, Color.Black);
                else
                    RenderSquare(coord, Color.Green);
            }
        }
        private static void RenderSquare(SqCoord coord, Color color)
        {
            Graphics gr = Panel.CreateGraphics();
            gr.FillRectangle(new SolidBrush(color), SquareCoord.SqCoordToRect(coord));
        }
    }
}
