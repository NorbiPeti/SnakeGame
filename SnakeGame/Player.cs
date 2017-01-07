using System;
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
                name = value;
                if (Own)
                    Network.SyncUpdate(NetUpdateType.Name);
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
                    Network.SyncUpdate(NetUpdateType.Color);
            }
        }
        public Point Position;
        public TcpClient Client;
        //public readonly int ID;
        public readonly bool Own;
        public Player(string name, bool own = false, Color color = default(Color))
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
            Position = new Point(0, 0);
        }
        /*public static void Reset()
        {
            NextID = 0;
        }*/
    }
}
