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
        public static SqCoord GameSize;
        public static List<SqCoord> GameField;
        public static SqCoord PlayerPos;
        public static int Length = 4;
        public static int UpdateTime = 2000;
        public static Direction MoveDirection;

        public static void Start(GameStartMode mode)
        {
            switch(mode)
            {
                case GameStartMode.SinglePlayer:
                    break;
                default:
                    throw new ArgumentException();
            }
        }

        public static void Load(Size size)
        {
            //GameSize = size;
            GameSize = SquareCoord.PointToSqCoord(new Point(size));
            GameField = new List<SqCoord>(GameSize.X * GameSize.Y);
            PlayerPos = new SqCoord { X = GameSize.X / 2, Y = 1 };
            for (int i = 0; i < GameSize.X; i++)
            {
                for (int j = 0; j < GameSize.Y; j++)
                {
                    SqCoord coord = new SqCoord { X = i, Y = j, Tick = 0 };
                    if (i == 0 || j == 0 || i == GameSize.X - 1 || j == GameSize.Y - 1)
                        coord.Tick = -1;
                    else if (i == PlayerPos.X && j == PlayerPos.Y)
                        coord.Tick = 4;
                    GameField.Add(coord);
                }
            }
            //GameField.Single(entry => entry.X == PlayerPos.X && entry.Y == PlayerPos.Y).Tick;
            /*for (int i = 0; i < GameField.Count; i++)
            {
                if(GameField[i].X==PlayerPos.X && GameField[i].Y==PlayerPos.Y)
                {
                    GameField[i].Tick = Length;
                }
            }*/
            MoveDirection = Direction.Down;
            GameRenderer.Render();
        }

        public static void Refresh()
        {
            //Decrease any positive Ticks; if next player position is other than zero, game over
            //Otherwise set next player position and set Tick on player position to current Length
            //Console.WriteLine("Refreshing...");
            for (int i = 0; i < GameField.Count; i++)
            {
                if (GameField[i].Tick > 0)
                    GameField[i] = new SqCoord { X = GameField[i].X, Y = GameField[i].Y, Tick = GameField[i].Tick - 1 };
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
            if (Game.GetCoord(nextcoord).Tick != 0)
                Stop();
            for (int i = 0; i < GameField.Count; i++)
            {
                if (GameField[i].X == nextcoord.X && GameField[i].Y == nextcoord.Y)
                {
                    GameField[i] = new SqCoord { X = nextcoord.X, Y = nextcoord.Y, Tick = Length };
                    PlayerPos = GameField[i];
                }
            }
            GameRenderer.Render();
        }

        public static SqCoord GetCoord(SqCoord nextcoord)
        {
            return GameField.Single(entry => entry.X == nextcoord.X && entry.Y == nextcoord.Y);
        }

        public static void Stop()
        {
            Form1.Timer.Stop();
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
