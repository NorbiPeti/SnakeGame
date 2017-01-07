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

namespace SnakeGame
{
    public static class Network
    {
        public static void SyncUpdate()
        {
            /*using (var client = new HttpClient())
            {
                var values = new Dictionary<string, string>
    {
       { "thing1", "hello" },
       { "thing2", "world" }
    };

                var content = new FormUrlEncodedContent(values);

                //var response = await client.PostAsync("http://snakegame.16mb.com", content);
                Task task = client.PostAsync("http://snakegame.16mb.com", content);
                task.RunSynchronously();
                HttpResponseMessage response = (HttpResponseMessage)task.AsyncState;

                //var responseString = await response.Content.ReadAsStringAsync();
                task = response.Content.ReadAsStringAsync();
                task.RunSynchronously();
                string responseString = (string)task.AsyncState;
                Console.WriteLine("Received response: " + responseString);
            }*/
            using (var client = new WebClient())
            {
                var values = new NameValueCollection();
                values["client"] = "test";

                var response = client.UploadValues("http://snakegame.16mb.com", values);

                var responseString = Encoding.Default.GetString(response);
                Console.WriteLine("Received response: " + responseString);
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
                        match.OwnerIP = IPAddress.Parse(responses[x]);
                        x++;
                        Matches.Add(match);
                    }
                }
                catch (WebException) { }
            }
        }
        public static void CreateGame(NetMatch match)
        {
            MessageBox.Show("Create game: " + match.Name + " (" + match.MaxPlayers + " players)");
            using (var client = new WebClient())
            {
                var values = new NameValueCollection();
                values["client"] = Game.UserName;
                values["name"] = match.Name;
                values["maxplayers"] = match.MaxPlayers.ToString();

                IPAddress ip = GetIPs();
                if (ip == null)
                    return;
                values["ip"] = ip.ToString();

                var response = client.UploadValues("http://snakegame.16mb.com", values);

                var responseString = Encoding.Default.GetString(response);
                if (responseString != "OK")
                    MessageBox.Show("Error!\n" + responseString);
                else
                {
                    if (ConnectedMatch != null)
                        Leave();
                    ConnectedMatch = match;
                    StartListening();
                    //HttpWebRequest request = WebRequest.Create("http://masterserver2.raknet.com/testServer") as HttpWebRequest;
                    //JObject obj = new JObject();
                    //obj.Add("__gameId", JToken.Parse("mygame"));
                    /*obj["__gameId"] = "mygame";
                    obj["__clientReqId"] = "0";
                    obj["__timeoutSec"] = "300";
                    obj["__updatePW"] = "up";
                    obj["__readPW"] = "rp";
                    obj["gamename"] = "Test";
                    obj["gameport"] = "60000";*/
                    /*var postData = "{" +
                    "\"__gameId\": \"mygame\"," +
                    "\"__clientReqId\": \"0\"," +
                    "\"__timeoutSec\": \"300\"," +
                    "\"__updatePW\": \"up\"," +
                    "\"__readPW\": \"rp\"," +
                    "\"mapname\": \"DoomCastle\"," +
                    "\"gameport\": \"60000\"" +
                    "}";*/
                    //var data = Encoding.ASCII.GetBytes(postData);
                    /*var data = Encoding.ASCII.GetBytes(obj.ToString());
                    request.Method = "POST";
                    request.ContentType = "application/x-www-form-urlencoded";
                    request.ContentLength = data.Length;

                    using (var stream = request.GetRequestStream())
                    {
                        stream.Write(data, 0, data.Length);
                    }

                    var response2 = (HttpWebResponse)request.GetResponse();

                    responseString = new StreamReader(response2.GetResponseStream()).ReadToEnd();
                    obj = JObject.Parse(responseString);
                    MessageBox.Show(obj.ToString());*/
                    
                }
            }
        }
        public static void Connect(NetMatch match)
        {
            MessageBox.Show("Connect to game: " + match.Name + " (" + match.MaxPlayers + " players)");
        }
        public static void Leave()
        {
            if (ConnectedMatch == null)
                return;
        }
        public static IPAddress[] GetIPs()
        {
            //https://msdn.microsoft.com/en-us/library/ee663252%28v=vs.110%29.aspx
            try
            {
                return Dns.GetHostEntry(Dns.GetHostName()).AddressList.Single(entry => entry.AddressFamily == AddressFamily.InterNetworkV6);
            }
            catch
            {
                MessageBox.Show("Error! Failed to get IP address.\nDoes your system support IPv6?");
                return null;
            }
        }
        public static Thread ReceiverThread;
        public static void StartListening()
        {
            if (ReceiverThread == null)
                ReceiverThread = new Thread(new ThreadStart(ThreadRun));
        }
        public static void ThreadRun()
        {
            MessageBox.Show("Listener thread started.");
        }
    }
}
