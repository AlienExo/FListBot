using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CogitoSharp
{
	public partial class ChatUI : Form
	{
		public ChatUI(){ 
			InitializeComponent(); 
			this.chatTabs = new ChatTabControl();
			this.chatTabs.TabPages.Clear();
			this.chatTabs.TabPages.Add("Console");	
			((ChatTab)this.chatTabs.TabPages["Console"]).UserList.Hide();
			this.MdiParent = Core.cogitoUI;
			this.Hide();
		}

		private void sendButton_Click(object sender, EventArgs e){
			Console.WriteLine("Send button has been pressed");
			IO.Message m = new IO.Message();
			m.message = mainTextBox.Text;
			Object parent = ((ChatTab)this.chatTabs.SelectedTab).parent;
			if ( parent.GetType() == typeof(User)) { m.sourceUser = (User)parent; }
			else if (parent.GetType() == typeof(Channel)) { m.sourceChannel = (Channel)parent; }
			m.send();
			mainTextBox.Text = "";
		}

		private void textBox1_GotFocus(object sender, EventArgs e){ Console.WriteLine("Text Box got focus!"); this.AcceptButton = sendButton; }

		private void textBox1_KeyDown(object sender, KeyEventArgs e){
			switch (e.KeyCode) //"Depending on the key pressed, do..."
			{
				case Keys.Tab: //Key is Tab
					if (((ChatTab)this.chatTabs.SelectedTab).parent.GetType() == typeof(User)){ //this is a private conversation, so there's only one option
						//this.mainTextBox.Text.Remove(this.mainTextBox.Text.LastIndexOf(" ")); //remove all text after the last space (that's when you started typing the name...? COMMENTED OUT, FUCK IT
						this.mainTextBox.Text += " " + ((ChatTab)this.chatTabs.SelectedTab).parent.ToString() + " "; //Insert a space, the other's name, another space, BAM!
					}
					else if (((ChatTab)this.chatTabs.SelectedTab).parent.GetType() == typeof(Channel)){	//It's a channel with anywhere between 1 and 1000 users.
						Channel c = ((ChatTab)this.chatTabs.SelectedTab).parent as Channel; //create actual object to avoid a billion casts
						string searchParam;	//instantiate a string 
						if (c.lastSearchFragment.Length > 0){
							if (this.mainTextBox.Text.IndexOf(c.lastSearchFragment) > -1) { searchParam = this.mainTextBox.Text.Substring(this.mainTextBox.Text.IndexOf(c.lastSearchFragment)); }
							else { //original search param has been deleted/altered; gotta reset
								searchParam = this.mainTextBox.Text.Substring(this.mainTextBox.Text.LastIndexOf(" ")).ToLowerInvariant(); 
								c.lastSearchFragment = searchParam;
							}
						} //if you've (unsuccessfully) searched before, the search fragment is extended, except if it's been removed utterly
						else{ searchParam = this.mainTextBox.Text.Substring(this.mainTextBox.Text.LastIndexOf(" ")).ToLowerInvariant(); } //searchParam is everything from the last space forward
						string[] matches = c.Users.FindAll(n => n.Name.ToLowerInvariant().StartsWith(searchParam)).ConvertAll<string>(n => n.Name).ToArray(); //Find all users whose name Starts with (e.g. search from the right) the Search param and convert to a string array
						if (matches.Length > 1){
							c.lastSearchFragment = searchParam; //We have more than one result, so the user must supply more data. Saving our current search(in case of edits)
							c.Log("");
						}
						else if (matches.Length == 1) { 
							this.mainTextBox.Text.Remove(this.mainTextBox.Text.LastIndexOf(" "));
							this.mainTextBox.Text += " " + matches[0] + " ";
							c.lastSearchFragment = "";
						}
					}
					break;

				case Keys.Enter:
					((Channel)((ChatTab)chatTabs.SelectedTab).parent).lastSearchFragment = "";
					base.OnKeyDown(e);
					break;
				default:
					base.OnKeyDown(e);
					break;
			}
		}

		private void ChatUI_Load(object sender, EventArgs e){
			CogitoSharp.Core.websocket.OnMessage += Core.OnWebsocketMessage;
			if (!CogitoSharp.Core.websocket.IsAlive) { CogitoSharp.Core.websocket.Connect(); } //this is where we start
			//TODO grab OWN character's avatar and display on top?

		}

		private void chatTabs_DoubleClick(object sender, EventArgs e){
			TabPage page = chatTabs.SelectedTab;
			if (page != null && page.Text != "Console") { chatTabs.TabPages.Remove(page); }
		}
	}

	/// <summary>
	/// Collection of chat tabs, with a method to avoid killing the console.
	/// </summary>
	public class ChatTabControl : TabControl{
		protected override void OnControlRemoved(ControlEventArgs e){
			ChatTab tp = e.Control as ChatTab;
			if (e.Control.Text != "Console") { base.OnControlRemoved(e); }
		}

		//ChatTabControl() : base() { }

		//public Channel getParentChannel(int tabIndex) { return this.TabPages[tabIndex].parentChannel; }
	}

	//public class ChatTabCollection : TabControl.TabPageCollection{
	//	
	//}

	/// <summary>
	/// A Tab in the chat interface, representing an open conversation with a room or a single user
	/// </summary>
	internal class ChatTab : TabPage{
		private TextBox ChatTabTextInput = new TextBox();
		internal TextBox ChannelMessages = new TextBox();
		internal Object parent = null;
		internal ChatUserList UserList = new ChatUserList();
		private bool flashing = false;
		internal string AutoCompleteBuffer = "";

		private void Flash(){ 
			//TODO: implement e.g. via simple, shitty timer. Since you'll have many tabs, maybe have a central list rather than a hundred individual timers 
		}

		public ChatTab() {
			this.ChatTabTextInput.AcceptsReturn = true;
			this.ChannelMessages.ReadOnly = true;
			this.ChannelMessages.Multiline = true;
			this.SuspendLayout();
			this.Controls.Add(ChatTabTextInput);

			ChannelMessages.Parent = this;
			ChannelMessages.Dock = DockStyle.Fill;
			ChannelMessages.Anchor = AnchorStyles.None;

			ChatTabTextInput.Parent = this;
			ChatTabTextInput.Dock = DockStyle.Bottom;
			ChatTabTextInput.Anchor = AnchorStyles.Bottom;
			this.ResumeLayout();
			ChatTabTextInput.BringToFront();

			this.UserList.Items.Add(Core.OwnUser);
		}

		public ChatTab(Channel parent) : this(){
			this.Name = parent.name;
			this.Text = parent.name;
			this.parent = parent;
			this.UserList = new ChatUserList();
			this.SuspendLayout();
			this.Controls.Add(this.UserList);
			this.UserList.Dock = DockStyle.Right;
			this.UserList.Parent = this;
			this.UserList.Anchor = AnchorStyles.Right;
			this.ResumeLayout();
		}

		public ChatTab(User parent) : this(){
			this.Name = parent.Name;
			this.Text = parent.Name;
			this.parent = parent;
		}
	}

	internal class ChatUserList : OwnerDrawnListBox{
		
	}
}
