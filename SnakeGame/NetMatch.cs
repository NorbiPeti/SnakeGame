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
        public PlayerCollection Players = new PlayerCollection();
        
        private string ownername = "";
        public string OwnerName
        {
            get
            {
                return ownername;
            }
            set
            {
                foreach (Player player in Players)
                {
                    if (player.Name == ownername)
                    {
                        player.Name = value;
                        break;
                    }
                }
                ownername = value;
            }
        }
        public IPAddress[] OwnerIP;
        public Player GetPlayerByName(string name)
        {
            return Players.FirstOrDefault(entry => entry.Name == name);
        }
    }
}
