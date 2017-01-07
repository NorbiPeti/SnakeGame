using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame
{
    public class NetMatch
    {
        public string Name = "";
        public int MaxPlayers = 0;
        //public List<string> PlayerNames = new List<string>();
        public List<Player> Players = new List<Player>();
        public string OwnerName = "";
        public IPAddress OwnerIP;
        public Player GetPlayerByID(int id)
        {
            return Players.Single(entry => entry.ID == id);
        }
    }
}
