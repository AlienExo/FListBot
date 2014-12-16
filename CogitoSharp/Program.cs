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
using Newtonsoft.Json.Linq;
/* TODO LIST
 * public event TabControlEventHandler Selected
 * 
 * [|||||-----] TODO: I have no idea what I'm doing 
 * [||||||||||] TODO: DONE 1 Start off by implementing some way of getting your API Key (getKey())
 * [||||||||--] TODO: Split Login and actual interface into two things > this.hide(), open login form, if login succeeds and character is chosen, this.show()
 * 
 * 
 * [----------] TODO: 2* Get the flipping interface to run 'IN PARALLEL' with your other code. queue and events! Or THREADIIIIING
 * [----------] TODO: 3 Connect to test server (8722), use Type.InvokeMember and a static class to get delicious command parsing in a loop/event drived
 * [----------] TODO: Consider proper delegates for this thing
 * [----------] TODO: 4 Port the rest of the API/Scraping
 * [----------] TODO: 5 Implement logic so the UI Components resize when the window does. 1/4 for the user list, all down the side. Three lines of text for the entry point, to the bottom. Rest TabPage.
 * [----------] TODO: Status bar at the top of the program for multi-character chats
 * [----------] TODO: Tiny profile view of people you're talking to, on the side. web browser or scraping. 
 *		shows age, gender, name, avatar
 *		small info area with quickmatch - fave matche, no match, your faves in their no, your no in their fav
 *		Customizable filters - value (e.g. Age) from combobox, operator from a combobox, then a relevant value in the next box
 *		detach? new window?
 *		Custom highlight triggers (array string[])
 * TODO: Log formatting and output. As .txt or .html, activate/deacticate images (checkbox)
 * TODO: Tree Diagram of friends	*----*
 *									|------*
 *									
 * TODO: A Autocorrection as BLOSUM Like matrix of likelihood of typos, e.g. high for z and y, low for t and [Space] etc.
 * Settings panel on login form to set host and port
 * 
 * 
*/

namespace CogitoSharp
{
	/// <summary> Interface for all Plugins. Defining method triggers, commands, loop triggers and exit cleanup.
	/// </summary>
	interface ICogitoPlugin
	{
	//TODO: PluginInterface. Featuring... er. Name of the plugin, command to be bound/delegate to be gotten. constructor and destructor? Or loop and exit.
	}

	/// <summary> May be implemented for proper JSON serialization. Or not, because fuck this.
	/// </summary>
	
	/// <summary> Manages login and -out as well as Identity Management
	/// </summary>
	/// 

	class Account
	{
		protected internal static string account;
		protected internal static int accountID;
		protected internal static List<string> Characters = new List<string>();
		protected internal static string APIkey = "";
			
		protected internal static bool login(string _account, string _password){
			using (var wb = new WebClient()){
				var data = new NameValueCollection();
				data["account"] = _account;
				data["password"] = _password;
				var byteTicket = wb.UploadValues("http://www.f-list.net/json/getApiTicket.php", "POST", data);
				string t1 = System.Text.Encoding.ASCII.GetString(byteTicket);
				dynamic Response = JsonConvert.DeserializeObject(t1);
				if (Response.SelectToken("error").ToString().Length > 0) { return false; }
				else {
					//TODO: Get characters, set up list, insert.
					APIkey = Response.SelectToken("ticket");
					string characters = Response.SelectToken("characters").ToString();
					Characters.AddRange(characters.Split(','));
					#if DEBUG
					Console.WriteLine(Characters);
					#endif
					return true;
				}	
			}
		}

		protected internal static void characterSelect(string character){
			if(APIkey.Length<=0){throw new ArgumentException("No valid login ticket/API Key is present.");}
			var logindata = new Dictionary<string, string>();
			logindata["method"] = "ticket";
			logindata["account"] = account;
			logindata["character"] = character;
			logindata["ticket"] = APIkey;
			logindata["cname"] = "COGITO";
			logindata["cversion"] = Application.ProductVersion;
			
			string openString = JsonConvert.SerializeObject(logindata);
			openString = "IDN " + openString;
			#if DEBUG
			Console.WriteLine("Open String: " + openString);
			#endif
			Core.websocket.OnOpen += (sender, e) => Core.websocket.Send(openString);
		}
	}

	/// <summary> Websocket handling, server connection, threading, all that goodness
	/// </summary>
    static class Core{
		internal static CogitoUI cogitoUI = null;

		#if DEBUG
        internal static WebSocket websocket = new WebSocket("ws://chat.f-list.net:9722"); //8722 Dev, 9722 Real but dev server is down \o/
		#else
        internal static WebSocket websocket = new WebSocket(String.Format("ws://{0}:{1}", Properties.Settings.Default.Host, Properties.Settings.Default.Port));
		#endif

		internal static List<User> users = new List<User>();
        internal static List<Channel> channels = new List<Channel>();
		
		private static Queue<FListMessageEventArgs> IncomingMessageQueue = new Queue<FListMessageEventArgs>();
		private static Queue<string> OutgoingMessageQueue = new Queue<string>();
		private static Thread Sender = new Thread(SendMessage);
		private static volatile bool _sendForever = new bool();
		
		
		//public static Thread Receiver = new Thread();

		private static void SendMessage(){while(_sendForever){websocket.Send(OutgoingMessageQueue.Dequeue());}}

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
        static void Main(){
			//dampenedSpringDelta();
			
			Application.SetCompatibleTextRenderingDefault(false);
            Application.EnableVisualStyles();
            cogitoUI = new CogitoUI();
			websocket.OnMessage += (sender, e) => Console.WriteLine("Sender: "+sender.ToString() + "Event: "+e.Data.ToString());//TODO: Insert RawMessage-to-Object-onto-Collections.Queue
			//TODO: IMPLEMENT PROPERLY websocket.OnMessage += OnWebsocketMessage();
			try{Console.WriteLine(Properties.Settings.Default.userAutoComplete.Count);}
			catch(System.NullReferenceException){Properties.Settings.Default.userAutoComplete = new AutoCompleteStringCollection();}
			websocket.Connect();
			Thread.Sleep(100);
			//Sender.Start();
			Application.Run(cogitoUI);
			//Sender.Join();
        }

		private static EventHandler<MessageEventArgs> OnWebsocketMessage()
		{
			throw new NotImplementedException();
		}
		
		/// <summary> Fetches the corresponding User instance from the program's List of users
		/// </summary>
		/// <param name="user">Username (string) to look for</param>
		/// <returns></returns>
		public static User getUser(string username){
			User _user = Core.users.Find(x => x.Name == username);
			if (_user != null) {return _user;}
			else{
				_user = new User(username);
				return _user;
			}
		}
		
		/// <summary> Overloaded in order to immediately return User instances, as may happen.
		/// </summary>
		/// <param name="user">User instance.</param>
		/// <returns></returns>
		public static User getUser(User user){return user;}
		
		public static Channel getChannel(Channel channel) { return channel; }

		public static Channel getChannel(string channel){
			Channel _channel = Core.channels.Find(x => x.key == channel);
			if (_channel != null) { return _channel; }
			else{
				_channel = new Channel(channel);
				return _channel;
			}
		}

		static private void digestRawMessage(string rawmessage){			
		}

		static void ProcessMessageOntoQueue(EventHandler<MessageEventArgs> e){
		}

		static void ProcessMessageFromQueue(FListMessageEventArgs msgobj){
		//TODO: get message from queue, Type.InvokeMember on namespace/class FLISTPROCESSING
		//TODO: print how?
		//msgobj.opcode not in ["PRI", "MSG"]
		}

		/// <summary> Gets the API Ticket needed for login, using Account and Password, and prepares the openString to be submitted on webSocket opening to authenticate to the server
		/// </summary>
		/// 

		public static float[] dampenedSpringDelta(int numResults=25, float amplitude = 1f, float damping = 0.2f, float tension = 0.7f)
		{
			//dampened spring oscillation is preferable to straight-up sin wave.
			//newVelocity = oldVelocity * (1 - damping);             // 0:no damping; 1:full damping
			//newVelocity -= (oldPosition - restValue) * springTension;   // The force to pull it back to the resting point
			//newPosition = oldPosition + newVelocity;
			//if (input[x]) y = input[x];
			//	velo *= 1-damp.value;
			//	velo -= y*k.value;
			//	y += velo;
			//	ctx.lineTo(x, mid - y*amp.value);

			//which value is a full period? approx. 50
			float position = 0f;
			float velocity = 0f;
			float[] deviations = new float[numResults];
			for (int i = 0; i < numResults; i++)
			{
				//insert amplitude somehow.
				velocity = velocity * (1f - damping);
				velocity -= (position - damping) * tension;
				position += velocity;
				deviations[i] = position;
			}
			float average = deviations.Average();
			for (int i = 0; i < numResults; i++){
				deviations[i] = deviations[i] - average;
				Console.WriteLine(i.ToString() + "\t" + deviations[i].ToString());	
			}
			return deviations;
		}
    }
        
    public class Channel : IComparable{
		public readonly int CID;
		/// <summary>
		/// Keys are the UUID for private channels; channel title for normal. Always use .key for channel-specific commands.
		/// </summary>
        public readonly string key;
        public readonly string name;
        private User[] users;
        private TabPage chanTab;
		private Int16 minAge = 0;
		public Int16 MinAge =0;
			
        public bool Join(){
			
            return true;
        }

        public void Leave(){ }

        public Channel(string _name){
            this.key = _name;
            this.name = _name;
            this.chanTab = new ChatTab(this.name);
            Core.cogitoUI.chatTabs.TabPages.Add(this.chanTab);
			Core.channels.Add(this);
			this.CID=Core.channels.Count();
        }

        public Channel(string _key, string _name){
            this.key = _key;
            this.name = _name;
            this.chanTab = new ChatTab(this.name);
            Core.cogitoUI.chatTabs.TabPages.Add(this.chanTab);
			Core.channels.Add(this);
			this.CID = Core.channels.Count;
        }

        ~Channel(){
            Core.cogitoUI.chatTabs.TabPages.Remove(this.chanTab);
        }

        public override string ToString(){
            return this.name;
        }

        public event EventHandler<FListMessageEventArgs> OnMessage;
		
		public override bool Equals(object obj){
            if (obj == null) { return false; }
            Channel c = obj as Channel;
            if (this.name == c.name) { return true; }
            else { return false; }
        }

        public override int GetHashCode(){
            return this.CID;
        }

        public bool Equals(Channel channel){
            if (this.name == channel.name) { return true; }
            else { return false; }
        }

        public int CompareTo(object obj){
            if (obj == null) { return 1; }
            Channel c = obj as Channel;
            if (c != null) { return this.key.CompareTo(c.key); }
            else { throw new ArgumentException("Object cannot be made into Channel. Cannot CompareTo()."); }

        }
    }

    public class User : IComparable{
        private readonly int UID = Core.users.Count + 1;
        public readonly string Name = "Unnamed Character";
		public readonly int Age = 0;
		private readonly string Gender = "None";
		public Dictionary<string,string> Stats;
		public Dictionary<string,List<string>> Kinks;

        public User(string nName){this.Name = nName;}

        public User(string nName, int nAge){
            this.Name = nName;
            this.Age = nAge;
        }

        public override string ToString(){return this.Name;}

        public override bool Equals(object obj){
            if (obj == null) { return false; }
            User o = obj as User;
            if(this.Name == o.Name){return true;}
            else { return false; }
        }

        public override int GetHashCode(){
            return this.UID;
        }

        public bool Equals(User user){
            if (this.Name == user.Name) { return true; }
            else { return false; }
        }

        public int CompareTo(object obj){
            if (obj == null) { return 1; }
            User o = obj as User;
            if (o != null) { return this.Name.CompareTo(o.Name); }
            else { throw new ArgumentException("Object cannot be made into User. Cannot CompareTo()."); }

        }

    }

	//TODO: Oh god why this is horrible code
    public class FListMessageEventArgs : EventArgs{
        public string opcode { get; set; }
        public Dictionary<string, string> args { get; set; }
		Source? source = null;
		public FListMessageEventArgs(EventArgs e){
			string s = e.ToString();
			#if DEBUG
				Console.WriteLine(s);
			#endif
			string opcode = s.Substring(0, 3);
			Dictionary<string, string> Message = JsonConvert.DeserializeObject<Dictionary<string, string>>(s.Substring(3));
			if (Message.ContainsKey("Channel"))
				{
					source = new Source(Core.getUser(Message["User"]), Core.getChannel(Message["Channel"]));
				}

			else{}
			
			}
    }

	///<summary>
	///Represents the Source (User and Channel) of a Message. User and Channel may default to null for system messages.
	///</summary>
	struct Source{
		User user;
		Channel channel;

		public Source(User _user, Channel _channel){
			user = _user;
			channel =_channel;
		}
	};

	/*
    class Message{
        public string[] args;
        public string prms;
		public string opcopde;
		public Source source;
		
        Message()
        {

        }

    }
	*/

	/// <summary>
	/// Contains all methods that process FList server/client commands
	/// </summary>
	public sealed class FListProcessor{
		public static void BRO(){}
		public static void LIS(){}

	}	
	/*
	ACB This command requires chat op or higher. Request a character's account be banned from the server.<< ACB { "character": string }
	ADL Sends the client the current list of chatops. >> ADL { "ops": [string] }
	AOP The given character has been promoted to chatop. >> AOP { "character": string }
	AOP This command is admin only. Promotes a user to be a chatop (global moderator). << AOP { "character": string }
	AWC This command requires chat op or higher. Requests a list of currently connected alts for a characters account. << AWC { "character": string }
	BRO Incoming admin broadcast. >> BRO { "message": string }
	BRO This command is admin only. Broadcasts a message to all connections. << BRO { "message": string }
	CBL This command requires channel op or higher. Request the channel banlist. << CBL { "channel": string }
	CBU This command requires channel op or higher. Bans a character from a channel. << CBU {"character": string, "channel": string}
	CBU This command requires channel op or higher. Removes a user from a channel, and prevents them from re-entering. >> CBU {"operator":string,"channel":string,"character":string}
	CCR Create a private, invite-only channel. << CCR { "channel": string }
	CDS Alerts the client that that the channel's description has changed. This is sent whenever a client sends a JCH to the server. >> CDS { "channel": string, "description": string }
	CDS This command requires channel op or higher. Changes a channel's description. << CDS { "channel": string, "description": string }
	CHA Request a list of all public channels. << CHA
	CHA Sends the client a list of all public channels. >> CHA { "channels": [object] }
	CIU Invites a user to a channel. >> CIU { "sender":string,"title":string,"name":string }
	CIU This command requires channel op or higher. Sends an invitation for a channel to a user. << CIU { "channel": string, "character": string }
	CKU This command requires channel op or higher. Kicks a user from a channel. << CKU { "channel": string, "character": string }
	CKU This command requires channel op or higher. Kicks a user from a channel. >> CKU {"operator":string,"channel":string,"character":string}
	COA This command requires channel op or higher. Promotes a user to channel operator. >> COA {"character":"character_name", "channel":"channel_name"}
	COA This command requires channel op or higher. Request a character be promoted to channel operator (channel moderator). << COA { "channel": string, "character": string }
	COL Gives a list of channel ops. Sent in response to JCH. >> COL { "channel": string, "oplist": [string] }
	COL Requests the list of channel ops (channel moderators). << COL { "channel": string }
	CON After connecting and identifying you will receive a CON command, giving the number of connected users to the network. >> CON { "count": int }
	COR This command requires channel op or higher. Demotes a channel operator (channel moderator) to a normal user. << COR { "channel": string, "character": string }
	COR This command requires channel op or higher. Removes a channel operator. >> COR {"character":"character_name", "channel":"channel_name"}
	CRC This command is admin only. Creates an official channel. << CRC { "channel": string }
	CSO Sets the owner of the current channel to the character provided. >> CSO {"character":"string","channel":"string"}
	CSO This command requires channel op or higher. Set a new channel owner. << CSO {"character":"string","channel":"string"}
	CTU Temporarily bans a user from the channel for 1-90 minutes. A channel timeout. >> CTU {"operator":"string","channel":"string","length":int,"character":"string"}
	CTU This command requires channel op or higher. Temporarily bans a user from the channel for 1-90 minutes. A channel timeout. << CTU { "channel":string, "character":string, "length":num }
	CUB This command requires channel op or higher. Unbans a user from a channel. << CUB { channel: "channel", character: "character" }
	DOP The given character has been stripped of chatop status. >> DOP { "character": character }
	DOP This command is admin only. Demotes a chatop (global moderator). << DOP { "character": string }
	ERR Indicates that the given error has occurred. ERR {"message": "You have already joined this channel.", "number": 28}
	FKS Search for characters fitting the user's selections. Kinks is required, all other parameters are optional. << FKS { "kinks": [int], "genders": [enum], "orientations": [enum], "languages": [enum], "furryprefs": [enum], "roles": [enum] }
	FKS Sent by as a response to the client's FKS command, containing the results of the search. >> FKS { "characters": [object], "kinks": [object] }
	FLN Sent by the server to inform the client a given character went offline. >> FLN { "character": string }
	FRL Initial friends list. >> FRL { "characters": [string] }
	HLO Server hello command. Tells which server version is running and who wrote it. >> HLO { "message": string }
	ICH Initial channel data. Received in response to JCH, along with CDS. >> ICH { "users": [object], "channel": string, "title": string, "mode": enum }
	* ICH {"users": [{"identity": "Shadlor"}, {"identity": "Bunnie Patcher"}, {"identity": "DemonNeko"}, {"identity": "Desbreko"}, {"identity": "Robert Bell"}, {"identity": "Jayson"}, {"identity": "Valoriel Talonheart"}, {"identity": "Jordan Costa"}, {"identity": "Skip Weber"}, {"identity": "Niruka"}, {"identity": "Jake Brian Purplecat"}, {"identity": "Hexxy"}], "channel": "Frontpage", mode: "chat"}
	IDN This command is used to identify with the server. << IDN { "method": "ticket", "account": string, "ticket": string, "character": string, "cname": string, "cversion": string }
	IDN Used to inform the client their identification is successful, and handily sends their character name along with it.>> IDN { "character": string }
	IGN A multi-faceted command to handle actions related to the ignore list. The server does not actually handle much of the ignore process, as it is the client's responsibility to block out messages it recieves from the server if that character is on the user's ignore list. << 
	IGN Handles the ignore list. >> IGN { "action": string, "characters": [string] | "character":object }
	JCH Indicates the given user has joined the given channel. This may also be the client's character. >> JCH { "channel": string, "character": object, "title": string }
	JCH Send a channel join request. << JCH { "channel": string }
	KID Kinks data in response to a KIN client command. >> KID { "type": enum, "message": string, "key": [int], "value": [int] }
	KIK This command requires chat op or higher. Request a character be kicked from the server. << KIK { "character": string }
	KIN Request a list of a user's kinks. << KIN { "character": string }
	LCH An indicator that the given character has left the channel. This may also be the client's character. >> LCH { "channel": string, "character": character }
	LCH Request to leave a channel. << LCH { "channel": string }
	LIS Sends an array of all the online characters and their gender, status, and status message. >> LIS { characters: [object] }
	LRP A roleplay ad is received from a user in a channel. LRP { "channel": "", "message": "", "character": ""}
	MSG A message is received from a user in a channel. >> MSG { "character": string, "message": string, "channel": string }
	MSG Sends a message to all other users in a channel. << MSG { "channel": string, "message": string }
	NLN A user connected. >> NLN { "identity": string, "gender": enum, "status": enum }
	ORS Gives a list of open private rooms. >> ORS { "channels": [object] } 
	* {"name":"ADH-300f8f419e0c4814c6a8","characters":0,"title":"Ariel's Fun Club"}
	ORS Request a list of open private rooms. << ORS
	PIN Ping command from the server, requiring a response, to keep the connection alive. >> PIN 
	PIN Sends a ping response to the server. Timeout detection, and activity to keep the connection alive. << PIN
	PRD Profile data commands sent in response to a PRO client command. >> PRD { "type": enum, "message": string, "key": string, "value": string }
	PRI A private message is received from another user. >> PRI { "character": string, "message": string }
	PRI Sends a private message to another user. << PRI { "recipient": string, "message": string }
	PRO Requests some of the profile tags on a character, such as Top/Bottom position and Language Preference. << PRO { "character": string }
	RLD This command requires chat op or higher. Reload certain server config files << RLD { "save": string }
	RLL Roll dice or spin the bottle. << RLL { "channel": string, "dice": string }
	RLL Rolls dice or spins the bottle. 
	RMO Change room mode to accept chat, ads, or both. >> RMO {"mode": enum, "channel": string}
	RMO This command requires channel op or higher. Change room mode to accept chat, ads, or both. << RMO {"channel": string, "mode": enum}
	RST This command requires channel op or higher. Sets a private room's status to closed or open. (private << RST { "channel": string, "status": enum }
	RTB Real-time bridge. Indicates the user received a note or message, right at the very moment this is received. >> RTB { "type": string, "character": string }
	RWD This command is admin only. Rewards a user, setting their status to 'crown' until they change it or log out. << RWD { "character": string }
	SFC Alerts admins and chatops (global moderators) of an issue. << SFC { "action": "report", "report": string, "character": string }
	SFC Alerts admins and chatops (global moderators) of an issue. >> SFC {action:"string", moderator:"string", character:"string", timestamp:"string"}
	STA A user changed their status >> STA { status: "status", character: "channel", statusmsg:"statusmsg" }
	STA Request a new status be set for your character. << STA { "status": enum, "statusmsg": string }
	SYS An informative autogenerated message from the server. >> SYS { "message": string, "channel": string }
	TMO This command requires chat op or higher. Times out a user for a given amount minutes. << TMO { "character": string, "time": int, "reason": string }
	TPN "user x is typing/stopped typing/has entered text" for private messages. << TPN { "character": string, "status": enum }
	TPN A user informs you of his typing status. >> TPN { "character": string, "status": enum }
	UBN This command requires chat op or higher. Unbans a character's account from the server. << UBN { "character": string }
	UPT Informs the client of the server's self-tracked online time, and a few other bits of information >> UPT { "time": int, "starttime": int, "startstring": string, "accepted": int, "channels": int, "users": int, "maxusers": int }
	UPT Requests info about how long the server has been running, and some stats about usage. << UPT
	VAR Variables the server sends to inform the client about server variables. 
	*	priv_max: Maximum number of bytes allowed with PRI.
	*	lfrp_max: Maximum number of bytes allowed with LRP.
	*	lfrp_flood: Required seconds between LRP messages.
	*	chat_flood: Required seconds between MSG messages.
	*	permissions: Permissions mask for this character.
	*  chat_max: Maximum number of bytes allowed with MSG.	
	*/

}
