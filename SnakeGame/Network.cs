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
using Newtonsoft.Json;

namespace SnakeGame
{
    public static partial class Network
    {
        public const int Port = 12885;
        public static void SyncUpdate(NetUpdateType updatetype, object data)
        {
            if (ConnectedMatch == null || !SendUpdate)
                return;
            try
            {
                BinaryWriter bw;
                foreach (Player player in ConnectedMatch.Players)
                {
                    if (player.Name == Game.Player.Name)
                        continue; //Don't send to ourselves
                    bool isserver = ConnectedMatch.OwnerName == Game.Player.Name;
                    if (!isserver)
                    {
                        //bw = new BinaryWriter(ConnectedMatch.GetPlayerByName(ConnectedMatch.OwnerName).Client.GetStream().ToSafeNetStream()); //If not server, send only to server
                        var c = ConnectedMatch.GetPlayerByName(ConnectedMatch.OwnerName).Client; //If not server, send only to server
                        bw = new BinaryWriter(c.GetStream().ToSafeNetStream(c, null));
                    }
                    else
                    {
                        //bw = new BinaryWriter(player.Client.GetStream().ToSafeNetStream());
                        var c = player.Client;
                        bw = new BinaryWriter(c.GetStream().ToSafeNetStream(c, null));
                        bw.Write(Game.Player.Name); //Otherwise write playername as listener expects
                    }
                    bw.Write((int)updatetype);
                    switch (updatetype)
                    {
                        case NetUpdateType.Name:
                            string newname = (string)data;
                            bw.Write(newname);
                            break;
                        case NetUpdateType.Color:
                            int color = ((Color)data).ToArgb();
                            bw.Write(color);
                            break;
                        case NetUpdateType.Move:
                            int direction = (int)data; //Converting to enum and back to int is unnecessary
                            bw.Write(direction);
                            break;
                        case NetUpdateType.Leave:
                            break;
                        case NetUpdateType.Teleport:
                            Point point = (Point)data;
                            bw.Write(point.X);
                            bw.Write(point.Y);
                            break;
                        case NetUpdateType.Score:
                            int score = (int)data;
                            bw.Write(score);
                            break;
                        case NetUpdateType.Lives:
                            int lives = (int)data;
                            bw.Write(lives);
                            break;
                        case NetUpdateType.Pause:
                            bool pause = (bool)data;
                            bw.Write(pause);
                            break;
                    }
                    if (!isserver)
                        break; //If not server, only send to the server
                }
            }
            catch (IOException)
            {
            }
            catch (Exception e)
            {
                Program.HandleException(e);
            }
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
                            match.Players.Add(new Player(responses[i]));
                        x += players;
                        //match.OwnerIP = IPAddress.Parse(responses[x]);
                        match.OwnerIP = responses[x].Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries).Select(entry => IPAddress.Parse(entry)).ToArray();
                        x++;
                        Matches.Add(match);
                    }
                }
                catch (WebException) { }
                catch (Exception e)
                {
                    Program.HandleException(e);
                }
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
                values["action"] = "create";

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
                    Join(match, true);
                }
            }
        }
        public static void Connect(NetMatch match)
        {
            //MessageBox.Show("Connect to game: " + match.Name + " (" + match.MaxPlayers + " players)");
            Join(match, false);
        }
        public static void Join(NetMatch match, bool server)
        {
            if (ConnectedMatch != null)
                Leave();
            ConnectedMatch = match;
            StartListening(server);
        }
        private static event EventHandler StopEventPerPlayer;
        public static void Leave()
        {
            if (ConnectedMatch == null)
                return;
            SendUpdate = false;
            SyncUpdate(NetUpdateType.Leave, null);
            if (ConnectedMatch.OwnerName == Game.Player.Name)
            {
                using (var client = new WebClient())
                {
                    var values = new NameValueCollection();
                    values["client"] = Game.Player.Name;
                    values["name"] = ConnectedMatch.Name;
                    values["maxplayers"] = ConnectedMatch.MaxPlayers.ToString();
                    values["action"] = "remove";

                    /*IPAddress[] ip = GetIPs();
                    if (ip == null)
                        return;*/
                    values["ip"] = "";
                    Array.ForEach(ConnectedMatch.OwnerIP, new Action<IPAddress>(entry => values["ip"] += entry.ToString() + ";"));

                    var response = client.UploadValues("http://snakegame.16mb.com", values);

                    var responseString = Encoding.Default.GetString(response);
                    if (responseString != "OK")
                        MessageBox.Show("Error!\n" + responseString);
                }
            Listener.Stop();
            }
            ReceiverThread.Abort();
            if (StopEventPerPlayer != null)
                StopEventPerPlayer(null, null);
            foreach (Thread t in PlayerThreads)
                t.Abort();
            PlayerThreads.Clear();
            ConnectedMatch = null;
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
        public static void StartListening(bool server)
        {
            /*if (ReceiverThread == null)
                (ReceiverThread = new Thread(new ThreadStart(server ? ServerListenerThreadRun : ClientListenerThreadRun))).Start();*/
            if (ReceiverThread == null)
            {
                if (server)
                    (ReceiverThread = new Thread(new ThreadStart(ServerListenerThreadRun))).Start();
                else
                    (ReceiverThread = new Thread(new ThreadStart(ClientListenerThreadRun))).Start();
            }
        }
        public static bool SendUpdate = false;
        private static bool ReceiveAndProcessData(Player player, BinaryReader br)
        {
            string playername = player.Name;
            try
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
                        player.Client.Close();
                        break;
                    case NetUpdateType.Teleport:
                        player.Position = new Point(br.ReadInt32(), br.ReadInt32());
                        Game.MovePlayerPost(player, player.Position);
                        foreach (BinaryWriter bw in ForwardMessage(player, playername, (int)updatetype))
                        {
                            bw.Write(player.Position.X);
                            bw.Write(player.Position.Y);
                        }
                        break;
                    case NetUpdateType.Score:
                        player.Score = br.ReadInt32();
                        foreach (BinaryWriter bw in ForwardMessage(player, playername, (int)updatetype))
                        {
                            bw.Write(player.Score);
                        }
                        break;
                    case NetUpdateType.Lives:
                        player.Lives = br.ReadInt32();
                        foreach (BinaryWriter bw in ForwardMessage(player, playername, (int)updatetype))
                        {
                            bw.Write(player.Lives);
                        }
                        break;
                    case NetUpdateType.Pause:
                        Form1.SetTimerWithoutSend(!br.ReadBoolean());
                        foreach(BinaryWriter bw in ForwardMessage(player, playername, (int)updatetype))
                        {
                            bw.Write(Game.Paused);
                        }
                        break;
                }
            }
            catch (IOException)
            {
                return false;
            }
            catch (Exception e)
            {
                Program.HandleException(e);
            }
            return true;
        }
    }
    public enum NetUpdateType
    {
        Name,
        Color,
        Move,
        //Login, - Login==Connect
        Leave,
        Teleport,
        Score,
        Lives,
        Pause
    }
}
