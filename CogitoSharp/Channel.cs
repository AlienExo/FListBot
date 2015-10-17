using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CogitoSharp
{
	internal enum ChannelMode : int { chat = 0, ads, both }

	/// <summary>Channel class, for both public and private channels</summary>
	[Serializable]
	internal class Channel : IComparable, IDisposable
	{
		/// <summary>Channel ID Number</summary>
		[NonSerialized] private static int Count = 1;
		[NonSerialized] private readonly int CID = ++Channel.Count;
		[NonSerialized] private bool disposed = false;
		[NonSerialized] internal bool isJoined = false;

		/// <summary> Any characters that join below this age will cause a Mod to be alerted. When set to 0, it's off.</summary>
		internal int alertMinAge = 0;
		/// <summary> When true, also alerts Mods when a character whose age cannot be parsed joins. </summary>
		internal bool alertNoAge = false;
		
		internal bool isFavorite = false;
		
		/// <summary>Keys are the UUID for private channels; channel title for normal. Always use .key for channel-specific commands.</summary>
		[NonSerialized] internal string _key;

		internal string Key
		{
			get { return this._key ?? this.Name; }
			set { this._key = value;}
		}

		/// <summary>Channel name, in human-readable format</summary>
		internal string Name;


		/// <summary> Channel mode - chat only, ads only, both. </summary>
		internal ChannelMode mode = ChannelMode.both;

		/// <summary>Associated TabPage for this channel</summary>
		[NonSerialized] internal ChatTab chanTab;

		/// <summary>Minimum age to be in this channel. If set to a value greater than 0, the bot will attempt to kick everyone below this age.</summary>
		internal Int16 minAge = 0;

		/// <summary> EXPERIMENTAL - contains the last fragment for Autocompletion </summary>
		[NonSerialized] internal string lastSearchFragment = "";

		[NonSerialized] internal HashSet<User> Mods = new HashSet<User>();

		[NonSerialized] internal HashSet<User> Users = new HashSet<User>();

		[NonSerialized] internal CogitoSharp.IO.Logging.LogFile ChannelLog = null;
		
		/// <summary>
		/// Implementation of IDispose - removes tab page and disposes of Log to ensure buffer is flushed
		/// </summary>
		public void Dispose(){
			if (!disposed){
				if (this.chanTab != null) { this.chanTab.Dispose(); }
				if (this.ChannelLog != null) { this.ChannelLog.Dispose(); }
			}
			this.disposed = true;
		}

		internal void Join() { CogitoUI.chatUI.chatTabs.channelJoined(this); }
		internal void Leave(){ CogitoUI.chatUI.chatTabs.channelLeft(this); }

		/// <summary>
		/// Constructor, used with Public (e.g. name-only) channels
		/// </summary>
		/// <param name="_name">The channel's name</param>
		public Channel(string _name){
			this.CID = Core.channels.Count;
			this.Key = null;
			this.Name = _name;
			this.chanTab = new ChatTab(this);
			//this.chanTab.Name = this.Name;
			this.chanTab.Text = this.Name;
			Core.channels.Add(this);
		}

		/// <summary>
		/// Constructor for private channel, used on Invite or after ORS
		/// </summary>
		/// <param name="_key">The channel's UUID, used to join it</param>
		/// <param name="_name">The channel's name</param>
		public Channel(string _key, string _name = "[Private Channel]") : this(_name){ this.Key = _key; }

		internal void MessageReceived(CogitoSharp.IO.Message m){
			string _m = m.ToString();
			this.ChannelLog.LogRaw(_m);
			int lastfullmessage = Array.FindIndex<string>(this.chanTab.messageBuffer, n => n == null);
			if (lastfullmessage == Config.AppSettings.MessageBufferSize - 1) { Array.Copy(this.chanTab.messageBuffer, 1, this.chanTab.messageBuffer, 0, this.chanTab.messageBuffer.Length - 1); }
			this.chanTab.messageBuffer[lastfullmessage + 1] = m.ToString();
			CogitoUI.chatUI.chatTabs.ensureVisible(this.chanTab);
			//this.chanTab.Flash(); //TODO: Flash tab			
		}

		internal void MessageReceived(string s){
			string _s = String.Format("<{0}> -- {1}{2}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), s, Environment.NewLine);
			this.ChannelLog.LogRaw(_s);
			if (this.chanTab.messageBuffer.Length == Config.AppSettings.MessageBufferSize) { Array.Copy(this.chanTab.messageBuffer, 1, this.chanTab.messageBuffer, 0, this.chanTab.messageBuffer.Length - 1); }
			this.chanTab.messageBuffer[this.chanTab.messageBuffer.Length + 1] = _s;
			CogitoUI.chatUI.chatTabs.ensureVisible(this.chanTab);
			//this.chanTab.Flash(); //TODO: Flash tab			
		}

		/// <summary>Generic destrutor, closes associated TabPage</summary>
		~Channel()
		{
			CogitoUI.chatUI.chatTabs.ensureNotVisible(this.chanTab);
			Dispose();
		}

		//TODO Is this really how you want it?
		public override string ToString(){
			return this._key == null ? String.Format("Public Channel '{0}'", this.Name) : String.Format("Private Channel '{0}' ({1})", this.Name, this.Key);
		}

		public override bool Equals(object obj){
			if (obj == null) { return false; }
			Channel c = obj as Channel;
			if (this.Key == c.Key) { return true; }
			//Two channels can have the same name, but never the same key.
			else { return false; }
		}

		public override int GetHashCode(){ return this.CID; }

		public bool Equals(Channel channel){
			if (this.Name == channel.Name) { return true; }
			else { return false; }
		}

		public int CompareTo(object obj){
			if (obj == null) { return 1; }
			Channel c = obj as Channel;
			if (c != null) { return this.Key.CompareTo(c.Key); }
			else { throw new ArgumentException("Object cannot be made into Channel. Cannot CompareTo()."); }

		}

		public void Log(string s){ this.ChannelLog.Log(s); }
	}
}
