using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

/*
 * TODO: [----------] Add code to populate ChatListUser box
 * TODO: [----------] Add code to populate MenuUseRightClick 
 * TODO: [----------] Add code that only right clicks over the UserList cause that menu to open
 */

namespace CogitoSharp
{
	public partial class ChatUI : Form
	{
		internal delegate void AddChatTabCallback(ChatTab tab);
		internal delegate void RemoveChatTabCallback(ChatTab tab);

		ChatTab chatConsole = null;

		public ChatUI(){ 
			InitializeComponent(); 
			this.chatTabs = new ChatTabControl();
			this.chatTabs.TabPages.Clear();
			chatConsole = new ChatTab();
			this.chatTabs.TabPages.Add(chatConsole);	
			chatConsole.UserList.Hide();
			this.MdiParent = Core.cogitoUI;
			this.Hide();
		}

		private void sendButton_Click(object sender, EventArgs e){
			IO.Message m = new IO.Message();
			m.message = mainTextBox.Text;
			Object parent = ((ChatTab)this.chatTabs.SelectedTab).parent;
			if(parent == null) { mainTextBox.Text = ""; return; }
			if ( parent.GetType() == typeof(User)) { m.sourceUser = (User)parent; }
			else if (parent.GetType() == typeof(Channel)) { m.sourceChannel = (Channel)parent; }
			m.Send();
			mainTextBox.Text = "";
			mainTextBox.Select();
		}

		private void mainTextBox_Enter(object sender, EventArgs e){  this.AcceptButton = sendButton; }

		private void ChatUI_Load(object sender, EventArgs e){
			if (Core.OwnUser.Avatar == null) { Core.OwnUser.GetAvatar(); }
			this.label1.Text = Core.OwnUser.Name;
			try{ this.currenctCharAvatar.Image = new Bitmap(Core.OwnUser.Avatar, this.currenctCharAvatar.Size); }
			catch (Exception) { }
		}

		private void chatTabs_DoubleClick(object sender, EventArgs e){
			TabPage page = chatTabs.SelectedTab;
			if (page != null && page.Text != "Console") { chatTabs.TabPages.Remove(page); }
		}

		private void mainTextBox_KeyDown(object sender, KeyEventArgs e)
		{
			switch (e.KeyCode) //"Depending on the key pressed, do..."
			{
				case Keys.Tab: //Key is Tab
					if (((ChatTab)this.chatTabs.SelectedTab).parent.GetType() == typeof(User))
					{ //this is a private conversation, so there's only one option
						//this.mainTextBox.Text.Remove(this.mainTextBox.Text.LastIndexOf(" ")); //remove all text after the last space (that's when you started typing the name...? COMMENTED OUT, FUCK IT
						this.mainTextBox.Text += " " + ((ChatTab)this.chatTabs.SelectedTab).parent.ToString() + " "; //Insert a space, the other's name, another space, BAM!
					}
					else if (((ChatTab)this.chatTabs.SelectedTab).parent.GetType() == typeof(Channel))
					{	//It's a channel with anywhere between 1 and 1000 users.
						Channel c = ((ChatTab)this.chatTabs.SelectedTab).parent as Channel; //create actual object to avoid a billion casts
						object SearchObject;
						if (c.name == "Console") { SearchObject = Core.allGlobalUsers; }
						else { SearchObject = c.Users; }
						string searchParam;	//instantiate a string 
						if (c.lastSearchFragment.Length > 0)
						{
							if (this.mainTextBox.Text.IndexOf(c.lastSearchFragment) > -1) { searchParam = this.mainTextBox.Text.Substring(this.mainTextBox.Text.IndexOf(c.lastSearchFragment)); }
							else
							{ //original search param has been deleted/altered; gotta reset
								searchParam = this.mainTextBox.Text.Substring(this.mainTextBox.Text.LastIndexOf(" ")).ToLowerInvariant();
								c.lastSearchFragment = searchParam;
							}
						} //if you've (unsuccessfully) searched before, the search fragment is extended, except if it's been removed utterly
						else { searchParam = this.mainTextBox.Text.Substring(this.mainTextBox.Text.LastIndexOf(" ")).ToLowerInvariant(); } //searchParam is everything from the last space forward
						string[] matches = ((HashSet<User>)SearchObject).Where(n => n.Name.ToLowerInvariant().StartsWith(searchParam)).Select(n => n.Name).ToArray<string>(); //Find all users whose name Starts with (e.g. search from the right) the Search param and convert to a string array
						if (matches.Length > 1)
						{
							c.lastSearchFragment = searchParam; //We have more than one result, so the user must supply more data. Saving our current search(in case of edits)
							c.Log(String.Format("Multiple possible matches for {0}: {1}", searchParam, String.Join(" ", matches)));
						}
						else if (matches.Length == 1)
						{
							this.mainTextBox.Text.Remove(this.mainTextBox.Text.LastIndexOf(" "));
							this.mainTextBox.Text += " " + matches[0] + " ";
							c.lastSearchFragment = "";
						}
						else { c.Log("No matches found to autocomplete " + searchParam); }
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


	class ChatRichTextBox : RichTextBox{
		delegate void AppendTextCallback(string text);

		public new void AppendText(string text)
		{
			if (this.InvokeRequired)
			{
				AppendTextCallback d = new AppendTextCallback(AppendText);
				this.Invoke(d, new object[] { text });
			}
			else
			{
				if (!text.EndsWith(Environment.NewLine)) { text += Environment.NewLine; }
				this.Text += text;
			}
		}
	}

	/// <summary>
	/// A Tab in the chat interface, representing an open conversation with a room or a single user
	/// </summary>
	internal class ChatTab : TabPage{
		private TextBox ChatTabTextInput = new TextBox();
		internal ChatRichTextBox ChannelMessages = new ChatRichTextBox();
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
			parent.GetAvatar();
		}
	}

	//TODO Hack
	internal class ChatUserList : OwnerDrawnListBox{
		const int FONT_SIZE    = 10;
		const int DRAW_OFFSET  = 5;

		public ChatUserList(){

			// Determine what the item height should be
			// by adding 30% padding after measuring
			// the letter A with the selected font.
			Graphics g = this.CreateGraphics();
			this.ItemHeight = (int)(g.MeasureString("A", this.Font).Height * 1.3);
			g.Dispose();
		}

		// Return the name of the selected font.
		public string SelectedFaceName{
			get{ return (string)this.Items[this.SelectedIndex]; }
		}

		// Determine what the text color should be
		// for the selected item drawn as highlighted
		Color CalcTextColor(Color backgroundColor){
			if(backgroundColor.Equals(Color.Empty)) { return Color.Black;}

			int sum = backgroundColor.R + backgroundColor.G + backgroundColor.B;

			if(sum > 256){ return Color.Black; }
			else { return Color.White; }
		}

		//TODO: Reprogram the shit out of. Font color... icons... the works.
		protected override void OnPaint(PaintEventArgs e){
			Font font;
			Color fontColor;

			// The base class contains a bitmap, offScreen, for constructing
			// the control and is rendered when all items are populated.
			// This technique prevents flicker.
			Graphics gOffScreen = Graphics.FromImage(this.OffScreen);
			gOffScreen.FillRectangle(new SolidBrush(this.BackColor), this.ClientRectangle);

			int itemTop = 0;

			// Draw the fonts in the list.
			for(int n = this.VScrollBar.Value; n < this.VScrollBar.Value + DrawCount; n++){
				// If the font name contains "dings" it needs to be displayed
				// in the list box with a readable font with the default font.
				if(((string)this.Items[n]).ToLower().IndexOf("dings") != -1) { font = new Font(this.Font.Name, FONT_SIZE, FontStyle.Regular); }
				else { font = new Font((string)this.Items[n], FONT_SIZE, FontStyle.Regular); }

				// Draw the selected item to appear highlighted
				if(n == this.SelectedIndex){
					gOffScreen.FillRectangle(new SolidBrush(SystemColors.Highlight),
						1,
						itemTop + 1,
						// If the scroll bar is visible, subtract the scrollbar width
						// otherwise subtract 2 for the width of the rectangle
						this.ClientSize.Width - (this.VScrollBar.Visible ? this.VScrollBar.Width : 2),
						this.ItemHeight);
					fontColor = CalcTextColor(SystemColors.Highlight);
				}
				else{ fontColor = this.ForeColor; }

				// Draw the item
				gOffScreen.DrawString((string)this.Items[n], font, new SolidBrush(fontColor), DRAW_OFFSET, itemTop);
				itemTop += this.ItemHeight;
			}

			// Draw the list box
			e.Graphics.DrawImage(this.OffScreen, 1, 1);

			gOffScreen.Dispose();
		}

		// Draws the external border around the control.

		protected override void OnPaintBackground(PaintEventArgs e){
			e.Graphics.DrawRectangle(new Pen(Color.Black), 0, 0, this.ClientSize.Width - 1, this.ClientSize.Height - 1);
		}
	}
}
