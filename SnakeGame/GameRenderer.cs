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
                    //if (Game.GameField[i, j].Tick == -1)
                    if (Game.GameField[i, j].Tick == 0)
                        RenderSquare(new Point { X = i, Y = j }, Color.Black);
                    else if (Game.GameField[i, j].Type == SquareType.Wall)
                        RenderSquare(new Point { X = i, Y = j }, Color.Red);
                    else if (Game.GameField[i, j].Type == SquareType.Player)
                        RenderSquare(new Point { X = i, Y = j }, Color.Green);
                    else if (Game.GameField[i, j].Type == SquareType.Point)
                        RenderSquare(new Point { X = i, Y = j }, Color.Blue);
                    else
                        RenderSquare(new Point { X = i, Y = j }, Color.Gray);
                }
            }
        }
        private static void RenderSquare(Point coord, Color color)
        {
            Graphics gr = Panel.CreateGraphics();
            gr.FillRectangle(new SolidBrush(color), SquareCoord.SqCoordToRect(coord, Panel.Size));
        }
    }
}
