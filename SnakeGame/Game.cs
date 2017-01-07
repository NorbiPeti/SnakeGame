using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SnakeGame
{
    public static class Game
    {
        public static Point GameSize;
        public static SqCoord[,] GameField;
        public static int Length;
        public static int UpdateTime = 800;
        public static Direction MoveDirection;
        public static Label ScoreLabel;
        public static Label LivesLabel;
        public static Panel DialogPanel;
        public static Player Player = new Player("Player", true);
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
                        int input = int.Parse(inp);
                        if (input > 5)
                        {
                            GameRenderer.Panel.CreateGraphics().FillRectangle(new SolidBrush(Color.Black), new Rectangle(new Point(), GameRenderer.Panel.Size));
                            //int xy = GameRenderer.Panel.Size.Width / input;
                            //GameSize = new Point(xy, xy);
                            if (GameRenderer.Panel.Size.Width / input < 3 || GameRenderer.Panel.Size.Height / input < 3)
                            {
                                Game.Paused = true;
                                return;
                            }
                            GameSize = new Point(GameRenderer.Panel.Size.Width / input, GameRenderer.Panel.Size.Height / input);
                            Game.Reset();
                            Form1.TimerEnabled = true;
                        }
                        else
                            Game.Paused = true;
                    }));
                    break;
                case GameStartMode.MultiPlayer:
                    if (Game.Player.Name == "Player")
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
                            var match = new NetMatch { Name = strs[0], MaxPlayers = num, OwnerIP = Network.GetIPs(), OwnerName = Player.Name };
                            match.Players.Add(Game.Player);
                            Game.Reset();
                            Network.CreateGame(match);
                        }
                        else
                            Game.Paused = true;
                    }));
                    break;
                case GameStartMode.Connect:
                    if (Game.Player.Name == "Player")
                    {
                        MSGBox.ShowMSGBox("Please change your username from default.", "", MSGBoxType.Text);
                        break;
                    }

                    //string matches = Network.Matches.Combine<NetMatch, string>((match, combiner) => combiner += match.Name + "    " + match.Players.Count + "/" + match.MaxPlayers + "    " + match.OwnerName + "\n");
                    string inputs = "IP:\n";
                    MSGBox.ShowMSGBox("Connect to game", inputs, MSGBoxType.MultipleInput, new EventHandler<string>(delegate(object s, string input)
                    {
                        IPAddress address;
                        if (IPAddress.TryParse(input.Replace("\n", ""), out address))
                        {
                            Game.Reset();
                            Network.Connect(new NetMatch { OwnerIP = new IPAddress[] { address } });
                            //Game.Reset(false); - On successfull connect event
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
            GameSize = new Point { X = size.Width / 20, Y = size.Height / 20 };
            Game.Reset();
        }

        public static void Reset(bool fullreset)
        {
            if (fullreset)
                Network.Leave();
            Size size = GameRenderer.Panel.Size;
            GameField = new SqCoord[GameSize.X, GameSize.Y];
            //Player.Position = new Point { X = GameSize.X / 2, Y = 1 };
            var rand=new Random();
            Direction dir = (Direction)rand.Next(4);
            do
            {
                switch (dir)
                {
                    case Direction.Up:
                        Player.Position = new Point(rand.Next(1, GameSize.X - 2), 1);
                        MoveDirection = Direction.Down;
                        break;
                    case Direction.Down:
                        Player.Position = new Point(rand.Next(1, GameSize.X - 2), GameSize.Y - 2);
                        MoveDirection = Direction.Up;
                        break;
                    case Direction.Left:
                        Player.Position = new Point(1, rand.Next(1, GameSize.Y - 2));
                        MoveDirection = Direction.Right;
                        break;
                    case Direction.Right:
                        Player.Position = new Point(GameSize.X - 2, rand.Next(1, GameSize.Y - 2));
                        MoveDirection = Direction.Left;
                        break;
                }
            } while (GameField[Player.Position.X, Player.Position.Y].Tick > 0);
            if (fullreset)
            {
                Player.Score = 0;
                Player.Lives = 3;
                UpdateTime = 600;
                Length = 4;
            }
            Network.SyncUpdate(NetUpdateType.Teleport, Player.Position);
            for (int i = 0; i < GameSize.X; i++)
            {
                for (int j = 0; j < GameSize.Y; j++)
                {
                    if (i == 0 || j == 0 || i == GameSize.X - 1 || j == GameSize.Y - 1)
                    {
                        GameField[i, j].Type = SquareType.Wall;
                        GameField[i, j].Tick = -1;
                    }
                    else if (i == Player.Position.X && j == Player.Position.Y)
                    {
                        GameField[i, j].Type = SquareType.Player;
                        GameField[i, j].Tick = Length;
                        GameField[i, j].PlayerName = Player.Name;
                    }
                }
            }
            Game.AddPoint();
            //MoveDirection = Direction.Down;
        }

        public static void Reset()
        {
            Reset(true);
        }

        public static void Refresh()
        {
            //Decrease any positive Ticks; if next player position is other than zero, game over
            //Otherwise set next player position and set Tick on player position to current Length
            if (!Form1.TimerEnabled)
                return; //Not playing currently
            for (int i = 0; i < GameField.GetLength(0); i++)
            {
                for (int j = 0; j < GameField.GetLength(1); j++)
                {
                    if (GameField[i, j].Tick > 0)
                        GameField[i, j].Tick--;
                }
            }
            Point nextcoord = MovePlayerPre(Player, MoveDirection);
            Network.SyncUpdate(NetUpdateType.Move, MoveDirection);
            if (nextcoord.X >= Game.GameSize.X || nextcoord.Y >= Game.GameSize.Y
            || (Game.GameField[nextcoord.X, nextcoord.Y].Tick != 0 && Game.GameField[nextcoord.X, nextcoord.Y].Type != SquareType.Point))
            {
                Player.Lives--;
                LivesLabel.ForeColor = Color.Red;
                if (Player.Lives <= 0)
                    Stop();
                else
                    Reset(false);
            }
            else
            {
                //LivesLabel.ForeColor = Color.White;
                LivesLabel.ForeColor = Game.Player.Color;
                if (GameField[nextcoord.X, nextcoord.Y].Type == SquareType.Point)
                {
                    Player.Score += 1000;
                    ScoreLabel.ForeColor = Color.Blue;
                    Game.AddPoint();
                }
                else
                    //ScoreLabel.ForeColor = Color.White;
                    ScoreLabel.ForeColor = Game.Player.Color;
                if (Player.Score > 0)
                    Player.Score -= new Random().Next(1, 20);
                MovePlayerPost(Player, nextcoord);
            }
            GameRenderer.Render();
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

        public static void Stop()
        {
            MSGBox.ShowMSGBox("Game over!", "", MSGBoxType.Text);
            Game.Paused = true;
            Network.Leave(); //2015.08.29.
        }

        public static Point MovePlayerPre(Player player, Direction direction)
        {
            Point nextcoord;
            switch (direction)
            {
                case Direction.Down:
                    nextcoord = new Point { X = player.Position.X, Y = player.Position.Y + 1 };
                    break;
                case Direction.Left:
                    nextcoord = new Point { X = player.Position.X - 1, Y = player.Position.Y };
                    break;
                case Direction.Right:
                    nextcoord = new Point { X = player.Position.X + 1, Y = player.Position.Y };
                    break;
                case Direction.Up:
                    nextcoord = new Point { X = player.Position.X, Y = player.Position.Y - 1 };
                    break;
                default:
                    nextcoord = player.Position;
                    break;
            }
            return nextcoord;
        }
        public static void MovePlayerPost(Player player, Point point)
        {
            if (point.X >= GameSize.X || point.Y >= GameSize.Y)
                return;
            GameField[point.X, point.Y].Tick = Length;
            GameField[point.X, point.Y].Type = SquareType.Player;
            GameField[point.X, point.Y].PlayerName = player.Name;
            player.Position = point;
        }
        public static Color GetRandomColor()
        {
            var values = Enum.GetValues(typeof(KnownColor));
            KnownColor[] colors = new KnownColor[values.Length];
            values.CopyTo(colors, 0);
            values = null;
            List<KnownColor> colorlist = new List<KnownColor>(colors);
            colorlist.Remove(KnownColor.Black);
            colorlist.Remove(KnownColor.Blue);
            colorlist.Remove(KnownColor.Red);
            colorlist.RemoveAll(entry => Color.FromKnownColor(entry).IsSystemColor);
            colors = colorlist.ToArray();
            return Color.FromKnownColor(colors[new Random().Next(colors.Length)]);
        }
        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="filename"></param>
        public static void Load(string filename)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="filename"></param>
        public static void Save(string filename)
        {
            throw new NotImplementedException();
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
