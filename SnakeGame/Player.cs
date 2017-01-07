using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame
{
    public class Player
    {
        public static int NextID = 0;
        public string Name;
        public Color Color;
        public int ID;
        public Player(string name)
        {
            Name = name;
            var values=Enum.GetValues(typeof(KnownColor));
            KnownColor[] colors = new KnownColor[values.Length];
            values.CopyTo(colors, 0);
            Color = Color.FromKnownColor(colors[new Random().Next(colors.Length)]);
            ID = NextID;
            NextID++;
        }
    }
}
