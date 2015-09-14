using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CogitoSharp
{
	internal partial class LoginForm : Form
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
			if ((DateTime.Now - Account.LoginKey.ticketTaken) >= Config.AppSettings.ticketLifetime) { 
				string _dummy;
				Account.login(this.loginUserField.Text, this.loginPasswordField.Text, out _dummy);
			}
			Account.characterSelect((string)this.characterSelectBox.SelectedItem);
			this.Hide();
			Core.OwnUser = new User((string)this.characterSelectBox.SelectedItem); //set the ref needed for the GUI to construct; else it fails with 0 items due to ~scroll bars~
			CogitoUI.chatUI = new ChatUI();
			CogitoUI.chatUI.Show();
		}

		private void loginSubmitButton_Click(object sender, EventArgs e)
		{
			this.loginUserField.Enabled = false;
			this.loginPasswordField.Enabled = false;
			this.showAdvancedLoginButton.Enabled = false;
			Application.DoEvents();
			if(this.AdvancedLoginOptionsShown){showAdvancedLoginOptions();}
			this.loginErrorLabel.Text = "";
			this.loginSubmitButton.Text = "Login...";
			this.loginSubmitButton.Enabled = false;
			Properties.Settings.Default.Account = this.loginUserField.Text;
			Properties.Settings.Default.savePassword = this.rememberPasswordCheck.Checked;
			string loginError = "";
			if (!CogitoSharp.Account.login(this.loginUserField.Text, this.loginPasswordField.Text, out loginError))
			{
				//shake animation
				this.loginErrorLabel.Text = loginError;
				this.loginErrorLabel.Show();
				int initialPasswordXLocation = this.loginElements.Location.X;
				int initialPasswordYLocation = this.loginElements.Location.Y;
				float[] shakevalues = Utils.Math.dampenedSpringDelta(initialPasswordXLocation, 10f);
				foreach (float x in shakevalues)
				{
					this.loginElements.Location = new Point((int)x, initialPasswordYLocation);
					this.loginElements.Refresh();
					System.Threading.Thread.Sleep(15);
				}
				this.loginSubmitButton.Enabled = true;
				this.loginSubmitButton.Text = "Login";
				this.loginPasswordField.Enabled = true;
				this.loginUserField.Enabled = true;
				this.showAdvancedLoginButton.Enabled = true;

				this.loginPasswordField.Text = "";
			}
			else{
				if (Properties.Settings.Default.userAutoComplete.Contains(this.loginUserField.Text) == false){Properties.Settings.Default.userAutoComplete.Add(this.loginUserField.Text);}
				if (this.rememberPasswordCheck.Checked == true){
					try{
						if (!File.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory + CogitoSharp.Config.AppSettings.UserFileName))) { File.Create(Path.Combine(AppDomain.CurrentDomain.BaseDirectory + CogitoSharp.Config.AppSettings.UserFileName)); }
						using (FileStream fs = new FileStream(Path.Combine(AppDomain.CurrentDomain.BaseDirectory + CogitoSharp.Config.AppSettings.UserFileName), FileMode.Truncate)) //Truncate deletes all existing content!
						{
							byte[] PW = System.Text.Encoding.ASCII.GetBytes(this.loginPasswordField.Text);
							byte[] encryptedPW;
							byte[] entropy = new byte[16];
							new RNGCryptoServiceProvider().GetBytes(entropy);
							encryptedPW = ProtectedData.Protect(PW, entropy, DataProtectionScope.CurrentUser);
							fs.Write(encryptedPW, 0, encryptedPW.Length);
							fs.Flush();
							Properties.Settings.Default.PWEntropy = System.Convert.ToBase64String(entropy);
							Properties.Settings.Default.PWLen = encryptedPW.Length;
							Properties.Settings.Default.Save();
						}
					}

					catch (IOException){ }
				}
				this.loginElements.Hide();
				this.characterSelectBox.Enabled = true;
				this.characterSelectBox.BringToFront();
			}
		}

		private void loginForm_Load(object sender, EventArgs e){
			this.loginUserField.Text = Properties.Settings.Default.Account;
			this.rememberPasswordCheck.Checked = Properties.Settings.Default.savePassword;
			this.portSelectionBox.Text = Properties.Settings.Default.Port.ToString();
			try{
				byte[] DecryptedPassword;
				byte[] ReadingBuffer = new byte[Properties.Settings.Default.PWLen];
				using (FileStream fs = new FileStream(Path.Combine(AppDomain.CurrentDomain.BaseDirectory + CogitoSharp.Config.AppSettings.UserFileName), FileMode.Open))
				{
					if (fs.CanRead) { fs.Read(ReadingBuffer, 0, Properties.Settings.Default.PWLen); }
				}
				DecryptedPassword = ProtectedData.Unprotect(ReadingBuffer, System.Convert.FromBase64String(Properties.Settings.Default.PWEntropy), DataProtectionScope.CurrentUser);
				this.loginPasswordField.Text = Encoding.ASCII.GetString(DecryptedPassword);
				this.ActiveControl = loginSubmitButton;
			}
			catch (Exception ex){ 
				Core.ErrorLog.Log(String.Format("Error whilst decrypting stored password: {0} {1} {2}", ex.Message, ex.InnerException, ex.StackTrace));
				this.loginErrorLabel.Text = "Could not load any stored password; resetting to null..."; 
				Properties.Settings.Default.PWLen = 0;
				Properties.Settings.Default.PWEntropy = null;	
				Properties.Settings.Default.Save();
			}
		} //loginForm_Load

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
			this.characterSelectBox.DataSource = Account.LoginKey.characters;
			this.characterSelectBox.SelectedIndex = 0;
		}

		private void AdvancedLoginOptionsApplyButton_Click(object sender, EventArgs e)
		{
			Properties.Settings.Default.Port = int.Parse(this.portSelectionBox.Text);
			if (this.WSButton.Checked == true){Core.websocket = new WebSocketSharp.WebSocket(String.Format("ws://{0}:{1}", Properties.Settings.Default.Host, Properties.Settings.Default.Port));}
			else if (this.WSSButton.Checked == true){Core.websocket = new WebSocketSharp.WebSocket(String.Format("wss://{0}:{1}", Properties.Settings.Default.Host, Properties.Settings.Default.Port));}
			this.showAdvancedLoginButton_Click(sender, e);
			Console.WriteLine(String.Format("\tForm Value: {0} Type: {1}\n\tSettings value: {2} Type: {3}", this.portSelectionBox.Text, this.portSelectionBox.Text.GetType(), Properties.Settings.Default.Port, Properties.Settings.Default.Port.GetType()));
			Properties.Settings.Default.Save();
		}

		private void LoginForm_FormClosing(object sender, FormClosingEventArgs e){
			if (this.DialogResult == DialogResult.Cancel){ Application.Exit(); }
		}

/*		private void loginPasswordField_KeyUp(object sender, KeyEventArgs e)
 *		{
 *			char kc = (char)e.KeyCode;
 *			if (!char.IsLetterOrDigit(kc) && !char.IsPunctuation(kc) && !(new char[] { ' ', '!', '"', '£', '$', '%', '^', '&', '*', '(', ')', '_', '+', '=', '\'', '§' }.Contains<char>(kc)))
 *			{ 
 *				switch (e.KeyCode){
 *					case Keys.Return:
 *						loginSubmitButton_Click(null, null);
 *						break;
 *
 *					case Keys.Back | Keys.Delete:
 *						if (loginPasswordField.Text.Length > 0) { for (int i = 0; i < this.loginPasswordField.SelectionLength; i++) { password.RemoveAt(this.loginPasswordField.SelectionStart); } } //TODO test
 *						break;
 *				} //switch
 *			} //  isn't letter or digit
 *
 *			else{
 *				password.AppendChar((char)e.KeyCode);
 *				//this.loginPasswordField.Text.Remove(this.loginPasswordField.Text.Length);
 *				e.Handled = true;
 *			}
 *		} //KeyPress
 */
	}// class LoginForm
}
