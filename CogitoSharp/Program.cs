﻿#region Library Imports
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
#endregion

#region To-do list
/* TODO LIST
 * public event TabControlEventHandler Selected
 * 
 * To try: Send raw AOP w/o op status. Send login for a different character w/ valid ticket for another
 * 
 * [|||||||---] TODO: I have no idea what I'm doing 
 * [||||||||||] TODO: DONE 1 Start off by implementing some way of getting your API Key (getKey())
 * [||||||||--] TODO: Split Login and actual interface into two things > this.hide(), open login form, if login succeeds and character is chosen, this.show()
 * 
 * The advanced login options should probably be processed befoooooore opening the ws://
 * [----------] TODO: 2* Get the flipping interface to run 'IN PARALLEL' with your other code. queue and events! Or THREADIIIIING
 * [----------] TODO: 3 Connect to test server (8722), use Type.InvokeMember and a static class to get delicious command parsing in a loop/event drived
 * [----------] TODO: Consider proper delegates for this thing
 * [----------] TODO: 4 Port the rest of the API/Scraping
 * [----------] TODO: 5 Implement logic so the UI Components resize when the window does. 1/4 for the user list, all down the side. Three lines of text for the entry point, to the bottom. Rest TabPage.
 * [----------] TODO: Status bar at the top of the program for multi-character chats
 * [----------] TODO: Tiny profile view of people you're talking to, on the side. web browser or scraping. 
 * [----------]	shows age, gender, name, avatar
 * [----------]		small info area with quickmatch - fave matche, no match, your faves in their no, your no in their fav
 * [----------]		Customizable filters - value (e.g. Age) from combobox, operator from a combobox, then a relevant value in the next box
 * [----------]		detach? new window?
 * [----------]		Custom highlight triggers (array string[])
 * [----------] TODO: Log formatting and output. As .txt or .html, activate/deacticate images (checkbox)
 * [----------] TODO: Tree Diagram of friends	*----*
 *												|------*
 *									
 * TODO: A Autocorrection as BLOSUM Like matrix of likelihood of typos, e.g. high for z and y, low for t and [Space] etc.
 * [----------] Settings panel on login form to set host and port
 * 
 * 
*/
#endregion

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
	public interface ILoginKey{
		string error { get; set; }
		string ticket { get; set; }
	}

	public class LoginKey : ILoginKey{
		/// <summary>Server-side account number</summary>
		public int account_id { get; set; }
		/// <summary>character set as default on the server</summary>
		public string default_character { get; set; }
		/// <summary>All characters on the account. Limited to 30 for normal users.</summary>
		public List<string> characters { get; set; }
		/// <summary>Login error message (if any)</summary>
		public string error { get; set; }
		/// <summary>Characters bookmarked</summary>
		public List<Dictionary<string, string>> bookmarks { get; set; }
		/// <summary>List of characters befriended, and whom by</summary>
		public List<Dictionary<string, string>> friends { get; set; }
		/// <summary>The API Ticket used to access the system</summary>
		public string ticket { get; set; }
	}

	public class LoginKeyConverter : Newtonsoft.Json.Converters.CustomCreationConverter<ILoginKey>{
		public override ILoginKey Create(Type objectType){
			return new LoginKey();
		}
	}

	class Account
	{
		protected internal static string account;
		protected internal static LoginKey loginkey = null;
		protected internal static List<string> bookmarks = new List<string>();
			
		protected internal static bool login(string _account, string _password, out string error){
			#if NOCONNECT
			error = "DEBUG MODE";
			loginkey = new LoginKey();
			loginkey.account_id = 000001;
			bookmarks.Add("DEBUG BOOKMARK CHARACTER");
			loginkey.characters = new List<string>();
			loginkey.characters.Add("DEBUG USER CHARACTER");
			loginkey.characters.Add("DEBUG DEFAULT CHARACTER");
			loginkey.default_character = "DEBUG DEFAULT CHARACTER";
			loginkey.ticket = "t_DEBUGTICKERXXXXXXXX0000000001";
			return true;
			#endif
			using (var wb = new WebClient()){
				var data = new NameValueCollection();
				data["account"] = _account;
				data["password"] = _password;
				var byteTicket = wb.UploadValues("https://www.f-list.net/json/getApiTicket.php", "POST", data);
				string t1 = System.Text.Encoding.ASCII.GetString(byteTicket);
				loginkey = JsonConvert.DeserializeObject<LoginKey>(t1, new LoginKeyConverter());
				if (loginkey.error.Length > 0) {
					error = loginkey.error;
					return false;}
				else {
					Account.account = _account;
					error = "";
					foreach (Dictionary<string, string> d in loginkey.bookmarks){
						foreach (KeyValuePair<string, string> kv in d){Account.bookmarks.Add(kv.Value);}
					}
					Account.bookmarks.Sort();
					loginkey.bookmarks.Clear();
					return true;
				}	
			}
		}

		protected internal static void characterSelect(string character){
			if(loginkey.ticket.Length<=0){throw new ArgumentException("No valid login ticket/API Key is present.");}
			var logindata = new Dictionary<string, string>();
			logindata["method"] = "ticket";
			logindata["account"] = Account.account;
			logindata["character"] = character;
			logindata["ticket"] = Account.loginkey.ticket;
			logindata["cname"] = "COGITO";
			logindata["cversion"] = Application.ProductVersion;
			string openString = JsonConvert.SerializeObject(logindata);
			openString = "IDN " + openString;
			#if DEBUG
			Console.WriteLine("Open String: " + openString);
			#endif
			Core.websocket.OnOpen += (sender, e) => Core.websocket.Send(openString);
			Core.websocket.OnOpen += (sender, e) => Console.WriteLine("OPENED WEBSOCKET");
		}
	}

	/// <summary> Websocket handling, server connection, threading, all that goodness
	/// </summary>
    static class Core{
		internal static CogitoUI cogitoUI = null;

		#if DEBUG
        internal static WebSocket websocket = new WebSocket("ws://chat.f-list.net:8722"); //8722 Dev, 9722 Real but dev server is down \o/
		#else
        internal static WebSocket websocket = new WebSocket(String.Format("ws://{0}:{1}", Properties.Settings.Default.Host, Properties.Settings.Default.Port));
		#endif

		internal static volatile List<User> users = new List<User>();
        internal static volatile List<Channel> channels = new List<Channel>();
		internal static List<User> globalOps = new List<User>();
		
		private static Queue<FListMessageEventArgs> IncomingMessageQueue = new Queue<FListMessageEventArgs>();
		private static Queue<string> OutgoingMessageQueue = new Queue<string>();
		private static volatile bool _sendForever = true;
		
		private static void SendMessage(Object senderbool)
		{
			if (OutgoingMessageQueue.Count > 0){websocket.Send(OutgoingMessageQueue.Dequeue());}
		}

		private static TimerCallback sendTimerCallback = SendMessage;
		internal static System.Threading.Timer Sender = new System.Threading.Timer(sendTimerCallback, _sendForever, 0, 500); 

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
        static void Main(){
			Application.SetCompatibleTextRenderingDefault(false);
			Application.EnableVisualStyles();
			cogitoUI = new CogitoUI();
			//websocket.OnMessage += (sender, e) => Console.WriteLine("Sender: " + sender.ToString() + "Event: " + e.Data.ToString());//TODO: Insert RawMessage-to-Object-onto-Collections.Queue
			//TODO: IMPLEMENT PROPERLY websocket.OnMessage += OnWebsocketMessage();
			//websocket.OnMessage += OnWebsocketMessage();
			try { Console.WriteLine(Properties.Settings.Default.userAutoComplete.Count); }
			catch (System.NullReferenceException) { Properties.Settings.Default.userAutoComplete = new AutoCompleteStringCollection(); }
			//Sender.Start();
			Application.Run(cogitoUI);
			//Sender.Join();
        }

		private static void OnWebsocketClose()
		{
			websocket.Send("");
		}

		private static EventHandler<MessageEventArgs> OnWebsocketMessage()
		{
			throw new NotImplementedException();
		}
		
		/// <summary> Fetches the corresponding User instance from the program's List of users
		/// </summary>
		/// <param name="username">Username (string) to look for</param>
		/// <returns>User instance</returns>
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
		/// <returns>User instance</returns>
		public static User getUser(User user){return user;}

		/// <summary> Overloaded in order to immediately return Channel instances, as may happen.
		/// </summary>
		/// <param name="channel">Channel instance.</param>
		/// <returns>channel instance</returns>
		public static Channel getChannel(Channel channel) { return channel; }

		/// <summary>
		/// Fetches the corresponding channel instance from the List of all channels registered in CogitoSharp.Core
		/// </summary>
		/// <param name="channel"></param>
		/// <returns>Channel Instance</returns>
		public static Channel getChannel(string channel){
			Channel _channel = Core.channels.Find(x => x.key == channel);
			if (_channel != null) { return _channel; }
			else{
				_channel = new Channel(channel);
				return _channel;
			}
		}

		private delegate void digestRawMessage(string rawmessage);

		static void ProcessMessageOntoQueue(EventHandler<MessageEventArgs> e)
		{
			throw new NotImplementedException("NOPE");
		}

		static void ProcessMessageFromQueue(FListMessageEventArgs msgobj)
		{
			//TODO: get message from queue, Type.InvokeMember on namespace/class FLISTPROCESSING
			//TODO: print how?
			//msgobj.opcode not in ["PRI", "MSG"]
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
			CogitoUI.chatUI.chatTabs.TabPages.Add(this.chanTab);
			Core.channels.Add(this);
			this.CID=Core.channels.Count();
        }

        public Channel(string _key, string _name){
            this.key = _key;
            this.name = _name;
            this.chanTab = new ChatTab(this.name);
            CogitoUI.chatUI.chatTabs.TabPages.Add(this.chanTab);
			Core.channels.Add(this);
			this.CID = Core.channels.Count;
        }

        ~Channel(){
            CogitoUI.chatUI.chatTabs.TabPages.Remove(this.chanTab);
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

    class Message{
		public string opcode { get; set; }
        public Dictionary<string, string> args { get; set; }
		Source? source = null;
		public Message(string rawmessage){
			#if DEBUG
				Console.WriteLine(rawmessage);
			#endif
			string opcode = rawmessage.Substring(0, 3);
			Dictionary<string, string> Message = JsonConvert.DeserializeObject<Dictionary<string, string>>(rawmessage.Substring(3));
			if (Message.ContainsKey("Channel"))
				{
					source = new Source(Core.getUser(Message["User"]), Core.getChannel(Message["Channel"]));
				}

			else{}
			
			}

    }

	/// <summary>
	/// Contains all methods that process FList server/client commands
	/// </summary>
	internal sealed class FListProcessor{
		/// <summary> ACB This command requires chat op or higher. Request a character's account be banned from the server. </summary>
		public static void ACB(Message message){}

		/// <summary> AOP The given character has been promoted to chatop. >> AOP { "character": string } / Promotes user to ChatOP</summary>
		internal static void ADL(Message message){}

		/// <summary> </summary>
		internal static void AOP(Message message){}

		/// <summary> </summary>
		internal static void AWC(Message message){}

		/// <summary> </summary>
		internal static void BRO(Message message){}

		/// <summary> </summary>
		internal static void CBL(Message message){}

		/// <summary> </summary>
		internal static void CBU(Message message){}

		/// <summary> </summary>
		internal static void CCR(Message message){}

		/// <summary> </summary>
		internal static void CDS(Message message){}

		/// <summary> </summary>
		internal static void CHA(Message message){}

		/// <summary> </summary>
		internal static void CIU(Message message){}

		/// <summary> </summary>
		internal static void CKU(Message message){}

		/// <summary> </summary>
		internal static void COA(Message message){}

		/// <summary> </summary>
		internal static void COL(Message message){}

		/// <summary> </summary>
		internal static void CON(Message message){}

		/// <summary> </summary>
		internal static void COR(Message message){}

		/// <summary> </summary>
		internal static void CRC(Message message){}

		/// <summary> </summary>
		internal static void CSO(Message message){}

		/// <summary> </summary>
		internal static void CTU(Message message){}

		/// <summary> </summary>
		internal static void CUB(Message message){}

		/// <summary> </summary>
		internal static void DOP(Message message){}

		/// <summary> </summary>
		internal static void ERR(Message message){}

		/// <summary> </summary>
		internal static void FKS(Message message){}

		/// <summary> </summary>
		internal static void FLN(Message message){}

		/// <summary> </summary>
		internal static void FRL(Message message){}

		/// <summary> </summary>
		internal static void HLO(Message message){}

		/// <summary> </summary>
		internal static void ICH(Message message){}

		/// <summary> </summary>
		internal static void IGN(Message message){}

		/// <summary> </summary>
		internal static void JCH(Message message){}

		/// <summary> </summary>
		internal static void KID(Message message){}

		/// <summary> </summary>
		internal static void KIK(Message message){}

		/// <summary> </summary>
		internal static void KIN(Message message){}

		/// <summary> </summary>
		internal static void LCH(Message message){}

		/// <summary> </summary>
		internal static void LIS(Message message){}

		/// <summary> </summary>
		internal static void LRP(Message message){}

		/// <summary> </summary>
		internal static void MSG(Message message){}

		/// <summary> </summary>
		internal static void NLN(Message message){}

		/// <summary> </summary>
		internal static void ORS(Message message){}

		/// <summary> </summary>
		internal static void PIN(Message message){Core.websocket.Send("PIN");}

		/// <summary> </summary>
		internal static void PRD(Message message){}

		/// <summary> </summary>
		internal static void PRI(Message message){}

		/// <summary> </summary>
		internal static void PRO(Message message){}

		/// <summary> </summary>
		internal static void RLD(Message message){}

		/// <summary> </summary>
		internal static void RLL(Message message){}

		/// <summary> </summary>
		internal static void RMO(Message message){}

		/// <summary> </summary>
		internal static void RST(Message message){}

		/// <summary> </summary>
		internal static void RTB(Message message){}

		/// <summary> </summary>
		internal static void RWD(Message message){}

		/// <summary> </summary>
		internal static void SFC(Message message){}

		/// <summary> </summary>
		internal static void STA(Message message){}

		/// <summary> </summary>
		internal static void SYS(Message message){}

		/// <summary> </summary>
		internal static void TMO(Message message){}

		/// <summary> </summary>
		internal static void TPN(Message message){}

		/// <summary> </summary>
		internal static void UBN(Message message){}

		/// <summary> </summary>
		internal static void UPT(Message message){}

		/// <summary> </summary>
		internal static void VAR(Message message){}
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
