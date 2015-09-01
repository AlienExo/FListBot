﻿using System;
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
		public ChatUI(){ InitializeComponent(); }

		private void sendButton_Click(object sender, EventArgs e){
			Console.WriteLine(mainTextBox.Text);
			mainTextBox.Text = "";
		}

		private void textBox1_GotFocus(object sender, EventArgs e){
			Console.WriteLine(sender.ToString());
			this.AcceptButton = sendButton;
			//TODO: set sendButton to get whichever tab is in focus/front and therefore where to send what's entered
		}

		private void SimpleConsoleDumper(object sender, WebSocketSharp.MessageEventArgs e){
			string a = e.Data.ToString();
			if(a.Substring(3) != "LIS"){Console.WriteLine(a);}
		}

		private void ChatUI_Load(object sender, EventArgs e){
			Console.WriteLine("Registering prototype reporting subroutine...");
			//CogitoSharp.Core.websocket.OnMessage += SimpleConsoleDumper;//TODO: Insert RawMessage-to-Object-onto-Collections.Queue
			CogitoSharp.Core.websocket.OnMessage += Core.OnWebsocketMessage;
			if (!CogitoSharp.Core.websocket.IsAlive)
			{
				CogitoSharp.Core.websocket.Connect(); //this is where we start
				System.Threading.Thread.Sleep(1000);
			}
			//grab OWN character's avatar and display on top?
		}

		private void chatTabs_DoubleClick(object sender, EventArgs e){
			TabPage page = chatTabs.SelectedTab;
			if (page != null && page.Text != "Console") { chatTabs.TabPages.Remove(page); }
		}

	}

	public class ChatTabControl : TabControl{
		protected override void OnControlRemoved(ControlEventArgs e){
			ChatTab tp = e.Control as ChatTab;
			if (e.Control.Text != "Console") { base.OnControlRemoved(e); }
		}
	}

	public class ChatTab : TabPage{
		private TextBox ChatTabTextInput = new TextBox();
		internal TextBox ChannelMessages = new TextBox();
		private bool flashing = false;

		private void Flash(){
		
		}

		public ChatTab(string _title){
			base.Name = _title;
			base.Text = _title;
			this.ChatTabTextInput.AcceptsReturn = true;
			this.ChannelMessages.ReadOnly = true;
			this.ChannelMessages.Multiline = true;
			this.SuspendLayout();
			this.Controls.Add(ChatTabTextInput);
			this.ResumeLayout();
			ChannelMessages.Parent = this;
			ChannelMessages.Dock = DockStyle.Top;
			ChatTabTextInput.Parent = this;
			ChatTabTextInput.Dock = DockStyle.Bottom;
			ChatTabTextInput.BringToFront();
		}

	}
}
