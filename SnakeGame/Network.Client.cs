using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SnakeGame
{
    partial class Network
    {
        private static void ClientListenerThreadRun()
        {
            //Connect to the server
            TcpClient client = new TcpClient(AddressFamily.InterNetworkV6);
            client.Connect(ConnectedMatch.OwnerIP, Port);
            var ns = client.GetStream();
            var sw = new BinaryWriter(ns);
            sw.Write(52);
            JObject writedata = new JObject();
            writedata["PlayerName"] = Game.Player.Name;
            writedata["Color"] = Game.Player.Color.ToArgb();
            sw.Write(writedata.ToString());
            var sr = new BinaryReader(ns);
            JObject readdata = JObject.Parse(sr.ReadString());
            Game.GameSize = new Point((int)readdata["GameSize"]["X"], (int)readdata["GameSize"]["Y"]);
            Game.GameField = new SqCoord[Game.GameSize.X, Game.GameSize.Y];
            Game.Length = (int)readdata["Length"];
            for (int i = 0; i < Game.GameSize.X; i++)
            {
                for (int j = 0; j < Game.GameSize.Y; j++)
                {
                    Game.GameField[i, j].PlayerName = (string)readdata["GameField"][i + ""][j + ""]["PlayerName"];
                    Game.GameField[i, j].Tick = (int)readdata["GameField"][i + ""][j + ""]["Tick"];
                    Game.GameField[i, j].Type = (SquareType)Enum.Parse(typeof(SquareType), (string)readdata["GameField"][i + ""][j + ""]["Type"]);
                }
            }
            foreach(JProperty item in readdata["Players"])
            {
                ConnectedMatch.Players.Add(new Player(item.Name, color: Color.FromArgb((int)item.Value["Color"]), x: (int)item.Value["Position"]["X"], y: (int)item.Value["Position"]["Y"]));
            }
            Game.Paused = false;
            SendUpdate = true;
            while (true)
            {
                string playername = sr.ReadString();
                Player player = ConnectedMatch.Players.Single(entry => entry.Name == playername);
                ReceiveAndProcessData(player, sr);
            }
        }
    }
}
