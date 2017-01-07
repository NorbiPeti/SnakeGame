using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
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
        public static Point GameSize;
        //public static List<SqCoord> GameField;
        public static SqCoord[,] GameField;
        public static Point PlayerPos;
        public static int Length = 4;
        public static int UpdateTime = 800;
        public static Direction MoveDirection;
        public static Label ScoreLabel;
        public static Label LivesLabel;
        public static Panel DialogPanel;
        public static string UserName;
        private static int score;
        public static int Score
        {
            get
            {
                return score;
            }
            set
            {
                score = value;
                ScoreLabel.Text = "Score: " + score;
            }
        }
        private static int lives;
        public static int Lives
        {
            get
            {
                return lives;
            }
            set
            {
                lives = value;
                LivesLabel.Text = "Lives: " + lives;
            }
        }
        /// <summary>
        /// Opposite of Form1.TimerEnabled
        /// </summary>
        public static bool Paused
        {
            get
            {
                return !Form1.TimerEnabled;
            }
            set
            {
                Form1.TimerEnabled = !value;
            }
        }

        public static void Start(GameStartMode mode)
        {
            switch(mode)
            {
                case GameStartMode.SinglePlayer:
                    MSGBox.ShowMSGBox("New singleplayer game", "Size:", MSGBoxType.SizeInput, new EventHandler<string>(delegate(object s, string inp)
                    {
                        //string[] strs=input.Split(' ');
                        //int x, y;
                        //if (strs.Length == 2 && int.TryParse(strs[0], out x) && int.TryParse(strs[1], out y))
                        int input = int.Parse(inp);
                        if (input > 5)
                        {
                            //GameSize = new Point(x, y);
                            GameRenderer.Panel.CreateGraphics().FillRectangle(new SolidBrush(Color.Black), new Rectangle(new Point(), GameRenderer.Panel.Size));
                            int xy = GameRenderer.Panel.Size.Width / input;
                            GameSize = new Point(xy, xy);
                            Game.Reset();
                            Form1.TimerEnabled = true;
                        }
                        else
                            Game.Paused = true;
                    }));
                    break;
                case GameStartMode.MultiPlayer:
                    if (Game.UserName == "Player")
                    {
                        MSGBox.ShowMSGBox("Please change your username from default.", "", MSGBoxType.Text);
                        break;
                    }
                    MSGBox.ShowMSGBox("New multiplayer game", "Name:\nMax. players:", MSGBoxType.MultipleInput, new EventHandler<string>(delegate(object s, string input)
                    {
                        string[] strs = input.Split('\n');
                        int num = 0;
                        if (int.TryParse(strs[1], out num))
                        {
                            var match = new NetMatch { Name = strs[0], MaxPlayers = num, OwnerIP = Network.GetIPs() };
                            Network.CreateGame(match);
                            Game.Paused = false;
                        }
                        else
                            Game.Paused = true;
                    }));
                    break;
                case GameStartMode.Connect:
                    if (Game.UserName == "Player")
                    {
                        MSGBox.ShowMSGBox("Please change your username from default.", "", MSGBoxType.Text);
                        break;
                    }
                    Network.DownloadGameList();
                    string matches = "";
                    for (int i = 0; i < Network.Matches.Count; i++)
                    {
                        matches += Network.Matches[i].Name + "    " + Network.Matches[i].Players.Count + "/" + Network.Matches[i].MaxPlayers + "    " + Network.Matches[i].OwnerName + "\n";
                    }
                    MSGBox.ShowMSGBox("Connect to game", matches, MSGBoxType.List, new EventHandler<string>(delegate(object s, string input)
                    {
                        //Network.Connect(Network.Matches[int.Parse(input)]);
                        int num = 0;
                        if (int.TryParse(input, out num) && num != -1)
                        {
                            Network.Connect(Network.Matches[num]);
                            Game.Paused = false;
                        }
                        else
                            Game.Paused = true;
                    }));
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
            GameSize = new Point { X = size.Width / 20, Y = size.Height / 20 };
            UserName = "Player";
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

        public static void Reset(bool fullreset)
        {
            Size size = GameRenderer.Panel.Size;
            //GameSize = new Point { X = size.Width / 20, Y = size.Height / 20 };
            GameField = new SqCoord[GameSize.X, GameSize.Y];
            PlayerPos = new Point { X = GameSize.X / 2, Y = 1 };
            if (fullreset)
            {
                Score = 0;
                Lives = 3;
                UpdateTime = 800;
                Length = 4;
            }
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
                    {
                        GameField[i, j].Type = SquareType.Wall;
                        GameField[i, j].Tick = -1;
                    }
                    else if (i == PlayerPos.X && j == PlayerPos.Y)
                    {
                        GameField[i, j].Type = SquareType.Player;
                        GameField[i, j].Tick = Length;
                    }
                }
            }
            Game.AddPoint();
            MoveDirection = Direction.Down;
        }

        public static void Reset()
        {
            Reset(true);
        }

        public static void Refresh()
        {
            //Decrease any positive Ticks; if next player position is other than zero, game over
            //Otherwise set next player position and set Tick on player position to current Length
            //Console.WriteLine("Refreshing...");
            //for (int i = 0; i < GameField.Count; i++)
            if (!Form1.TimerEnabled)
                return; //Not playing currently
            for (int i = 0; i < GameField.GetLength(0); i++)
            {
                for (int j = 0; j < GameField.GetLength(1); j++)
                {
                    /*if (GameField[i].Tick > 0)
                        GameField[i] = new SqCoord { X = GameField[i].X, Y = GameField[i].Y, Tick = GameField[i].Tick - 1 };*/
                    if (GameField[i, j].Tick > 0)
                        GameField[i, j].Tick--;
                }
            }
            Point nextcoord;
            switch (MoveDirection)
            {
                case Direction.Down:
                    nextcoord = new Point { X = PlayerPos.X, Y = PlayerPos.Y + 1 };
                    break;
                case Direction.Left:
                    nextcoord = new Point { X = PlayerPos.X - 1, Y = PlayerPos.Y };
                    break;
                case Direction.Right:
                    nextcoord = new Point { X = PlayerPos.X + 1, Y = PlayerPos.Y };
                    break;
                case Direction.Up:
                    nextcoord = new Point { X = PlayerPos.X, Y = PlayerPos.Y - 1 };
                    break;
                default:
                    nextcoord = PlayerPos;
                    break;
            }
            //if (Game.GetCoord(nextcoord).Tick != 0)
            if (Game.GameField[nextcoord.X, nextcoord.Y].Tick != 0 && Game.GameField[nextcoord.X, nextcoord.Y].Type != SquareType.Point)
            {
                Lives--;
                if (Lives <= 0)
                    Stop();
                else
                    Reset(false);
            }
            else
            {
                /*for (int i = 0; i < GameField.GetLength(0); i++)
                {
                    for (int j = 0; j < GameField.GetLength(1); j++)
                    {
                        if (i == nextcoord.X && j == nextcoord.Y)
                        {*/
                if (GameField[nextcoord.X, nextcoord.Y].Type == SquareType.Point)
                {
                    Score += 1000;
                    Game.AddPoint();
                }
                GameField[nextcoord.X, nextcoord.Y].Tick = Length;
                GameField[nextcoord.X, nextcoord.Y].Type = SquareType.Player;
                PlayerPos = new Point { X = nextcoord.X, Y = nextcoord.Y };
                /*if (Score > 0)
                    Score--;*/
                if (Score > 0)
                    Score -= new Random().Next(1, 20);
/*                        }
                    }
                }*/
                GameRenderer.Render();
            }
        }

        public static void AddPoint()
        {
            Random rand = new Random();
            int x, y;
            do
            {
                x = rand.Next(GameField.GetLength(0) - 1);
                y = rand.Next(GameField.GetLength(1) - 1);
            } while (GameField[x, y].Tick != 0 && GameField[x, y].Type == SquareType.Wall);
            GameField[x, y].Tick = -1;
            GameField[x, y].Type = SquareType.Point;
        }

        /*public static SqCoord GetCoord(SqCoord nextcoord)
        {
            return GameField.Single(entry => entry.X == nextcoord.X && entry.Y == nextcoord.Y);
        }*/

        public static void Stop()
        {
            //Form1.Timer.Stop();
            //Form1.TimerEnabled = false;
            //MessageBox.Show("Game over!");
            MSGBox.ShowMSGBox("Game over!", "", MSGBoxType.Text);
            Game.Paused = true;
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
