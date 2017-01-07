using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Collections.Specialized;
using System.Windows.Forms;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Net.Sockets;
using System.Threading;
using System.Drawing;

namespace SnakeGame
{
    public static class Network
    {
        public static void SyncUpdate(NetUpdateType updatetype) //If we are a server, forward every valid data we get
        {

        }
        public static List<NetMatch> Matches = new List<NetMatch>();
        public static NetMatch ConnectedMatch { get; private set; }
        public static void DownloadGameList()
        {
            using (var client = new WebClient())
            {
                var values = new NameValueCollection();
                values["client"] = "cheesecrescent"; //Sajtoskifli

                try
                {
                    var response = client.UploadValues("http://snakegame.16mb.com", values);

                    Matches.Clear();
                    var responseString = Encoding.Default.GetString(response);
                    string[] responses = responseString.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                    int x = 0;
                    while (x < responses.Length)
                    {
                        var match = new NetMatch();
                        match.Name = responses[x];
                        x++;
                        match.OwnerName = responses[x];
                        x++;
                        if (!Int32.TryParse(responses[x], out match.MaxPlayers))
                            MessageBox.Show("Error! The received text is in wrong format.");
                        x++;
                        int players;
                        if (!Int32.TryParse(responses[x], out players))
                            MessageBox.Show("Error! The received text is in wrong format.");
                        x++;
                        for (int i = x; i < x + players; i++)
                            match.Players.Add(new Player(responses[i], match.NextID++));
                        x += players;
                        //match.OwnerIP = IPAddress.Parse(responses[x]);
                        match.OwnerIP = responses[x].Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries).Select(entry => IPAddress.Parse(entry)).ToArray();
                        x++;
                        Matches.Add(match);
                    }
                }
                catch (WebException) { }
            }
        }
        public static void CreateGame(NetMatch match)
        {
            //MessageBox.Show("Create game: " + match.Name + " (" + match.MaxPlayers + " players)");
            using (var client = new WebClient())
            {
                var values = new NameValueCollection();
                values["client"] = Game.Player.Name;
                values["name"] = match.Name;
                values["maxplayers"] = match.MaxPlayers.ToString();

                /*IPAddress[] ip = GetIPs();
                if (ip == null)
                    return;*/
                values["ip"] = "";
                Array.ForEach(match.OwnerIP, new Action<IPAddress>(entry => values["ip"] += entry.ToString() + ";"));

                var response = client.UploadValues("http://snakegame.16mb.com", values);

                var responseString = Encoding.Default.GetString(response);
                if (responseString != "OK")
                    MessageBox.Show("Error!\n" + responseString);
                else
                {
                    Join(match);
                }
            }
        }
        public static void Connect(NetMatch match)
        {
            MessageBox.Show("Connect to game: " + match.Name + " (" + match.MaxPlayers + " players)");
            Join(match);
        }
        public static void Join(NetMatch match)
        {
            if (ConnectedMatch != null)
                Leave();
            ConnectedMatch = match;
            StartListening();
        }
        public static void Leave()
        {
            if (ConnectedMatch == null)
                return;
            ReceiverThread.Abort();
            foreach (Thread t in PlayerThreads)
                t.Abort();
            PlayerThreads.Clear();
        }
        public static IPAddress[] GetIPs()
        {
            //https://msdn.microsoft.com/en-us/library/ee663252%28v=vs.110%29.aspx
            try
            {
                return Dns.GetHostEntry(Dns.GetHostName()).AddressList.Where(entry => entry.AddressFamily == AddressFamily.InterNetworkV6).ToArray();
            }
            catch
            {
                MessageBox.Show("Error! Failed to get IP address.\nDoes your system support IPv6?");
                return null;
            }
        }
        public static Thread ReceiverThread;
        public static List<Thread> PlayerThreads = new List<Thread>();
        public static void StartListening()
        {
            if (ReceiverThread == null)
                (ReceiverThread = new Thread(new ThreadStart(ThreadRun))).Start();
        }
        private static void ThreadRun()
        {
            MessageBox.Show("Listener thread started.");
            var listener = new TcpListener(IPAddress.IPv6Any, 12885);
            listener.Start();
            while(true)
            {
                TcpClient client = listener.AcceptTcpClient();
                Thread t = new Thread(new ParameterizedThreadStart(ThreadPerPlayer));
                PlayerThreads.Add(t);
                t.Start(client);
            }
        }
        private static void ThreadPerPlayer(object c)
        {
            TcpClient client = c as TcpClient;
            NetworkStream ns = client.GetStream();
            BinaryReader br = new BinaryReader(ns);
            int is52=br.ReadInt32();
            if(is52!=52)
            {
                client.Close();
                return;
            }
            string playername = br.ReadString();
            Player player = new Player(playername, ConnectedMatch.NextID++); //Login==Connect
            player.Client = client;
            ConnectedMatch.Players.Add(player);
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
                        Game.MovePlayer(player, direction);
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
                    bw.Write(52);
                    bw.Write(playername);
                    bw.Write(updatetype);
                    yield return bw;
                }
            }
            yield break;
        }
    }
    public enum NetUpdateType
    {
        Name,
        Color,
        Move,
        //Login, - Login==Connect
        Leave
    }
}
