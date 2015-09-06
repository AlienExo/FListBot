﻿using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Speech.Synthesis;
using System.Security;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Web;

using WebSocketSharp;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using CogitoSharp.IO;

#region changelog
/* Changelog
 * v0.8.0.1
 *		Started bothering to write a changelog. Login works, character select works, UI opens as it should aaand... that's about it. I really need to work on getting
 *		a GUI for, yanno, actually joinin a channel. Then adding that  channel as a tab. etc. etc. etc....
 *		
 * v0.8.0.2
 *		Replaced ChatUserList with a custom control in order to draw gender-specific colors.
 *			Consider: Filtering, implement how? Most important categories are isInteresting (put a symbol there!), Gender, Age, Orientation since those
 *			are the most crucial factors for compatibility. But they have to be fetched from the server.
 *		Raw data dumping for everyony
 *		Overhauled Avatar image loading and saving; tested. Works well.
 */


#endregion

/*	--- Ideas ---
 * CASIE - shows profile of the other player
 *	Calculates compatibility %
 *		nKinks = number of kinks both players share
 *		foreach Kink:
 *			Fav - 1, Yes - 0.75, Maybe - 0.5, No - -1
 *			pct = ([KinkCatA] - [KinkCatB]) * KinkCatA
 *			compatibility -= (pct/nKinks)
 *		(See: CASIE.xlxs)
 *		
 * AdCreator and Poster
 *	Rich Text Editor; replace format codes with BBCode.
 *	Select channels (have a set for fav channels?)
 *	Post ad to all selected channels
 * 
 *	VIP List, just below real bookmarks/friends
 *	
 * Not interested  list, bottom of user list
 *  * Bookmarks
 *  * Interested
 *  * Other Users
 *  * Not Interested
 *  * [Ignored]
 */

/*	--- TODO  ---
 * HACK Send raw AOP w/o op status. 
 * HACK Send login for a different character w/ valid ticket for another
 * 
 * [||||||||||] DONE: I have no idea what I'm doing 
 * [||||||||||] DONE: Start off by implementing some way of getting your API Key (getKey())
 * [||||||||||] DONE: Split Login and actual interface into two things > this.hide(), open login form, if login succeeds and character is chosen, this.show()
 * 
 * [||||||||||] TODO: The advanced login options should probably be processed befoooooore opening the ws://
 * [|||||||---] 2* Get the flipping interface to run 'IN PARALLEL' with your other code. queue and events! Or THREADIIIIING
 * 
 * [|||||||||-] 3 Connect to test server (8722), use Type.InvokeMember and a static class to get delicious command parsing in a loop/event drived
 * 
 * [||||||||||] Consider proper delegates for this thing
 * 
 * [----------] 4 Port the rest of the API/Scraping
 * 
 * [|||||-----] 5 Implement logic so the UI Components resize when the window does. 
 *		1/4 for the user list, all down the side. Three lines of text for the entry point, to the bottom. Rest TabPage.
 * 
 * [----------] Status bar at the top of the program for multi-character chats
 * [----------] Tiny profile view of people you're talking to, on the side. Web browser, or scraping. 
 * [----------]		shows age, gender, name, avatar
 * [----------]		small info area with quickmatch - fave matche, no match, your faves in their no, your no in their fav
 * [----------]		Customizable filters - value (e.g. Age) from combobox, operator from a combobox, then a relevant value in the next box
 * [----------]		detach? new window?
 * 
 * [----------]		Custom highlight triggers (array string[])
 * [----------] TODO: Log formatting and output. As .txt or .html, activate/deacticate images (checkbox)
 * [----------] TODO: Friends list: "Friends with <Your Character>".
 *
 * [----------] TODO: Login stagger / reconnect on shit connection
 * [----------] TODO: User memos, displayed as tooltips on mouse-over in the UserList and in CASIE. HI VALORIN.
 *									
 * TODO: A Autocorrection as BLOSUM Like matrix of likelihood of typos, e.g. high for z and y, low for t and [Space] etc.
 * [||||||||||] DONE: Settings panel on login form to set host and port
 * 
 * Chat UI - when Single-User channel (/priv), replace UserList with CASIE window?
 * http://www.f-list.net/json/api/kink-list.php
 * 
 * [----------] TODO: write test for Avatar getting; compare... idk, bitmap hexcode or whatever to ensure a known character returns the right avatar.
 * 
*/

namespace CogitoSharp
{
	/// <summary>May be implemented for proper JSON serialization.</summary>
	public interface ILoginKey{
		/// <summary> Error message, if authentication failed</summary>
		string error { get; set; }
		/// <summary> The string which allows access to the fserv systems; returned on successful authentication with account and password</summary>
		string ticket { get; set; }
	}

	/// <summary> Collection of all values and variables needed to establish a connection to an fchat server </summary>
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

		/// <summary> The DateTime when this ticket was obtained; used to avoid </summary>
		public DateTime ticketTaken = DateTime.UtcNow;
	}

	/// <summary>Stub class for JSON Deserialization</summary>
	public class LoginKeyConverter : Newtonsoft.Json.Converters.CustomCreationConverter<ILoginKey>{
		public override ILoginKey Create(Type objectType){
			return new LoginKey();
		}
	}

	class Account{
		protected internal static string account;
		protected internal static LoginKey LoginKey = null;
		protected internal static List<string> bookmarks = new List<string>();

		protected internal static void getTicket(string _account, string _password){
			using (var wb = new WebClient()){
				var data = new NameValueCollection();
				data["account"] = _account;
				data["password"] = _password.ToString();
				var byteTicket = wb.UploadValues("https://www.f-list.net/json/getApiTicket.php", "POST", data);
				string t1 = System.Text.Encoding.ASCII.GetString(byteTicket);
				Account.LoginKey = JsonConvert.DeserializeObject<LoginKey>(t1, new LoginKeyConverter());
				Account.LoginKey.ticketTaken = DateTime.UtcNow;
			}
		}

		protected internal static bool login(string _account, string _password, out string error){
			getTicket(_account, _password);
			if (LoginKey.error.Length > 0) {
				error = LoginKey.error;
				Core.SystemLog.Log("Logging in with account " + _account + " failed: " + error);
				return false;}
			else {
				Core.SystemLog.Log("Successfully obtained login ticket " + Account.LoginKey.ticket + " for account " + _account);
				Account.account = _account;
				error = "";
				foreach (Dictionary<string, string> d in LoginKey.bookmarks){
					foreach (KeyValuePair<string, string> kv in d){Account.bookmarks.Add(kv.Value);}
				}
				Account.bookmarks.Sort();
				LoginKey.bookmarks.Clear();
				return true;
			}
		}

		protected internal static void characterSelect(string character){
			if(LoginKey.ticket.Length<=0){throw new ArgumentException("No valid login ticket/API Key is present.");}
			var logindata = new Dictionary<string, string>();
			logindata["method"] = "ticket";
			logindata["account"] = Account.account;
			logindata["character"] = character;
			logindata["ticket"] = Account.LoginKey.ticket;
			logindata["cname"] = "COGITO";
			logindata["cversion"] = Application.ProductVersion;
			string openString = JsonConvert.SerializeObject(logindata);
			openString = "IDN " + openString;
			#if DEBUG
			Console.WriteLine("Open String: " + openString);
			#endif
			Core.websocket.OnOpen += (sender, e) => Core.websocket.Send(openString);
			Core.websocket.OnOpen += (sender, e) => Core.SystemLog.Log("Opened Websocket.");
			Core.websocket.OnOpen += (sender, e) => Thread.Sleep(Math.Max(1000, IO.Message.chat_flood)); 
			//Core.websocket.OnOpen += (sender, e) => Core.websocket.Send("ORS");
			new SystemCommand("ORS").send();
			new SystemCommand("CHA").send();
			new SystemCommand("STA { \"status\": \"online\", \"statusmsg\": \"Running Cogito# v" + Application.ProductVersion + "\" }").send();
			Core.SystemLog.Log("Logging in with selected character '" + character + "'.");
		}
	}

	/// <summary>Websocket handling, server connection, threading, all that goodness</summary>
    internal static class Core{
		#if DEBUG
			internal static WebSocket websocket = new WebSocket("ws://chat.f-list.net:8722"); //8722 Dev, 9722 Real but dev server is down \o/
		#else
			internal static WebSocket websocket = new WebSocket(String.Format("ws://{0}:{1}", Properties.Settings.Default.Host, Properties.Settings.Default.Port));
		#endif
		internal static volatile HashSet<User> allGlobalUsers = new HashSet<User>();
        internal static volatile HashSet<Channel> channels = new HashSet<Channel>();
		internal static List<User> globalOps = new List<User>();
		
		private static Queue<CogitoSharp.IO.Message> IncomingMessageQueue = new Queue<CogitoSharp.IO.Message>();
		internal static Queue<SystemCommand> OutgoingMessageQueue = new Queue<SystemCommand>();
		private static volatile bool _sendForever = true;
		
		private static void SendMessageFromQueue(Object senderbool){ if (OutgoingMessageQueue.Count > 0){websocket.Send(OutgoingMessageQueue.Dequeue().ToServerString());} }

		private static TimerCallback sendTimerCallback = SendMessageFromQueue;
		internal static System.Threading.Timer EternalSender = new System.Threading.Timer(sendTimerCallback, _sendForever, System.Threading.Timeout.Infinite, IO.Message.chat_flood);

		internal static IO.Logging.LogFile SystemLog = new IO.Logging.LogFile("System Log");
		internal static IO.Logging.LogFile ErrorLog = new IO.Logging.LogFile("Error Log");
		#if DEBUG
		internal static IO.Logging.LogFile RawData = new IO.Logging.LogFile("RawData");
		#endif
		internal static CogitoUI cogitoUI = null;
		internal static SpeechSynthesizer SYBIL = new SpeechSynthesizer();

		internal static User OwnUser = null;

		//internal static ThreadStart EternalSendMethod = SendMessage;
		//internal static Thread EternalSender = new Thread();

		/// <summary>The main entry point for the application.</summary>
		[STAThread]
        static void Main(){
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			AppDomain.CurrentDomain.ProcessExit += OnProcessExit;
			cogitoUI = new CogitoUI();
			SystemLog.Log("Start up: CogitoSharp v." + Application.ProductVersion);
			SystemLog.Log("Loading Plugins...");
			try{ Plugins.loadPlugins(); }
			catch (Exception e) { 
				Console.WriteLine(e.InnerException.ToString());
				SystemLog.Log(e.InnerException.ToString());
				throw; 
			}
			SystemLog.Log("Loading User Database...");
			try{
				Stream s = File.OpenRead(Path.Combine(Config.AppSettings.DataPath, Config.AppSettings.UserFileName));
				BinaryFormatter bf = new BinaryFormatter();
				Core.allGlobalUsers = (HashSet<User>)bf.Deserialize(s);
				s.Close();
			}
			catch (FileNotFoundException) { } //Do Nothing ¯\_(ツ)_/¯
			catch (DirectoryNotFoundException) { Directory.CreateDirectory(Config.AppSettings.DataPath); }
			catch (UnauthorizedAccessException) { 
				SystemLog.Log("Incapable of accessing user database directory");
				MessageBox.Show("Warning: Application is unable to access its user database in " + Config.AppSettings.DataPath + 
				"'. Please ensure all proper permissions exist. Application may be unable to persist user database, leading to increased bandwith usage.", "Unable to load user database");
			 }
			 foreach (User u in Core.allGlobalUsers) { if ((u.dataTakenOn - DateTime.Now) >= Config.AppSettings.userProfileRefreshPeriod) { u.GetProfileInfo(); } } //Data is old, refresh it.
			SYBIL.SelectVoiceByHints(VoiceGender.Female, VoiceAge.Adult);
			SYBIL.Rate = 7;
			try{ Properties.Settings.Default.userAutoComplete.GetType(); }
			catch (NullReferenceException){ 
				Properties.Settings.Default.userAutoComplete = new AutoCompleteStringCollection();
				Properties.Settings.Default.Save();
			 }
			Application.Run(cogitoUI);
        }

		static void OnProcessExit(object sender, EventArgs e){
			using (Stream fs = File.Create(Config.AppSettings.DataPath + "UserData.dat"))
			{
				BinaryFormatter bf = new BinaryFormatter();
				bf.Serialize(fs, Core.allGlobalUsers);
				fs.Flush();
			}
			if (Core.websocket.IsAlive) { Core.websocket.Close(); }
		}

		private static void OnWebsocketClose(){
			SystemLog.Log("Closing connection...");
			//websocket.Send("");
		}

		internal static void OnWebsocketMessage(Object sender, WebSocketSharp.MessageEventArgs e){
			SystemCommand c = new SystemCommand(e.Data);
			#if DEBUG
				RawData.Log(c.ToServerString());
			#endif
			try{ typeof(FListProcessor).GetMethod(c.opcode, BindingFlags.NonPublic | BindingFlags.Static).Invoke(c, new Object[] {c}); }
			catch (Exception FuckUp) { ErrorLog.Log(String.Format("Invocation of Method {0} failed:\n\t{1}\n\t{2}", c.opcode, FuckUp.Message, FuckUp.InnerException)); }
		}
		
		/// <summary> Fetches the corresponding User instance from the program's users database; creates (and registers) and returns a new one if no match is found.
		/// </summary>
		/// <param name="username">Username (string) to look for</param>
		/// <returns>User instance</returns>
		public static User getUser(string username){
			return Core.allGlobalUsers.Count(x => x.Name == username) > 0 ? Core.allGlobalUsers.First<User>(n => n.Name == username) : new User(username);
		}
		
		///// <summary> Overloaded in order to immediately return User instances, as may happen...?
		///// </summary>
		///// <param name="user">User instance.</param>
		///// <returns>User instance</returns>
		//public static User getUser(User user) { return user; }

		///// <summary> Overloaded in order to immediately return Channel instances, as may happen...?
		///// </summary>
		///// <param name="channel">Channel instance.</param>
		///// <returns>channel instance</returns>
		//public static Channel getChannel(Channel channel) { return channel; }

		/// <summary>
		/// Fetches the corresponding channel instance from the List of all channels registered in CogitoSharp.Core; creates a new one (adding it to the central register) if no match is found.
		/// </summary>
		/// <param name="channel"></param>
		/// <returns>Channel Instance</returns>
		public static Channel getChannel(string channel){
			return Core.channels.Count(x => x.key == channel) > 0 ? Core.channels.First<Channel>(n => n.key == channel) : new Channel(channel);
		}
	}
}
