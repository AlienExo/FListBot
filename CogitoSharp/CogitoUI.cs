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
    public partial class CogitoUI : Form
    {
        public CogitoUI()
        {
            InitializeComponent();
			loginPanel.BringToFront();
        }

        private void sendButton_Click(object sender, EventArgs e)
        {
            Console.WriteLine(textBox1.Text);
            textBox1.Text = "";
        }

        private void chatTabs_Selected(object sender, EventArgs e) 
        { 
            
        }

        private void textBox1_GotFocus(object sender, EventArgs e)
        {
            Console.WriteLine(sender.ToString());
            this.AcceptButton = sendButton;
            //TODO: set sendButton to get whichever tab is in focus/front and therefore where to send what's entered
        }

		private void loginPanel_VisibleChanged(object sender, EventArgs e)
		{
			this.loginButton.Visible = true;
			this.loginPassword.Visible = true;
			this.loginUser.Visible = true;
			this.UsernameLabel.Visible = true;
			this.PasswordLabel.Visible = true;
			this.loginUser.Focus();
		}

		private void loginButton_Click(object sender, EventArgs e)
		{
			if (this.rememberPasswordCheck.Checked == true){Properties.Settings.Default.Password = this.loginPassword.Text;}
			else {Properties.Settings.Default.Password = "";}
			CogitoSharp.Account.login(this.loginUser.Text, this.loginPassword.Text);
		}

		private void loginUser_VisibleChanged(object sender, EventArgs e)
		{
			this.Text = Properties.Settings.Default.Account;
		}

		private void loginPassword_VisibleChanged(object sender, EventArgs e)
		{
			this.Text = Properties.Settings.Default.Password;
		}    
    }

    public class ChatTab : TabPage
    { 
        
        private TextBox text = new TextBox();

        public ChatTab(string _title)
        {
            base.Name = _title;
            base.Text = _title;
            this.text.AcceptsReturn = true;
            this.SuspendLayout();
            this.Controls.Add(text);
            this.ResumeLayout();
            text.Parent = this;
            text.Dock = DockStyle.Fill;
            text.BringToFront();
        }

    }
}
