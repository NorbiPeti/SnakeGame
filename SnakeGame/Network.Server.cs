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
            SafeNetStream ns = client.GetStream().ToSafeNetStream(client, Thread.CurrentThread);
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
            try
            {
                JObject readdata = JObject.Parse(br.ReadString());
                string playername = readdata["PlayerName"].ToString();
                /*int initialcolor;
                if (!int.TryParse(readdata["Color"].ToString(), out initialcolor))
                {
                    client.Close();
                    return;
                }*/
                if (Network.ConnectedMatch.Players.SingleOrDefault(entry => entry.Name == playername) != null)
                {
                    client.Close();
                    return;
                }
                Player joinedplayer = new Player(playername, color: Color.FromArgb((int)readdata["Color"])); //Login==Connect
                joinedplayer.Client = client;
                ConnectedMatch.Players.Add(joinedplayer);
                BinaryWriter bwp = new BinaryWriter(ns);
                var senddata = new JObject();
                senddata["Length"] = Game.Length;
                senddata["OwnerName"] = Game.Player.Name; //2015.08.29.
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
                foreach (Player player in ConnectedMatch.Players)
                {
                    if (player.Name == joinedplayer.Name)
                        continue;
                    senddata["Players"][player.Name] = new JObject();
                    senddata["Players"][player.Name]["Position"] = new JObject();
                    senddata["Players"][player.Name]["Position"]["X"] = player.Position.X;
                    senddata["Players"][player.Name]["Position"]["Y"] = player.Position.Y;
                    senddata["Players"][player.Name]["Color"] = player.Color.ToArgb();
                }
                bwp.Write(senddata.ToString());
                Game.Paused = false;
                SendUpdate = true;
                while (true)
                {
                    if (!ReceiveAndProcessData(joinedplayer, br))
                        break;
                }
            }
            catch
            {
            }
        }
        private static IEnumerable<BinaryWriter> ForwardMessage(Player player, string playername, int updatetype)
        {
            if (ConnectedMatch.OwnerName == Game.Player.Name)
            {
                Player p;
                //while (ConnectedMatch.Players.GetEnumerator().MoveNext())
                var en = ConnectedMatch.Players.GetEnumerator();
                while(en.MoveNext())
                {
                    p = ConnectedMatch.Players.GetEnumerator().Current;
                    if (p == null || p.Name == player.Name || p.Name == Game.Player.Name)
                        continue;
                    var bw = new BinaryWriter(p.Client.GetStream().ToSafeNetStream(p.Client, Thread.CurrentThread));
                    bw.Write(playername);
                    bw.Write(updatetype);
                    yield return bw;
                }
            }
            yield break;
        }
    }
}
