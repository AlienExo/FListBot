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
			loginElements.BringToFront();
			this.loginUserField.AutoCompleteCustomSource = Properties.Settings.Default.userAutoComplete;
		}

		private void characterSelectButton_Click(object sender, EventArgs e)
		{
			Account.characterSelect((string)this.characterSelectBox.SelectedItem);
			this.Hide();
			Console.WriteLine(String.Format("WebSocket is alive: {0}", CogitoSharp.Core.websocket.IsAlive));
			CogitoUI.chatUI.Show();
		}

		private void loginSubmitButton_Click(object sender, EventArgs e)
		{
			if(this.AdvancedLoginOptionsShown == true){showAdvancedLoginOptions();}
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
			this.rememberPasswordCheck.Checked = Properties.Settings.Default.savePassword;
			this.portSelectionBox.Text = Properties.Settings.Default.Port.ToString();
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

		private void AdvancedLoginOptionsApplyButton_Click(object sender, EventArgs e)
		{
			Properties.Settings.Default.Port = int.Parse(this.portSelectionBox.Text);
			if (this.WSButton.Checked == true){Core.websocket = new WebSocketSharp.WebSocket(String.Format("ws://{0}:{1}", Properties.Settings.Default.Host, Properties.Settings.Default.Port));}
			else if (this.WSSButton.Checked == true){Core.websocket = new WebSocketSharp.WebSocket(String.Format("ws://{0}:{1}", Properties.Settings.Default.Host, Properties.Settings.Default.Port));}
			this.showAdvancedLoginButton_Click(sender, e);
			Console.WriteLine(String.Format("\tForm Value: {0} Type: {1}\n\tSettings value: {2} Type: {3}", this.portSelectionBox.Text, this.portSelectionBox.Text.GetType(), Properties.Settings.Default.Port, Properties.Settings.Default.Port.GetType()));
		}
	}
}
