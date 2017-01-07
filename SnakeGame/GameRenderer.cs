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
            //foreach(var coord in Game.GameField)
            for (int i = 0; i < Game.GameField.GetLength(0); i++)
            {
                for (int j = 0; j < Game.GameField.GetLength(1); j++)
                {
                    //if (coord.Tick == -1)
                    if (Game.GameField[i, j] == -1)
                        RenderSquare(new SqCoord { X = i, Y = j }, Color.Red);
                    else if (Game.GameField[i, j] == 0)
                        RenderSquare(new SqCoord { X = i, Y = j }, Color.Black);
                    else
                        RenderSquare(new SqCoord { X = i, Y = j }, Color.Green);
                }
            }
        }
        private static void RenderSquare(SqCoord coord, Color color)
        {
            Graphics gr = Panel.CreateGraphics();
            gr.FillRectangle(new SolidBrush(color), SquareCoord.SqCoordToRect(coord, Panel.Size));
        }
    }
}
