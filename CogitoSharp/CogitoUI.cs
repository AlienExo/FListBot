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
    public partial class CogitoUI : Form
    {
        public CogitoUI()
        {
            InitializeComponent();
			this.IsMdiContainer = true;
		}
		
        private void sendButton_Click(object sender, EventArgs e)
        {
            Console.WriteLine(textBox1.Text);
            textBox1.Text = "";
        }

        private void textBox1_GotFocus(object sender, EventArgs e)
        {
            Console.WriteLine(sender.ToString());
            this.AcceptButton = sendButton;
            //TODO: set sendButton to get whichever tab is in focus/front and therefore where to send what's entered
        }

		private void CogitoUI_Load(object sender, EventArgs e)
		{
			//this.Visible = false;

			this.Text = "Cogito v"+Application.ProductVersion;
			//this.Hide();
			loginForm LoginForm = new loginForm();
			LoginForm.MdiParent = this;
			LoginForm.Show();
		}
	}

	public class ChatTab : TabPage
    { 
		private TextBox ChatTabTextInput = new TextBox();

        public ChatTab(string _title)
        {
            base.Name = _title;
            base.Text = _title;
            this.ChatTabTextInput.AcceptsReturn = true;
            this.SuspendLayout();
            this.Controls.Add(ChatTabTextInput);
            this.ResumeLayout();
            ChatTabTextInput.Parent = this;
            ChatTabTextInput.Dock = DockStyle.Fill;
            ChatTabTextInput.BringToFront();
        }

    }
}