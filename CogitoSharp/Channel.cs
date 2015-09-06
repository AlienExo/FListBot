using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CogitoSharp
{
	/// <summary>Channel class, for both public and private channels</summary>
	internal class Channel : IComparable, IDisposable
	{
		/// <summary>Channel ID Number</summary>
		private static int Count;
		private readonly int CID = ++Channel.Count;
		private bool disposed = false;
		
		/// <summary>Keys are the UUID for private channels; channel title for normal. Always use .key for channel-specific commands.</summary>
		internal string key
		{
			get { return this.key ?? this.name; }
			set { }
		}
		/// <summary>Channel name, in human-readable format</summary>
		internal string name;

		/// <summary>Associated TabPage for this channel</summary>
		internal ChatTab chanTab;
		/// <summary>Minimum age to be in this channel. If set to a value greater than 0, the bot will attempt to kick everyone below this age.</summary>
		internal Int16 minAge = 0;

		internal string lastSearchFragment = "";

		internal HashSet<User> Mods = new HashSet<User>();
		internal HashSet<User> Users = new HashSet<User>();

		private CogitoSharp.IO.Logging.LogFile ChannelLog = null;

		/// <summary>
		/// Implementation of IDispose - removes tab page and disposes of Log to ensure buffer is flushed
		/// </summary>
		public void Dispose(){
			if (!disposed){
				CogitoUI.chatUI.chatTabs.TabPages.Remove(this.chanTab);
				this.ChannelLog.Dispose();
				this.chanTab.Dispose();
			}
			this.disposed = true;
		}

		public void Join(){
			IO.SystemCommand c = new IO.SystemCommand();
			c.opcode = "JCH";
			c.data["channel"] = this.key;
			c.send();
			CogitoUI.chatUI.chatTabs.TabPages.Add(this.chanTab);
			this.ChannelLog = new IO.Logging.LogFile(this.name);
		}

		public void Leave(){
			IO.SystemCommand c = new IO.SystemCommand();
			c.opcode = "LCH";
			c.data["channel"] = this.key;
			c.send();
			CogitoUI.chatUI.chatTabs.TabPages.Remove(this.chanTab);
			this.ChannelLog.Dispose();
		}

		/// <summary>
		/// Constructor, used with Public (e.g. name-only) channels
		/// </summary>
		/// <param name="_name">The channel's name</param>
		public Channel(string _name){
			this.CID = Core.channels.Count;
			this.key = null;
			this.name = _name;
			this.chanTab = new ChatTab(this);
			Core.channels.Add(this);
		}

		/// <summary>
		/// Constructor for private channel, used on Invite or after ORS
		/// </summary>
		/// <param name="_key">The channel's UUID, used to join it</param>
		/// <param name="_name">The channel's name</param>
		public Channel(string _key, string _name = "[Private Channel]") : this(_name){ this.key = _key; }

		internal void MessageReceived(CogitoSharp.IO.Message m){
			//TODO: Flash tab
			this.ChannelLog.Log(m.ToString());
		}

		/// <summary>Generic destrutor, closes associated TabPage</summary>
		~Channel()
		{
			Dispose();
		}

		//TODO Is this really how you want it?
		public override string ToString(){
			return this.key == null ? String.Format("Public Channel '{0}'", this.name) : String.Format("Private Channel '{0}', Key: {1}", this.name, this.key);
		}

		public override bool Equals(object obj){
			if (obj == null) { return false; }
			Channel c = obj as Channel;
			if (this.key == c.key) { return true; }
			//Two channels can have the same name, but never the same key.
			else { return false; }
		}

		public override int GetHashCode(){ return this.CID; }

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

		internal void newMessage(IO.Message m){
			//this.chanTab.
		}

		public void Log(string s){ this.ChannelLog.Log(s); }
	}
}
