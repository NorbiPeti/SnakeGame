using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SnakeGame
{
    public static class Game
    {
        /// <summary>
        /// Plans:
        /// Replace MessageBox with own GUI; Add GUI to get GameSize...
        /// Maybe select region to set one square's size and/or visualize changes when user stops interaction
        /// </summary>
        public static SqCoord GameSize;
        //public static List<SqCoord> GameField;
        public static int[,] GameField;
        public static SqCoord PlayerPos;
        public static int Length = 4;
        public static int UpdateTime = 1000;
        public static Direction MoveDirection;

        public static void Start(GameStartMode mode)
        {
            switch(mode)
            {
                case GameStartMode.SinglePlayer:
                    Game.Reset();
                    Form1.TimerEnabled = true;
                    break;
                default:
                    throw new ArgumentException();
            }
        }

        public static void Load(Size size)
        {
            //GameSize = size;
            //GameSize = SquareCoord.PointToSqCoord(new Point(size));
            //GameSize = new SqCoord { X = size.Width / 20, Y = size.Height / 20 };
            //GameField = new List<SqCoord>(GameSize.X * GameSize.Y);
            //GameField = new int[GameSize.X, GameSize.Y];
            Game.Reset();
            //GameField.Single(entry => entry.X == PlayerPos.X && entry.Y == PlayerPos.Y).Tick;
            /*for (int i = 0; i < GameField.Count; i++)
            {
                if(GameField[i].X==PlayerPos.X && GameField[i].Y==PlayerPos.Y)
                {
                    GameField[i].Tick = Length;
                }
            }*/
            //GameRenderer.Render(); - It has no effect in loading part
        }

        public static void Reset()
        {
            Size size = GameRenderer.Panel.Size;
            GameSize = new SqCoord { X = size.Width / 20, Y = size.Height / 20 };
            GameField = new int[GameSize.X, GameSize.Y];
            PlayerPos = new SqCoord { X = GameSize.X / 2, Y = 1 };
            for (int i = 0; i < GameSize.X; i++)
            {
                for (int j = 0; j < GameSize.Y; j++)
                {
                    /*SqCoord coord = new SqCoord { X = i, Y = j, Tick = 0 };
                    if (i == 0 || j == 0 || i == GameSize.X - 1 || j == GameSize.Y - 1)
                        coord.Tick = -1;
                    else if (i == PlayerPos.X && j == PlayerPos.Y)
                        coord.Tick = 4;
                    GameField.Add(coord);*/
                    if (i == 0 || j == 0 || i == GameSize.X - 1 || j == GameSize.Y - 1)
                        GameField[i, j] = -1;
                    else if (i == PlayerPos.X && j == PlayerPos.Y)
                        GameField[i, j] = 4;
                    else
                        GameField[i, j] = 0;
                }
            }
            MoveDirection = Direction.Down;
        }

        public static void Refresh()
        {
            //Decrease any positive Ticks; if next player position is other than zero, game over
            //Otherwise set next player position and set Tick on player position to current Length
            //Console.WriteLine("Refreshing...");
            //for (int i = 0; i < GameField.Count; i++)
            for (int i = 0; i < GameField.GetLength(0); i++)
            {
                for (int j = 0; j < GameField.GetLength(1); j++)
                {
                    /*if (GameField[i].Tick > 0)
                        GameField[i] = new SqCoord { X = GameField[i].X, Y = GameField[i].Y, Tick = GameField[i].Tick - 1 };*/
                    if (GameField[i, j] > 0)
                        GameField[i, j]--;
                }
            }
            SqCoord nextcoord;
            switch (MoveDirection)
            {
                case Direction.Down:
                    nextcoord = new SqCoord { X = PlayerPos.X, Y = PlayerPos.Y + 1 };
                    break;
                case Direction.Left:
                    nextcoord = new SqCoord { X = PlayerPos.X - 1, Y = PlayerPos.Y };
                    break;
                case Direction.Right:
                    nextcoord = new SqCoord { X = PlayerPos.X + 1, Y = PlayerPos.Y };
                    break;
                case Direction.Up:
                    nextcoord = new SqCoord { X = PlayerPos.X, Y = PlayerPos.Y - 1 };
                    break;
                default:
                    nextcoord = PlayerPos;
                    break;
            }
            //if (Game.GetCoord(nextcoord).Tick != 0)
            if (Game.GameField[nextcoord.X, nextcoord.Y] != 0)
                Stop();
            else
            {
                for (int i = 0; i < GameField.GetLength(0); i++)
                {
                    for (int j = 0; j < GameField.GetLength(1); j++)
                    {
                        if (i == nextcoord.X && j == nextcoord.Y)
                        {
                            GameField[nextcoord.X, nextcoord.Y] = Length;
                            PlayerPos = new SqCoord { X = i, Y = j };
                        }
                    }
                }
                GameRenderer.Render();
            }
        }

        /*public static SqCoord GetCoord(SqCoord nextcoord)
        {
            return GameField.Single(entry => entry.X == nextcoord.X && entry.Y == nextcoord.Y);
        }*/

        public static void Stop()
        {
            //Form1.Timer.Stop();
            Form1.TimerEnabled = false;
            MessageBox.Show("Game over!");
        }
    }
    public enum GameStartMode
    {
        SinglePlayer,
        MultiPlayer,
        Connect
    }
    public enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }
}
