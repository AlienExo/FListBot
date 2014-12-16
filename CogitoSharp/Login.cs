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
	public partial class loginForm : Form
	{
		protected internal bool AdvancedLoginOptionsShown;

		public loginForm()
		{
			InitializeComponent();
			this.rememberPasswordCheck.Checked = Properties.Settings.Default.savePassword;
			loginElements.BringToFront();
			this.loginUserField.AutoCompleteCustomSource = Properties.Settings.Default.userAutoComplete;
		}

		private void rememberPasswordCheck_CheckedChanged(object sender, EventArgs e)
		{
			Properties.Settings.Default.savePassword = this.rememberPasswordCheck.Checked;
		}

		private void charSelectBox_Enter(object sender, EventArgs e)
		{
			this.charSelectBox.DataSource = Account.Characters;
		}

		private void charSelectButton_Click(object sender, EventArgs e)
		{
			Account.characterSelect(this.charSelectBox.SelectedText);
			this.Hide();
			Core.cogitoUI.Show();
			Core.cogitoUI.ShowInTaskbar = true;
		}

		private void charSelectPanel_VisibleChanged(object sender, EventArgs e)
		{
			if (this.charSelectPanel.Visible == true){
				this.charSelectButton.Visible = true;
				this.charSelectBox.Visible = true;
			}
		}

		private void loginSubmitButton_Click(object sender, EventArgs e)
		{
			if (this.rememberPasswordCheck.Checked == true)
			{
				Console.WriteLine("Savings password " + this.loginPasswordField.Text);
				Properties.Settings.Default.Password = this.loginPasswordField.Text;
			}
			else { Properties.Settings.Default.Password = ""; }
			Properties.Settings.Default.Account = this.loginUserField.Text;
			Properties.Settings.Default.Save();
			if (!CogitoSharp.Account.login(this.loginUserField.Text, this.loginPasswordField.Text))
			{
				//shake animation
				int maxDeviation = 20;

				//for (float i = 0; i<1; i+=0.1f){
				//	int deltaX = this.loginPasswordField.Location.X + (int)deviation * maxDeviation;
				//	Console.WriteLine(this.loginPasswordField.Location);
				//	this.loginPasswordField.Location = new Point(deltaX, this.loginPasswordField.Location.Y);
				//	Console.WriteLine(this.loginPasswordField.Location);
				//	this.loginPasswordField.Refresh();
				//}

				this.loginStatusLabel.ForeColor = System.Drawing.Color.Red;
				this.loginStatusLabel.Text = "Could not log in. Please check Account name and password and try again.";
				this.loginStatusLabel.BringToFront();
			}
			else
			{
				if (Properties.Settings.Default.userAutoComplete.Contains(this.loginUserField.Text) == false)
				{
					Properties.Settings.Default.userAutoComplete.Add(this.loginUserField.Text);
				}
				this.loginElements.Hide();
				this.charSelectPanel.Visible = true;
				this.charSelectPanel.BringToFront();

			}
		}

		private void loginForm_Load(object sender, EventArgs e)
		{
			this.ActiveControl = this.loginUserField;
			this.loginUserField.Text = Properties.Settings.Default.Account;
			this.loginPasswordField.Text = Properties.Settings.Default.Password;
		}

		private void loginForm_FormClosed(object sender, FormClosedEventArgs e)
		{
			//this.Parent.
		}

		private void showAdvancedLoginButton_Click(object sender, EventArgs e)
		{
			Point _pictureLocation = this.CogitoLogoBox.Location;
			for(int i = 0; i<this.CogitoLogoBox.Width; i++){
				if (!AdvancedLoginOptionsShown){this.CogitoLogoBox.Location= new Point(_pictureLocation.X + i, _pictureLocation.Y);}
				else {this.CogitoLogoBox.Location= new Point(_pictureLocation.X - i, _pictureLocation.Y);}
				this.CogitoLogoBox.Refresh();
			}
			AdvancedLoginOptionsShown = !AdvancedLoginOptionsShown;
		}
	}
}
