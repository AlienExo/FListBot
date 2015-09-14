﻿using System;
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
		
		/// <summary>Keys are the UUID for private channels; channel title for normal. Always use .key for channel-specific commands.</summary>
		[NonSerialized] internal string _key;

		internal string key
		{
			get { return this._key ?? this.name; }
			set { this._key = value;}
		}

		/// <summary>Channel name, in human-readable format</summary>
		internal string name;


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

		[NonSerialized] private CogitoSharp.IO.Logging.LogFile ChannelLog = null;
		
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

		public void Join(){
			IO.SystemCommand c = new IO.SystemCommand();
			c.OpCode = "JCH";
			c.Data["channel"] = this.key;
			c.Send();
			if (CogitoUI.chatUI.InvokeRequired){
				ChatUI.AddChatTabCallback a = new ChatUI.AddChatTabCallback(CogitoUI.chatUI.chatTabs.TabPages.Add);
				CogitoUI.chatUI.Invoke(a, new object[] { CogitoUI.chatUI });
			}
			else { CogitoUI.chatUI.chatTabs.TabPages.Add(this.chanTab); }
			this.ChannelLog = new IO.Logging.LogFile(this.name);
			this.isJoined = true;
			CogitoUI.chatUI.Refresh();
		}

		public void Leave(){
			IO.SystemCommand c = new IO.SystemCommand();
			c.OpCode = "LCH";
			c.Data["channel"] = this.key;
			c.Send();
			if (CogitoUI.chatUI.InvokeRequired)
			{
				ChatUI.RemoveChatTabCallback _a = new ChatUI.RemoveChatTabCallback(CogitoUI.chatUI.chatTabs.TabPages.Add);
				CogitoUI.chatUI.Invoke(_a, new object[] { CogitoUI.chatUI });
			}
			else { CogitoUI.chatUI.chatTabs.TabPages.Remove(this.chanTab); }
			this.ChannelLog.Dispose();
			this.isJoined = false;
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
			this.chanTab.Name = this.name;
			Core.channels.Add(this);
		}

		/// <summary>
		/// Constructor for private channel, used on Invite or after ORS
		/// </summary>
		/// <param name="_key">The channel's UUID, used to join it</param>
		/// <param name="_name">The channel's name</param>
		public Channel(string _key, string _name = "[Private Channel]") : this(_name){ this.key = _key; }

		internal void MessageReceived(CogitoSharp.IO.Message m){
			string _m = m.ToString();
			this.ChannelLog.LogRaw(_m);
			if (this.chanTab.messageBuffer.Length == Config.AppSettings.MessageBufferSize) { Array.Copy(this.chanTab.messageBuffer, 1, this.chanTab.messageBuffer, 0, this.chanTab.messageBuffer.Length - 1); }
			this.chanTab.messageBuffer[this.chanTab.messageBuffer.Length + 1] = m.ToString();
			if (!CogitoUI.chatUI.chatTabs.TabPages.Contains(this.chanTab)) { CogitoUI.chatUI.chatTabs.TabPages.Add(this.chanTab); }
			//this.chanTab.Flash(); //TODO: Flash tab			
		}

		/// <summary>Generic destrutor, closes associated TabPage</summary>
		~Channel()
		{
			Dispose();
		}

		//TODO Is this really how you want it?
		public override string ToString(){
			return this._key == null ? String.Format("Public Channel '{0}'", this.name) : String.Format("Private Channel '{0}' ({1})", this.name, this.key);
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

		public void Log(string s){ this.ChannelLog.Log(s); }
	}
}
