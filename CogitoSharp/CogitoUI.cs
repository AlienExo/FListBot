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
			loginPanel.Visible = true;
			this.rememberPasswordCheck.Checked = Properties.Settings.Default.savePassword;
			loginPanel.BringToFront();
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

		private void loginPanel_VisibleChanged(object sender, EventArgs e)
		{
			if (loginPanel.Visible == true){
				this.loginSubmitButton.Visible = true;
				this.loginPasswordField.Visible = true;
				this.loginUserField.Visible = true;
				this.UsernameLabel.Visible = true;
				this.PasswordLabel.Visible = true;
				this.ActiveControl = this.loginUserField;
				this.loginUserField.AutoCompleteCustomSource = Properties.Settings.Default.userAutoComplete;

			}
		}

		private void loginButton_Click(object sender, EventArgs e)
		{
			if (this.rememberPasswordCheck.Checked == true){
				Console.WriteLine("Savings password " + this.loginPasswordField.Text); 
				Properties.Settings.Default.Password = this.loginPasswordField.Text;
			}
			else {Properties.Settings.Default.Password = "";}
			Properties.Settings.Default.Account = this.loginUserField.Text;
			Properties.Settings.Default.Save();
			if (!CogitoSharp.Account.login(this.loginUserField.Text, this.loginPasswordField.Text)){
				this.loginStatusLabel.ForeColor = System.Drawing.Color.Red;
				this.loginStatusLabel.Text = "Could not log in. Please check Account name and password and try again.";
			}
			else{
				if (Properties.Settings.Default.userAutoComplete.Contains(this.loginUserField.Text) == false){
					Properties.Settings.Default.userAutoComplete.Add(this.loginUserField.Text);}
				loginPanel.Hide();
			}
				
		}

		private void loginUser_Enter(object sender, EventArgs e)
		{
			this.loginUserField.Text = Properties.Settings.Default.Account;
			this.loginPasswordField.Text = Properties.Settings.Default.Password;
		}

		private void CogitoUI_Load(object sender, EventArgs e)
		{
			this.Text = "Cogito v"+Application.ProductVersion;
		}

		private void rememberPasswordCheck_CheckedChanged(object sender, EventArgs e)
		{
			Properties.Settings.Default.savePassword = this.rememberPasswordCheck.Checked;
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
