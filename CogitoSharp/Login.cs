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
	public partial class LoginForm : Form
	{
		protected internal bool AdvancedLoginOptionsShown;

		public LoginForm()
		{
			InitializeComponent();
			this.rememberPasswordCheck.Checked = Properties.Settings.Default.savePassword;
			loginElements.BringToFront();
			this.loginUserField.AutoCompleteCustomSource = Properties.Settings.Default.userAutoComplete;
		}

		private void characterSelectButton_Click(object sender, EventArgs e)
		{
			Account.characterSelect(this.characterSelectBox.SelectedText);
			this.Hide();
			Console.WriteLine("Now attempting to resume main UI");
			CogitoUI.chatUI.Show();
		}

		private void loginSubmitButton_Click(object sender, EventArgs e)
		{
			if(this.AdvancedLoginOptionsShown == true){showAdvancedLoginOptions();}
			if (!CogitoSharp.Core.websocket.IsAlive)
			{
				CogitoSharp.Core.websocket.Connect();
				System.Threading.Thread.Sleep(100);
			}
			this.loginErrorLabel.Text = "";
			this.loginSubmitButton.Text = "Login...";
			this.loginSubmitButton.Enabled = false;
			if (this.rememberPasswordCheck.Checked == true){Properties.Settings.Default.Password = this.loginPasswordField.Text;}
			else { Properties.Settings.Default.Password = ""; }
			Properties.Settings.Default.Account = this.loginUserField.Text;
			Properties.Settings.Default.savePassword = this.rememberPasswordCheck.Checked;
			Properties.Settings.Default.Save();
			string loginError = "";
			if (!CogitoSharp.Account.login(this.loginUserField.Text, this.loginPasswordField.Text, out loginError))
			{
				//shake animation
				this.loginErrorLabel.Text = loginError;
				int initialPasswordXLocation = this.loginElements.Location.X;
				int initialPasswordYLocation = this.loginElements.Location.Y;
				float[] shakevalues = Utils.Math.dampenedSpringDelta(initialPasswordXLocation, 10f);
				foreach (float x in shakevalues)
				{
					this.loginElements.Location = new Point((int)x, initialPasswordYLocation);
					this.loginElements.Refresh();
					System.Threading.Thread.Sleep(20);
				}
				this.loginSubmitButton.Enabled = true;
				this.loginSubmitButton.Text = "Login";
			}
			else{
				this.loginSubmitButton.Enabled = true;
				if (Properties.Settings.Default.userAutoComplete.Contains(this.loginUserField.Text) == false){Properties.Settings.Default.userAutoComplete.Add(this.loginUserField.Text);}
				this.loginElements.Hide();
				this.characterSelectBox.Enabled = true;
			}
		}

		private void loginForm_Load(object sender, EventArgs e)
		{
			this.loginUserField.Text = Properties.Settings.Default.Account;
			this.loginPasswordField.Text = Properties.Settings.Default.Password;
			this.ActiveControl = this.loginUserField;
		}

		private void showAdvancedLoginOptions(){
			Point _pictureLocation = this.CogitoLogoBox.Location;
			for(int i = 0; i<this.CogitoLogoBox.Width; i++){
				if (!AdvancedLoginOptionsShown){this.CogitoLogoBox.Location= new Point(_pictureLocation.X + i, _pictureLocation.Y);}
				else {this.CogitoLogoBox.Location= new Point(_pictureLocation.X - i, _pictureLocation.Y);}
				this.CogitoLogoBox.Refresh();
			}
		}

		private void showAdvancedLoginButton_Click(object sender, EventArgs e)
		{
			showAdvancedLoginOptions();
			AdvancedLoginOptionsShown = !AdvancedLoginOptionsShown;
		}

		private void characterSelectBox_EnabledChanged(object sender, EventArgs e)
		{
			this.characterSelectBox.DataSource = Account.loginkey.characters;
			this.characterSelectBox.SelectedIndex = 0;
		}

	}
}
