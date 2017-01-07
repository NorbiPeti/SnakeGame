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
        public readonly int ID;
        public readonly bool Own;
        public Player(string name, int id, bool own = false)
        {
            Name = name;
            var values = Enum.GetValues(typeof(KnownColor));
            KnownColor[] colors = new KnownColor[values.Length];
            values.CopyTo(colors, 0);
            values = null;
            List<KnownColor> colorlist = new List<KnownColor>(colors);
            colorlist.Remove(KnownColor.Black);
            colorlist.Remove(KnownColor.Blue);
            colorlist.Remove(KnownColor.Red);
            colors = colorlist.ToArray();
            Color = Color.FromKnownColor(colors[new Random().Next(colors.Length)]);
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
