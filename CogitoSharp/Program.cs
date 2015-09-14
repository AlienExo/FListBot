using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Collections.Concurrent;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;
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
 * 
 *  !!!! new IO.SystemCommand("XXX").Send(); causes the "The header part of the frame cannot be read from the data source" error! ...HOW?!
 * 
 * HACK Send raw AOP w/o op status. 
 * HACK Send login for a different character w/ valid ticket for another
 * HACK are channel keys just ADH-[md5 channel name] or some other hash? test it.
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
	/// <summary>Implemented for proper JSON serialization.</summary>
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
			//Core.websocket.OnOpen += (sender, e) => Core.websocket.Send(openString);
			//Core.websocket.OnOpen += (sender, e) => Core.websocket.Send("ORS");
			Core.SystemLog.Log("Logging in with selected character '" + character + "'.");
			Core.websocket.Connect();
			Core.websocket.Send(openString);
		}
	}

	[Serializable]
	public class ConcurrentSet<T> : IEnumerable<T>, ISet<T>, ICollection<T>
	{
		private readonly ConcurrentDictionary<T, byte> _dictionary = new ConcurrentDictionary<T, byte>();

		/// <summary> Returns an enumerator that iterates through the collection. </summary>
		/// <returns> A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection. </returns>
		public IEnumerator<T> GetEnumerator(){ return _dictionary.Keys.GetEnumerator(); }

		/// <summary> Returns an enumerator that iterates through a collection. </summary>
		/// <returns> An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection. </returns>
		IEnumerator IEnumerable.GetEnumerator(){ return GetEnumerator(); }

		/// <summary> Removes the first occurrence of a specific object from the <see cref="T:System.Collections.Generic.ICollection`1"/>. </summary>
		/// <returns> true if <paramref name="item"/> was successfully removed from the <see cref="T:System.Collections.Generic.ICollection`1"/>; otherwise, false. This method also returns false if <paramref name="item"/> is not found in the original <see cref="T:System.Collections.Generic.ICollection`1"/>. </returns>
		/// <param name="item">The object to remove from the <see cref="T:System.Collections.Generic.ICollection`1"/>.</param><exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.</exception>
		public bool Remove(T item){ return TryRemove(item); }

		/// <summary>Gets the number of elements in the set. </summary>
		public int Count{ get { return _dictionary.Count; } }
		
		/// <summary> Gets a value indicating whether the <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only. </summary>
		/// <returns> true if the <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only; otherwise, false. </returns>
		public bool IsReadOnly { get { return false; } }

		/// <summary> Gets a value that indicates if the set is empty. </summary>
		public bool IsEmpty{ get { return _dictionary.IsEmpty; } }

		public ICollection<T> Values{ get { return _dictionary.Keys; } }

		/// <summary>Adds an item to the <see cref="T:System.Collections.Generic.ICollection`1"/>. </summary>
		/// <param name="item">The object to add to the <see cref="T:System.Collections.Generic.ICollection`1"/>.</param>
		/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.</exception>
		void ICollection<T>.Add(T item){
			if (!Add(item))
				throw new ArgumentException("Item already exists in set.");
		}

		/// <summary> Modifies the current set so that it contains all elements that are present in both the current set and in the specified collection. </summary>
		/// <param name="other">The collection to compare to the current set.</param><exception cref="T:System.ArgumentNullException"><paramref name="other"/> is null.</exception>
		public void UnionWith(IEnumerable<T> other){
			foreach (var item in other)
				TryAdd(item);
		}

		/// <summary>Modifies the current set so that it contains only elements that are also in a specified collection.</summary>
		/// <param name="other">The collection to compare to the current set.</param><exception cref="T:System.ArgumentNullException"><paramref name="other"/> is null.</exception>
		public void IntersectWith(IEnumerable<T> other){
			var enumerable = other as IList<T> ?? other.ToArray();
			foreach (var item in this)
			{
				if (!enumerable.Contains(item))
					TryRemove(item);
			}
		}

		/// <summary>Removes all elements in the specified collection from the current set. </summary>
		/// <param name="other">The collection of items to remove from the set.</param><exception cref="T:System.ArgumentNullException"><paramref name="other"/> is null.</exception>
		public void ExceptWith(IEnumerable<T> other){
			foreach (var item in other)
				TryRemove(item);
		}

		/// <summary>Modifies the current set so that it contains only elements that are present either in the current set or in the specified collection, but not both.  </summary>
		/// <param name="other">The collection to compare to the current set.</param><exception cref="T:System.ArgumentNullException"><paramref name="other"/> is null.</exception>
		public void SymmetricExceptWith(IEnumerable<T> other){
			throw new NotImplementedException();
		}

		/// <summary>Determines whether a set is a subset of a specified collection. </summary>
		/// <returns> true if the current set is a subset of <paramref name="other"/>; otherwise, false.</returns>
		/// <param name="other">The collection to compare to the current set.</param><exception cref="T:System.ArgumentNullException"><paramref name="other"/> is null.</exception>
		public bool IsSubsetOf(IEnumerable<T> other){
			var enumerable = other as IList<T> ?? other.ToArray();
			return this.AsParallel().All(enumerable.Contains);
		}

		/// <summary>Determines whether the current set is a superset of a specified collection.</summary>
		/// <returns>true if the current set is a superset of <paramref name="other"/>; otherwise, false.</returns>
		/// <param name="other">The collection to compare to the current set.</param><exception cref="T:System.ArgumentNullException"><paramref name="other"/> is null.</exception>
		public bool IsSupersetOf(IEnumerable<T> other){
			return other.AsParallel().All(Contains);
		}

		/// <summary>Determines whether the current set is a correct superset of a specified collection.</summary>
		/// <returns> true if the <see cref="T:System.Collections.Generic.ISet`1"/> object is a correct superset of <paramref name="other"/>; otherwise, false.</returns>
		/// <param name="other">The collection to compare to the current set. </param><exception cref="T:System.ArgumentNullException"><paramref name="other"/> is null.</exception>
		public bool IsProperSupersetOf(IEnumerable<T> other){
			var enumerable = other as IList<T> ?? other.ToArray();
			return this.Count != enumerable.Count && IsSupersetOf(enumerable);
		}

		/// <summary>Determines whether the current set is a property (strict) subset of a specified collection.</summary>
		/// <returns>true if the current set is a correct subset of <paramref name="other"/>; otherwise, false.</returns>
		/// <param name="other">The collection to compare to the current set.</param><exception cref="T:System.ArgumentNullException"><paramref name="other"/> is null.</exception>
		public bool IsProperSubsetOf(IEnumerable<T> other){
			var enumerable = other as IList<T> ?? other.ToArray();
			return Count != enumerable.Count && IsSubsetOf(enumerable);
		}

		/// <summary>Determines whether the current set overlaps with the specified collection.</summary>
		/// <returns>true if the current set and <paramref name="other"/> share at least one common element; otherwise, false.</returns>
		/// <param name="other">The collection to compare to the current set.</param><exception cref="T:System.ArgumentNullException"><paramref name="other"/> is null.</exception>
		public bool Overlaps(IEnumerable<T> other){
			return other.AsParallel().Any(Contains);
		}

		/// <summary>Determines whether the current set and the specified collection contain the same elements.</summary>
		/// <returns>true if the current set is equal to <paramref name="other"/>; otherwise, false.</returns>
		/// <param name="other">The collection to compare to the current set.</param><exception cref="T:System.ArgumentNullException"><paramref name="other"/> is null.</exception>
		public bool SetEquals(IEnumerable<T> other){
			var enumerable = other as IList<T> ?? other.ToArray();
			return Count == enumerable.Count && enumerable.AsParallel().All(Contains);
		}

		/// <summary>Adds an element to the current set and returns a value to indicate if the element was successfully added. </summary>
		/// <returns>true if the element is added to the set; false if the element is already in the set.</returns>
		/// <param name="item">The element to add to the set.</param>
		public bool Add(T item){
			return TryAdd(item);
		}

		public void Clear(){
			_dictionary.Clear();
		}

		public bool Contains(T item){
			return _dictionary.ContainsKey(item);
		}

		/// <summary>Copies the elements of the <see cref="T:System.Collections.Generic.ICollection`1"/> to an <see cref="T:System.Array"/>, starting at a particular <see cref="T:System.Array"/> index.</summary>
		/// <param name="array">The one-dimensional <see cref="T:System.Array"/> that is the destination of the elements copied from <see cref="T:System.Collections.Generic.ICollection`1"/>. The <see cref="T:System.Array"/> must have zero-based indexing.</param><param name="arrayIndex">The zero-based index in <paramref name="array"/> at which copying begins.</param><exception cref="T:System.ArgumentNullException"><paramref name="array"/> is null.</exception><exception cref="T:System.ArgumentOutOfRangeException"><paramref name="arrayIndex"/> is less than 0.</exception><exception cref="T:System.ArgumentException"><paramref name="array"/> is multidimensional.-or-The number of elements in the source <see cref="T:System.Collections.Generic.ICollection`1"/> is greater than the available space from <paramref name="arrayIndex"/> to the end of the destination <paramref name="array"/>.-or-Type <paramref name="T"/> cannot be cast automatically to the type of the destination <paramref name="array"/>.</exception>
		public void CopyTo(T[] array, int arrayIndex){
			Values.CopyTo(array, arrayIndex);
		}

		public T[] ToArray(){ return _dictionary.Keys.ToArray(); }

		public bool TryAdd(T item){ return _dictionary.TryAdd(item, default(byte)); }

		public bool TryRemove(T item){ 
			byte donotcare;
			return _dictionary.TryRemove(item, out donotcare);
		}
	}

	/// <summary>Websocket handling, server connection, threading, all that goodness</summary>
    internal static class Core{

		internal static WebSocket websocket = null;
		internal static volatile ConcurrentSet<User> allGlobalUsers = new ConcurrentSet<User>();
		internal static volatile ConcurrentSet<Channel> channels = new ConcurrentSet<Channel>();
		internal static List<User> globalOps = new List<User>();
		internal static Bitmap DefaultAvatar = null;


		internal static Queue<IO.SystemCommand> IncomingMessageQueue = new Queue<IO.SystemCommand>();
		internal static Queue<IO.SystemCommand> OutgoingMessageQueue = new Queue<IO.SystemCommand>();
				
		//internal static void SendMessageFromQueue(object sender, EventArgs e){ 
		internal static void SendMessageFromQueue(object stateobject){ 
			try{
				//Console.WriteLine("SendMessageFromQueue called");
				if (OutgoingMessageQueue.Count > 0) { //&& websocket.IsAlive){
					string _message = OutgoingMessageQueue.Dequeue().ToServerString();
					//#if DEBUG
						Core.RawData.Log(">> " + _message);
						Console.WriteLine("Sending message " + _message);
					//#endif
						websocket.Send(_message);
				} 
			}
			catch (Exception ex){
				Core.ErrorLog.Log(String.Format("Sending message failed:\n\t{0}\n\t{1}\t{2}", ex.Message, ex.InnerException, ex.StackTrace));
			}

		}

		//private static TimerCallback sendTimerCallback = SendMessageFromQueue;
		internal static bool _sendForever = true;
		//internal static System.Threading.Timer EternalSender = new System.Threading.Timer(sendTimerCallback, _sendForever, System.Threading.Timeout.Infinite, IO.Message.chat_flood);
		
		internal static IO.Logging.LogFile SystemLog = new IO.Logging.LogFile("System Log");
		internal static IO.Logging.LogFile ErrorLog = new IO.Logging.LogFile("Error Log");
		//#if DEBUG
		internal static IO.Logging.LogFile RawData = new IO.Logging.LogFile("RawData");
		//#endif
		internal static CogitoUI cogitoUI = null;

		internal static User OwnUser = null;

		/// <summary>The main entry point for the application.</summary>
		[STAThread]
        static void Main(){
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			cogitoUI = new CogitoUI();
			AppDomain.CurrentDomain.ProcessExit += new EventHandler(OnProcessExit);
			#if DEBUG
			websocket = new WebSocket("ws://chat.f-list.net:8722"); //8722 Dev, 9722 Real but dev server is down \o/
			#else
			websocket = new WebSocket(String.Format("ws://{0}:{1}", Properties.Settings.Default.Host, Properties.Settings.Default.Port));
			#endif
			websocket.OnMessage += Core.OnWebsocketMessage;
			websocket.OnError += Core.OnWebsocketError;

			DefaultAvatar = new Bitmap(Config.AppSettings.DefaultAvatarFile);
			
			SystemLog.Log("Start up: CogitoSharp v." + Application.ProductVersion);

			SystemLog.Log("Loading Plugins...");
			try{ Plugins.loadPlugins(); }
			catch (Exception e) { 
				Console.WriteLine(e.InnerException.ToString());
				SystemLog.Log(e.InnerException.ToString());
				throw; 
			}

			Core.allGlobalUsers = DeserializeDatabase<User>(Config.AppSettings.UserDBFileName);
			foreach (User u in Core.allGlobalUsers) { if ((u.dataTakenOn - DateTime.Now) >= Config.AppSettings.userProfileRefreshPeriod) { u.GetProfileInfo(); } } //Data is old, refresh it.

			Core.channels = DeserializeDatabase<Channel>(Config.AppSettings.ChannelDBFileName);

			try{ Properties.Settings.Default.userAutoComplete.GetType(); }
			catch (NullReferenceException){ 
				Properties.Settings.Default.userAutoComplete = new AutoCompleteStringCollection();
				Properties.Settings.Default.Save();
			}
			Utils.StringManipulation.Chunk("1234567890", 3, false);
			Application.Run(cogitoUI);
        }

		/// <summary>
		/// Function to Deserialize a ConcurrentSet T instance from BinarySerializer-produced files.
		/// </summary>
		/// <typeparam name="T">The inner type for the ConcurrentSet to deserialize, e.g. ConcurrentSet User</typeparam>
		/// <param name="TargetObject">The object into which the deserialized data is transmitted.</param>
		/// <param name="DataBaseFileName">The name of the BinaryFormatted database file to be deserialized. Expects a List T.</param>
		/// <param name="ContainingFolder">Leave optional (null) to load from Config.AppSettings.DataPath (/data/); else, supply full path to containing folder</param>
		/// <exception cref="System.ArgumentException">Thrown when the TargetObject's type and the data inside the file do not match.</exception>
		/// <exception cref=""></exception>
		private static ConcurrentSet<T> DeserializeDatabase<T>(string DataBaseFileName, string ContainingFolder = null)
		{
			SystemLog.Log("Loading " + typeof(T).Name + " Database...");
			ConcurrentSet<T> TargetObject = new ConcurrentSet<T>();
			try
			{
				ContainingFolder = ContainingFolder ?? Config.AppSettings.DataPath;
				Stream s = File.OpenRead(Path.Combine(ContainingFolder, DataBaseFileName));
				BinaryFormatter bf = new BinaryFormatter();
				try { TargetObject = (ConcurrentSet<T>)bf.Deserialize(s); }
				catch (System.Runtime.Serialization.SerializationException ex) { 
					Core.ErrorLog.Log(String.Format("Could not deserialize database: {0} {1}", ex.Message, ex.StackTrace));
					TargetObject = new ConcurrentSet<T>();
					}
				catch (Exception e) { Core.ErrorLog.Log(String.Format("Error whilst deserializing database: {0} {1}", e.Message, e.StackTrace)); }
				s.Close();
				SystemLog.Log("Deserialized " + typeof(T).Name + " Database and loaded " + ((ConcurrentSet<T>)TargetObject).Count() + " entries.");
			}
			catch (FileNotFoundException) { } //Do Nothing ¯\_(ツ)_/¯
			catch (DirectoryNotFoundException) { Directory.CreateDirectory(Config.AppSettings.DataPath); }
			catch (UnauthorizedAccessException)
			{
				SystemLog.Log("Incapable of accessing user database directory");
				MessageBox.Show("Warning: Application is unable to access its user database in " + Config.AppSettings.DataPath +
				"'. Please ensure all proper permissions exist. Application may be unable to persist user database, leading to increased bandwith usage.", "Unable to load user database");
			}
			return TargetObject;
		}

		static void OnProcessExit(object sender, EventArgs e){
			using (Stream fs = File.Create(Config.AppSettings.DataPath + Config.AppSettings.UserDBFileName)){
				BinaryFormatter bf = new BinaryFormatter();
				bf.Serialize(fs, Core.allGlobalUsers);
				fs.Flush();
			}

			using (Stream fs = File.Create(Config.AppSettings.DataPath + Config.AppSettings.ChannelDBFileName)){
				BinaryFormatter bf = new BinaryFormatter();
				bf.Serialize(fs, Core.channels);
				fs.Flush();
			}
			if (Core.websocket.IsAlive) { Core.websocket.Close(); }
		}

		private static void OnWebsocketClose(){
			SystemLog.Log("Closing connection...");
			//websocket.Send("");
		}

		internal static void OnWebsocketError(Object sender, WebSocketSharp.ErrorEventArgs e){
			ErrorLog.Log(String.Format("Error in Websocket: {0} {1}", e.Message, e.Exception));
		}

		internal static void OnWebsocketMessage(Object sender, WebSocketSharp.MessageEventArgs e){
			SystemCommand c = new SystemCommand(e.Data);
			//#if DEBUG
			if (!new string[]{"LIS", "NLN", "STA", "ORS"}.Contains(c.OpCode)){ RawData.Log(c.ToServerString()); }
			//#endif
			IncomingMessageQueue.Enqueue(c);
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
