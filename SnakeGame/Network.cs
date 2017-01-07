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
                            match.Players.Add(new Player(responses[i]));
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
            }
            Listener.Stop();
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
