namespace CogitoSharp
{
	partial class LoginForm
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LoginForm));
			this.CogitoLogoBox = new System.Windows.Forms.PictureBox();
			this.loginElements = new System.Windows.Forms.GroupBox();
			this.showAdvancedLoginButton = new System.Windows.Forms.Button();
			this.rememberPasswordCheck = new System.Windows.Forms.CheckBox();
			this.PasswordLabel = new System.Windows.Forms.Label();
			this.UsernameLabel = new System.Windows.Forms.Label();
			this.loginErrorLabel = new System.Windows.Forms.Label();
			this.loginPasswordField = new System.Windows.Forms.TextBox();
			this.loginSubmitButton = new System.Windows.Forms.Button();
			this.loginUserField = new System.Windows.Forms.TextBox();
			this.characterSelectionGroup = new System.Windows.Forms.GroupBox();
			this.characterSelectButton = new System.Windows.Forms.Button();
			this.characterSelectBox = new System.Windows.Forms.ComboBox();
			this.advancedLoginOptionsPanel = new System.Windows.Forms.Panel();
			this.protocolSelectionBox = new System.Windows.Forms.GroupBox();
			this.portSelectionBox = new System.Windows.Forms.MaskedTextBox();
			this.portLabel = new System.Windows.Forms.Label();
			this.WSSButton = new System.Windows.Forms.RadioButton();
			this.WSButton = new System.Windows.Forms.RadioButton();
			this.AdvancedLoginOptionsApplyButton = new System.Windows.Forms.Button();
			this.characterSelectPanel = new System.Windows.Forms.Panel();
			this.accountBindingSource = new System.Windows.Forms.BindingSource(this.components);
			((System.ComponentModel.ISupportInitialize)(this.CogitoLogoBox)).BeginInit();
			this.loginElements.SuspendLayout();
			this.characterSelectionGroup.SuspendLayout();
			this.advancedLoginOptionsPanel.SuspendLayout();
			this.protocolSelectionBox.SuspendLayout();
			this.characterSelectPanel.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.accountBindingSource)).BeginInit();
			this.SuspendLayout();
			// 
			// CogitoLogoBox
			// 
			this.CogitoLogoBox.BackColor = System.Drawing.Color.Transparent;
			resources.ApplyResources(this.CogitoLogoBox, "CogitoLogoBox");
			this.CogitoLogoBox.Name = "CogitoLogoBox";
			this.CogitoLogoBox.TabStop = false;
			// 
			// loginElements
			// 
			this.loginElements.Controls.Add(this.showAdvancedLoginButton);
			this.loginElements.Controls.Add(this.rememberPasswordCheck);
			this.loginElements.Controls.Add(this.PasswordLabel);
			this.loginElements.Controls.Add(this.UsernameLabel);
			this.loginElements.Controls.Add(this.loginErrorLabel);
			this.loginElements.Controls.Add(this.loginPasswordField);
			this.loginElements.Controls.Add(this.loginSubmitButton);
			this.loginElements.Controls.Add(this.loginUserField);
			resources.ApplyResources(this.loginElements, "loginElements");
			this.loginElements.Name = "loginElements";
			this.loginElements.TabStop = false;
			// 
			// showAdvancedLoginButton
			// 
			resources.ApplyResources(this.showAdvancedLoginButton, "showAdvancedLoginButton");
			this.showAdvancedLoginButton.Name = "showAdvancedLoginButton";
			this.showAdvancedLoginButton.UseVisualStyleBackColor = true;
			this.showAdvancedLoginButton.Click += new System.EventHandler(this.showAdvancedLoginButton_Click);
			// 
			// rememberPasswordCheck
			// 
			resources.ApplyResources(this.rememberPasswordCheck, "rememberPasswordCheck");
			this.rememberPasswordCheck.Name = "rememberPasswordCheck";
			this.rememberPasswordCheck.UseVisualStyleBackColor = true;
			// 
			// PasswordLabel
			// 
			resources.ApplyResources(this.PasswordLabel, "PasswordLabel");
			this.PasswordLabel.Name = "PasswordLabel";
			// 
			// UsernameLabel
			// 
			resources.ApplyResources(this.UsernameLabel, "UsernameLabel");
			this.UsernameLabel.Name = "UsernameLabel";
			// 
			// loginErrorLabel
			// 
			resources.ApplyResources(this.loginErrorLabel, "loginErrorLabel");
			this.loginErrorLabel.ForeColor = System.Drawing.Color.Red;
			this.loginErrorLabel.Name = "loginErrorLabel";
			// 
			// loginPasswordField
			// 
			resources.ApplyResources(this.loginPasswordField, "loginPasswordField");
			this.loginPasswordField.Name = "loginPasswordField";
			this.loginPasswordField.UseSystemPasswordChar = true;
			// 
			// loginSubmitButton
			// 
			resources.ApplyResources(this.loginSubmitButton, "loginSubmitButton");
			this.loginSubmitButton.Name = "loginSubmitButton";
			this.loginSubmitButton.UseVisualStyleBackColor = true;
			this.loginSubmitButton.Click += new System.EventHandler(this.loginSubmitButton_Click);
			// 
			// loginUserField
			// 
			this.loginUserField.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
			resources.ApplyResources(this.loginUserField, "loginUserField");
			this.loginUserField.Name = "loginUserField";
			// 
			// characterSelectionGroup
			// 
			this.characterSelectionGroup.Controls.Add(this.characterSelectButton);
			this.characterSelectionGroup.Controls.Add(this.characterSelectBox);
			resources.ApplyResources(this.characterSelectionGroup, "characterSelectionGroup");
			this.characterSelectionGroup.Name = "characterSelectionGroup";
			this.characterSelectionGroup.TabStop = false;
			// 
			// characterSelectButton
			// 
			resources.ApplyResources(this.characterSelectButton, "characterSelectButton");
			this.characterSelectButton.Name = "characterSelectButton";
			this.characterSelectButton.Click += new System.EventHandler(this.characterSelectButton_Click);
			// 
			// characterSelectBox
			// 
			this.characterSelectBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			resources.ApplyResources(this.characterSelectBox, "characterSelectBox");
			this.characterSelectBox.Name = "characterSelectBox";
			this.characterSelectBox.EnabledChanged += new System.EventHandler(this.characterSelectBox_EnabledChanged);
			// 
			// advancedLoginOptionsPanel
			// 
			this.advancedLoginOptionsPanel.Controls.Add(this.protocolSelectionBox);
			this.advancedLoginOptionsPanel.Controls.Add(this.AdvancedLoginOptionsApplyButton);
			resources.ApplyResources(this.advancedLoginOptionsPanel, "advancedLoginOptionsPanel");
			this.advancedLoginOptionsPanel.Name = "advancedLoginOptionsPanel";
			// 
			// protocolSelectionBox
			// 
			this.protocolSelectionBox.Controls.Add(this.portSelectionBox);
			this.protocolSelectionBox.Controls.Add(this.portLabel);
			this.protocolSelectionBox.Controls.Add(this.WSSButton);
			this.protocolSelectionBox.Controls.Add(this.WSButton);
			resources.ApplyResources(this.protocolSelectionBox, "protocolSelectionBox");
			this.protocolSelectionBox.Name = "protocolSelectionBox";
			this.protocolSelectionBox.TabStop = false;
			// 
			// portSelectionBox
			// 
			resources.ApplyResources(this.portSelectionBox, "portSelectionBox");
			this.portSelectionBox.Name = "portSelectionBox";
			// 
			// portLabel
			// 
			resources.ApplyResources(this.portLabel, "portLabel");
			this.portLabel.Name = "portLabel";
			// 
			// WSSButton
			// 
			resources.ApplyResources(this.WSSButton, "WSSButton");
			this.WSSButton.Name = "WSSButton";
			this.WSSButton.UseVisualStyleBackColor = true;
			// 
			// WSButton
			// 
			resources.ApplyResources(this.WSButton, "WSButton");
			this.WSButton.Checked = true;
			this.WSButton.Name = "WSButton";
			this.WSButton.TabStop = true;
			this.WSButton.UseVisualStyleBackColor = true;
			// 
			// AdvancedLoginOptionsApplyButton
			// 
			resources.ApplyResources(this.AdvancedLoginOptionsApplyButton, "AdvancedLoginOptionsApplyButton");
			this.AdvancedLoginOptionsApplyButton.Name = "AdvancedLoginOptionsApplyButton";
			this.AdvancedLoginOptionsApplyButton.UseVisualStyleBackColor = true;
			this.AdvancedLoginOptionsApplyButton.Click += new System.EventHandler(this.AdvancedLoginOptionsApplyButton_Click);
			// 
			// characterSelectPanel
			// 
			this.characterSelectPanel.Controls.Add(this.characterSelectionGroup);
			resources.ApplyResources(this.characterSelectPanel, "characterSelectPanel");
			this.characterSelectPanel.Name = "characterSelectPanel";
			// 
			// accountBindingSource
			// 
			this.accountBindingSource.DataSource = typeof(CogitoSharp.Account);
			// 
			// LoginForm
			// 
			this.AcceptButton = this.loginSubmitButton;
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.loginElements);
			this.Controls.Add(this.CogitoLogoBox);
			this.Controls.Add(this.advancedLoginOptionsPanel);
			this.Controls.Add(this.characterSelectPanel);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "LoginForm";
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.LoginForm_FormClosing);
			this.Load += new System.EventHandler(this.loginForm_Load);
			((System.ComponentModel.ISupportInitialize)(this.CogitoLogoBox)).EndInit();
			this.loginElements.ResumeLayout(false);
			this.loginElements.PerformLayout();
			this.characterSelectionGroup.ResumeLayout(false);
			this.advancedLoginOptionsPanel.ResumeLayout(false);
			this.protocolSelectionBox.ResumeLayout(false);
			this.protocolSelectionBox.PerformLayout();
			this.characterSelectPanel.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.accountBindingSource)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.PictureBox CogitoLogoBox;
		private System.Windows.Forms.GroupBox loginElements;
		private System.Windows.Forms.CheckBox rememberPasswordCheck;
		private System.Windows.Forms.Label PasswordLabel;
		private System.Windows.Forms.Label UsernameLabel;
		private System.Windows.Forms.TextBox loginPasswordField;
		private System.Windows.Forms.Button loginSubmitButton;
		private System.Windows.Forms.TextBox loginUserField;
		private System.Windows.Forms.Button showAdvancedLoginButton;
		private System.Windows.Forms.Panel advancedLoginOptionsPanel;
		private System.Windows.Forms.Button AdvancedLoginOptionsApplyButton;
		private System.Windows.Forms.RadioButton WSSButton;
		private System.Windows.Forms.RadioButton WSButton;
		private System.Windows.Forms.Label loginErrorLabel;
		private System.Windows.Forms.GroupBox characterSelectionGroup;
		private System.Windows.Forms.Button characterSelectButton;
		private System.Windows.Forms.ComboBox characterSelectBox;
		private System.Windows.Forms.GroupBox protocolSelectionBox;
		private System.Windows.Forms.BindingSource accountBindingSource;
		private System.Windows.Forms.Panel characterSelectPanel;
		private System.Windows.Forms.Label portLabel;
		private System.Windows.Forms.MaskedTextBox portSelectionBox;
	}
}