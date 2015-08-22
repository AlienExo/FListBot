using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CogitoSharp
{
	/// <summary>Channel class, for both public and private channels</summary>
	public class Channel : IComparable, IDisposable
	{
		/// <summary>Channel ID Number</summary>
		static int Count;
		private readonly int CID = ++Channel.Count;
		/// <summary>Keys are the UUID for private channels; channel title for normal. Always use .key for channel-specific commands.</summary>
		public string key
		{
			get { return this.key ?? this.name; }
			set { }
		}
		/// <summary>Channel name, in human-readable format</summary>
		public string name;
		/// <summary>Array of all Users in the channel</summary>
		private User[] users;
		/// <summary>Associated TabPage for this channel</summary>
		private TabPage chanTab;
		/// <summary>Minimum age to be in this channel. If set to a value greater than 0, the bot will attempt to kick everyone below this age.</summary>
		private Int16 minAge = 0;
		private bool disposed = false;
		/// <summary>
		/// 
		/// </summary>
		/// 
		public void Dispose(){
			CogitoUI.chatUI.chatTabs.TabPages.Remove(this.chanTab);
		}

		public bool Join()
		{
			throw new NotImplementedException();
		}

		public void Leave() { throw new NotImplementedException(); }

		/// <summary>
		/// Constructor, used with Public (e.g. name-only) channels
		/// </summary>
		/// <param name="_name">The channel's name</param>
		public Channel(string _name)
		{
			this.key = _name;
			this.name = _name;
			this.chanTab = new ChatTab(this.name);
			CogitoUI.chatUI.chatTabs.TabPages.Add(this.chanTab);
			Core.channels.Add(this);
		}

		/// <summary>
		/// Constructor for private channel, used on Invite or after ORS
		/// </summary>
		/// <param name="_key">The channel's UUID, used to join it</param>
		/// <param name="_name">The channel's name</param>
		public Channel(string _key, string _name = "[Private Channel]")
		{
			this.key = _key;
			this.name = _name;
			this.chanTab = new ChatTab(this.name);
			CogitoUI.chatUI.chatTabs.TabPages.Add(this.chanTab);
			Core.channels.Add(this);
			this.CID = Core.channels.Count;
		}

		/// <summary>Generic destrutor, closes associated TabPage</summary>
		~Channel()
		{
			Dispose();
		}

		public override string ToString()
		{
			return this.name;
		}

		public override bool Equals(object obj)
		{
			if (obj == null) { return false; }
			Channel c = obj as Channel;
			if (this.key == c.key) { return true; }
			//Two channels can have the same name, but never the same key.
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
}
