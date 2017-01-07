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
            senddata["GameSize"]["X"] = Game.GameSize.X;
            senddata["GameSize"]["Y"] = Game.GameSize.Y;
            for (int i = 0; i < Game.GameSize.X; i++)
            {
                for (int j = 0; j < Game.GameSize.Y; j++)
                {
                    senddata["GameField"][i][j]["PlayerName"] = Game.GameField[i, j].PlayerName;
                    senddata["GameField"][i][j]["Tick"] = Game.GameField[i, j].Tick;
                    senddata["GameField"][i][j]["Type"] = Game.GameField[i, j].Type.ToString();
                }
            }
            foreach (Player joinedplayer in ConnectedMatch.Players)
            {
                senddata["Players"][joinedplayer.Name]["Position"]["X"] = joinedplayer.Position.X;
                senddata["Players"][joinedplayer.Name]["Position"]["Y"] = joinedplayer.Position.Y;
                senddata["Players"][joinedplayer.Name]["Color"] = joinedplayer.Color.ToArgb();
            }
            bwp.Write(senddata.ToString());
            while (true)
            {
                NetUpdateType updatetype = (NetUpdateType)br.ReadInt32();
                switch (updatetype)
                {
                    case NetUpdateType.Name:
                        string newname = br.ReadString();
                        player.Name = newname;
                        foreach (BinaryWriter bw in ForwardMessage(player, playername, (int)updatetype))
                        { //ForwardMessage prepares each send and then here the only thing to do is to send the extra data
                            bw.Write(newname);
                        }
                        break;
                    case NetUpdateType.Color:
                        Color color = Color.FromArgb(br.ReadInt32());
                        player.Color = color;
                        foreach (BinaryWriter bw in ForwardMessage(player, playername, (int)updatetype))
                        {
                            bw.Write(color.ToArgb());
                        }
                        break;
                    case NetUpdateType.Move:
                        Direction direction = (Direction)br.ReadInt32();
                        Game.MovePlayerPost(player, Game.MovePlayerPre(player, direction));
                        foreach (BinaryWriter bw in ForwardMessage(player, playername, (int)updatetype))
                        {
                            bw.Write((int)direction);
                        }
                        break;
                    /*case NetUpdateType.Login:
                        ConnectedMatch.Players.Add(new Player(playername, ConnectedMatch.NextID));
                        break;*/
                    case NetUpdateType.Leave:
                        foreach (BinaryWriter bw in ForwardMessage(player, playername, (int)updatetype))
                        {
                        }
                        Network.ConnectedMatch.Players.RemoveAll(entry => entry.Name == playername);
                        client.Close();
                        break;
                }
            }
        }
        private static IEnumerable<BinaryWriter> ForwardMessage(Player player, string playername, int updatetype)
        {
            if (ConnectedMatch.OwnerName == Game.Player.Name)
            {
                //foreach (Player p in ConnectedMatch.Players)
                Player p;
                while (ConnectedMatch.Players.GetEnumerator().MoveNext())
                {
                    p = ConnectedMatch.Players.GetEnumerator().Current;
                    if (p == player)
                        continue;
                    var bw = new BinaryWriter(p.Client.GetStream());
                    //bw.Write(52); - It should only send it once
                    //bw.Write(playername); - It should only send it once
                    bw.Write(updatetype);
                    yield return bw;
                }
            }
            yield break;
        }
    }
}
