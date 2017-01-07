using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame
{
    public static class Network
    {
        public static void SyncUpdate()
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://snakegame.16mb.com/");
            request.Method = "POST";
        }
    }
}
