using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SnakeGame
{
    partial class Network
    {
        private static TcpListener Listener;
        private static void ServerListenerThreadRun()
        {
            //MessageBox.Show("Listener thread started.");
            Listener = new TcpListener(IPAddress.IPv6Any, Port);
            Listener.Start();
            while (true)
            {
                TcpClient client;
                try
                {
                    client = Listener.AcceptTcpClient();
                }
                catch (SocketException)
                {
                    return;
                }
                Thread t = new Thread(new ParameterizedThreadStart(ThreadPerPlayer));
                PlayerThreads.Add(t);
                t.Start(client);
            }
        }
        /// <summary>
        /// Serverside
        /// </summary>
        /// <param name="c"></param>
        private static void ThreadPerPlayer(object c)
        {
            TcpClient client = c as TcpClient;
            NetworkStream ns = client.GetStream();
            BinaryReader br = new BinaryReader(ns);
            StopEventPerPlayer += delegate
            {
                client.Close(); //Then the thread should abort
            };
            int is52 = br.ReadInt32();
            if (is52 != 52)
            {
                client.Close();
                return;
            }
            //Read and write inital data
            //string playername = br.ReadString();
            JObject readdata = JObject.Parse(br.ReadString());
            string playername = readdata["PlayerName"].ToString();
            /*int initialcolor;
            if (!int.TryParse(readdata["Color"].ToString(), out initialcolor))
            {
                client.Close();
                return;
            }*/
            Player player = new Player(playername, color: Color.FromArgb((int)readdata["Color"])); //Login==Connect
            player.Client = client;
            ConnectedMatch.Players.Add(player);
            BinaryWriter bwp = new BinaryWriter(ns);
            var senddata = new JObject();
            senddata["Length"] = Game.Length;
            senddata["GameSize"] = new JObject();
            senddata["GameSize"]["X"] = Game.GameSize.X;
            senddata["GameSize"]["Y"] = Game.GameSize.Y;
            senddata["GameField"] = new JObject();
            for (int i = 0; i < Game.GameSize.X; i++)
            {
                senddata["GameField"][i + ""] = new JObject();
                for (int j = 0; j < Game.GameSize.Y; j++)
                {
                    senddata["GameField"][i + ""][j + ""] = new JObject();
                    senddata["GameField"][i + ""][j + ""]["PlayerName"] = Game.GameField[i, j].PlayerName;
                    senddata["GameField"][i + ""][j + ""]["Tick"] = Game.GameField[i, j].Tick;
                    senddata["GameField"][i + ""][j + ""]["Type"] = Game.GameField[i, j].Type.ToString();
                }
            }
            senddata["Players"] = new JObject();
            foreach (Player joinedplayer in ConnectedMatch.Players)
            {
                if (joinedplayer.Name == player.Name)
                    continue;
                senddata["Players"][joinedplayer.Name] = new JObject();
                senddata["Players"][joinedplayer.Name]["Position"] = new JObject();
                senddata["Players"][joinedplayer.Name]["Position"]["X"] = joinedplayer.Position.X;
                senddata["Players"][joinedplayer.Name]["Position"]["Y"] = joinedplayer.Position.Y;
                senddata["Players"][joinedplayer.Name]["Color"] = joinedplayer.Color.ToArgb();
            }
            bwp.Write(senddata.ToString());
            Game.Paused = false;
            SendUpdate = true;
            while (true)
            {
                if (!ReceiveAndProcessData(player, br))
                    break;
            }
        }
        private static IEnumerable<BinaryWriter> ForwardMessage(Player player, string playername, int updatetype)
        {
            if (ConnectedMatch.OwnerName == Game.Player.Name)
            {
                Player p;
                while (ConnectedMatch.Players.GetEnumerator().MoveNext())
                {
                    p = ConnectedMatch.Players.GetEnumerator().Current;
                    if (p == player)
                        continue;
                    var bw = new BinaryWriter(p.Client.GetStream());
                    bw.Write(playername);
                    bw.Write(updatetype);
                    yield return bw;
                }
            }
            yield break;
        }
    }
}
