using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Web;

using WebSocketSharp;
using Newtonsoft.Json;

/* TODO LIST
public event TabControlEventHandler Selected

TODO: I have no idea what I'm doing
TODO: DONE 1 Start off by implementing some way of getting your API Key (getKey())
TODO: 2* Get the flipping interface to run 'IN PARALLEL' with your other code. queue and events! Or THREADIIIIING
TODO: 3 Connect to test server (8722), use Type.InvokeMember and a static class to get delicious command parsing in a loop
TODO: 4 Port the rest of the API/Scraping
TODO: 5 Implement logic so the UI Components resize when the window does. 1/4 for the user list, all down the side. Three lines of text for the entry point, to the bottom. Rest TabPage.
*/

namespace CogitoSharp
{

	interface ICogitoPlugin
	{
	//TODO: PluginInterface. Featuring... er. Name of the plugin, command to be bound/delegate to be gotten. constructor and destructor? Or loop and exit.
	}

    static class Program
    {

        /// <summary>
        /// The main entry point for the application.
        /// </summary>

        public static CogitoUI cogitoUI = null;

#if DEBUG
        static WebSocket websocket = new WebSocket("ws://chat.f-list.net:8722");
#else
        static WebSocket websocket = new WebSocket(String.Format("ws://{0}:{1}", Properties.Settings.Default.Host, Properties.Settings.Default.Port));
#endif

        public static List<User> users = new List<User>();
        public static List<Channel> channels = new List<Channel>();
		public static string ticket;
		public static Queue<RawMessage> messageQueue = new Queue<RawMessage>();

        static void Main()
        {
			Application.SetCompatibleTextRenderingDefault(false);
            Application.EnableVisualStyles();

			ticket = getKey();
            cogitoUI = new CogitoUI();
			//cogitoUI.Invoke()
			var data = new Dictionary<string, string>();
			data["method"]="ticket";
			data["account"]=Properties.Settings.Default.Account;
			data["character"] = Properties.Settings.Default.Character;
			data["ticket"] = Ticket;
			data["cname"] = "COGITO";
			data["cversion"] = Application.ProductVersion;
			string openString = JsonConvert.SerializeObject(data);
			openString = "IDN "+openString;
			websocket.OnOpen += (sender, e) => websocket.Send(openString);

			#if DEBUG
			Console.WriteLine("Open String: "+openString);
			
			websocket.OnMessage += (sender, e) => //TODO: Insert RawMessage-to-Object-onto-Collections.Queue
			websocket.Connect();
			Thread.Sleep(1000);
			#endif
			Application.Run(cogitoUI);
        }
		
		static User getUser(string user){return Program.users.Find(x => x.Name == user);}
		static User getUser(User user){return user;}

		static private void digestRawMessage(string rawmessage)
		{
			string OPCODE = rawmessage.Substring(0, 3);
			Dictionary<string, string> Message = JsonConvert.DeserializeObject<Dictionary<string, string>>(rawmessage.Substring(3));
			
		}

		static void ProcessMessage(RawMessage msgobj)
		{
		//TODO: get message from queue, Type.InvokeMember on namespace/class FLISTPROCESSING
		}

        static private string getKey()
        {
            //cogitoUI.passwordPanel
            using (var wb = new WebClient())
            {
                var data = new NameValueCollection();
#if DEBUG
                data["account"] = "Cogito";
                data["password"] = "1ChD3Nk34Ls=!";
#else
                cogitoUI.passwordPanel.BringToFront();
                data["account"] = Properties.Settings.Default.Account;
                data["password"] = null; //TODO: Get Password from the panel and entry thingums
#endif
				var byteTicket = wb.UploadValues("http://www.f-list.net/json/getApiTicket.php", "POST", data);
				string t1 = System.Text.Encoding.ASCII.GetString(byteTicket);
				string t2 = t1.Substring(t1.IndexOf("\"ticket\":\"") + "\"ticket\":\"".Length);
				string t3 = t2.Substring(0, t2.IndexOf("\""));
				return t3;
            }
        }
    }
        
    class Channel : IComparable
    {
		public readonly int CID;
        public readonly string key;
        public readonly string name;
        private User[] users;
        private TabPage chanTab;
		private Int16 minAge = 0;
		public Int16 MinAge =0;
			
        public bool Join()
        {
            return true;
        }

        public void Leave()
        { }

        public Channel(string _key)
        {
            this.key = _key;
            this.name = "Unnamed Private Channel";
            this.chanTab = new ChatTab(this.name);
            Program.cogitoUI.chatTabs.TabPages.Add(this.chanTab);
			Program.channels.Add(this);
			this.CID=Program.channels.Count();
        }

        public Channel(string _key, string _name)
        {
            this.key = _key;
            this.name = _name;
            this.chanTab = new ChatTab(this.name);
            Program.cogitoUI.chatTabs.TabPages.Add(this.chanTab);
			Program.channels.Add(this);
			this.CID = Program.channels.Count;
        }

        ~Channel()
        {
            Program.cogitoUI.chatTabs.TabPages.Remove(this.chanTab);
        }

        public override string ToString()
        {
            return this.name;
        }

        public void onMessage()
        {
            //code to flash the tab/container/whatever

        }

        public override bool Equals(object obj)
        {
            if (obj == null) { return false; }
            Channel c = obj as Channel;
            if (this.name == c.name) { return true; }
            else { return false; }
        }

        public override int GetHashCode()
        {
            return this.CID;
        }

        public bool Equals(Channel channel)
        {
            if (this.name == channel.name) { return true; }
            else { return false; }
        }

        public int CompareTo(object obj)
        {
            if (obj == null) { return 1; }
            Channel c = obj as Channel;
            if (c != null) { return this.key.CompareTo(c.key); }
            else { throw new ArgumentException("Object cannot be made into Channel. Cannot CompareTo()."); }

        }
    }

    class User : IComparable
    {
        private readonly int UID = Program.users.Count + 1;
        public readonly string Name = "Unnamed Character";
		public readonly int Age = 0;
		private readonly string Gender = "None";
		public Dictionary<string,string> Stats;
		public Dictionary<string,List<string>> Kinks;

        public User(string nName)
        {this.Name = nName;}

        public User(string nName, int nAge)
        {
            this.Name = nName;
            this.Age = nAge;
        }

        public override string ToString(){return this.Name;}

        public override bool Equals(object obj)
        {
            if (obj == null) { return false; }
            User o = obj as User;
            if(this.Name == o.Name){return true;}
            else { return false; }
        }

        public override int GetHashCode()
        {
            return this.UID;
        }

        public bool Equals(User user)
        {
            if (this.Name == user.Name) { return true; }
            else { return false; }
        }

        public int CompareTo(object obj)
        {
            if (obj == null) { return 1; }
            User o = obj as User;
            if (o != null) { return this.Name.CompareTo(o.Name); }
            else { throw new ArgumentException("Object cannot be made into User. Cannot CompareTo()."); }

        }

    }

	//TODO: Oh god why this is horrible code
    public class RawMessage : EventArgs
    {
        public string opcode { get; set; }
        public Dictionary<string, string> args { get; set; }
    }

	public static struct Source
	{
		public User user;
		public Channel channel;

		public Source(User _user, Channel _channel)
		{
			user = _user;
			channel =_channel;
		}
	};

    class Message
    {
        public string[] args;
        public string prms;
		public string opcopde;
		public readonly Source source;
		
		


        Message()
        {

        }

    }

	sealed class FListProcessor
	{
	
	}
}
