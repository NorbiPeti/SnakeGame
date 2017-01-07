using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame
{
    public class Player
    {
        //public static int NextID = 0;
        private string name = "";
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                if (Own)
                    Network.SyncUpdate(NetUpdateType.Name, value);
                name = value; //Only set name after sending update (which sends old name too)
                Form1.RefreshPlayerList();
            }
        }
        private Color color;
        public Color Color
        {
            get
            {
                return color;
            }
            set
            {
                color = value;
                if (Own)
                    Network.SyncUpdate(NetUpdateType.Color, value);
                Form1.RefreshPlayerList();
            }
        }
        public Point Position;
        public TcpClient Client;
        private int score;
        public int Score
        {
            get
            {
                return score;
            }
            set
            {
                score = value;
                if (Own)
                    Network.SyncUpdate(NetUpdateType.Score, value);
                Form1.RefreshPlayerList();
            }
        }
        private int lives;
        public int Lives
        {
            get
            {
                return lives;
            }
            set
            {
                lives = value;
                if (Own)
                    Network.SyncUpdate(NetUpdateType.Lives, value);
                Form1.RefreshPlayerList();
            }
        }
        //public readonly int ID;
        public readonly bool Own;
        public Player(string name, bool own = false, Color color = default(Color), int x = 0, int y = 0)
        {
            Name = name;
            if (color == default(Color))
            {
                Color = Game.GetRandomColor();
            }
            else
                Color = color;
            //ID = NextID;
            //NextID++;
            Own = own;
            //Position = new Point(0, 0);
            if (x != 0 && y != 0)
                Position = new Point(x, y);
            else
                Position = new Point(Game.GameSize.X / 2, 1);
            Score = 0;
            Lives = 3;
        }
        /*public static void Reset()
        {
            NextID = 0;
        }*/
    }
    public class PlayerCollection : IList<Player>
    {
        private List<Player> _list = new List<Player>();

        public int IndexOf(Player item)
        {
            return _list.IndexOf(item);
        }

        public void Insert(int index, Player item)
        {
            _list.Insert(index, item);
            Form1.RefreshPlayerList();
        }

        public void RemoveAt(int index)
        {
            _list.RemoveAt(index);
            Form1.RefreshPlayerList();
        }

        public Player this[int index]
        {
            get
            {
                return _list[index];
            }
            set
            {
                _list[index] = value;
            }
        }

        public void Add(Player item)
        {
            _list.Add(item);
            Form1.RefreshPlayerList();
        }

        public void Clear()
        {
            _list.Clear();
            Form1.RefreshPlayerList();
        }

        public bool Contains(Player item)
        {
            return _list.Contains(item);
        }

        public void CopyTo(Player[] array, int arrayIndex)
        {
            _list.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return _list.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool Remove(Player item)
        {
            bool ret = _list.Remove(item);
            Form1.RefreshPlayerList();
            return ret;
        }

        public int RemoveAll(Predicate<Player> match)
        {
            return _list.RemoveAll(match);
        }

        public IEnumerator<Player> GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _list.GetEnumerator();
        }
    }
}
