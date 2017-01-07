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
        /// <summary>
        /// TODO: Send player join/leave to master server
        /// </summary>
        //public List<Player> Players = new List<Player>();
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
            try
            {
                //return Players.Single(entry => entry.ID == id);
                return Players.Single(entry => entry.Name == name);
            }
            catch
            {
                return null;
            }
        }
        //public int NextID = 0;
    }
}
