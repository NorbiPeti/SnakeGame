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
        //private static Timer timer = new Timer();
        public static void Render()
        {
            //foreach(var coord in Game.GameField)
            /*if (timer.Enabled)
                return;
            else
            {
                timer.Interval = 1;
                timer.ClearEventInvocations("Tick");
                timer.Tick += delegate
                {
                    timer.Stop();*/
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
                        if (Network.ConnectedMatch == null)
                            RenderSquare(new Point { X = i, Y = j }, Color.LimeGreen);
                        else
                        {
                            if (Game.GameField[i, j].PlayerName == Game.Player.Name)
                                RenderSquare(new Point { X = i, Y = j }, Game.Player.Color);
                            else
                            {
                                Player player = Network.ConnectedMatch.GetPlayerByName(Game.GameField[i, j].PlayerName);
                                if (player != null)
                                    RenderSquare(new Point { X = i, Y = j }, player.Color);
                                else
                                    RenderSquare(new Point { X = i, Y = j }, Color.DarkGray);
                            }
                        }
                    else if (Game.GameField[i, j].Type == SquareType.Point)
                        RenderSquare(new Point { X = i, Y = j }, Color.Blue);
                    else
                        RenderSquare(new Point { X = i, Y = j }, Color.Gray);
                }
            }
            /*    };
                timer.Start();
            }*/
        }
        private static void RenderSquare(Point coord, Color color)
        {
            Graphics gr = Panel.CreateGraphics();
            gr.FillRectangle(new SolidBrush(color), SquareCoord.SqCoordToRect(coord, Panel.Size));
        }
    }
}
