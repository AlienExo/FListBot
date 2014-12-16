namespace CogitoSharp
{
	partial class loginForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(loginForm));
			this.CogitoLogoBox = new System.Windows.Forms.PictureBox();
			this.loginElements = new System.Windows.Forms.GroupBox();
			this.showAdvancedLoginButton = new System.Windows.Forms.Button();
			this.rememberPasswordCheck = new System.Windows.Forms.CheckBox();
			this.loginStatusLabel = new System.Windows.Forms.Label();
			this.PasswordLabel = new System.Windows.Forms.Label();
			this.UsernameLabel = new System.Windows.Forms.Label();
			this.loginPasswordField = new System.Windows.Forms.TextBox();
			this.loginSubmitButton = new System.Windows.Forms.Button();
			this.loginUserField = new System.Windows.Forms.TextBox();
			this.charSelectPanel = new System.Windows.Forms.Panel();
			this.charSelectBox = new System.Windows.Forms.ComboBox();
			this.charSelectButton = new System.Windows.Forms.Button();
			this.advancedLoginOptionsPanel = new System.Windows.Forms.Panel();
			this.AdvancedLoginOptionsApplyButton = new System.Windows.Forms.Button();
			this.radioButton2 = new System.Windows.Forms.RadioButton();
			this.radioButton1 = new System.Windows.Forms.RadioButton();
			((System.ComponentModel.ISupportInitialize)(this.CogitoLogoBox)).BeginInit();
			this.loginElements.SuspendLayout();
			this.charSelectPanel.SuspendLayout();
			this.advancedLoginOptionsPanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// CogitoLogoBox
			// 
			resources.ApplyResources(this.CogitoLogoBox, "CogitoLogoBox");
			this.CogitoLogoBox.Name = "CogitoLogoBox";
			this.CogitoLogoBox.TabStop = false;
			// 
			// loginElements
			// 
			this.loginElements.Controls.Add(this.showAdvancedLoginButton);
			this.loginElements.Controls.Add(this.rememberPasswordCheck);
			this.loginElements.Controls.Add(this.loginStatusLabel);
			this.loginElements.Controls.Add(this.PasswordLabel);
			this.loginElements.Controls.Add(this.UsernameLabel);
			this.loginElements.Controls.Add(this.loginPasswordField);
			this.loginElements.Controls.Add(this.loginSubmitButton);
			this.loginElements.Controls.Add(this.loginUserField);
			this.loginElements.Controls.Add(this.charSelectPanel);
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
			// loginStatusLabel
			// 
			resources.ApplyResources(this.loginStatusLabel, "loginStatusLabel");
			this.loginStatusLabel.Name = "loginStatusLabel";
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
			// charSelectPanel
			// 
			this.charSelectPanel.Controls.Add(this.charSelectBox);
			this.charSelectPanel.Controls.Add(this.charSelectButton);
			resources.ApplyResources(this.charSelectPanel, "charSelectPanel");
			this.charSelectPanel.Name = "charSelectPanel";
			this.charSelectPanel.VisibleChanged += new System.EventHandler(this.charSelectPanel_VisibleChanged);
			// 
			// charSelectBox
			// 
			this.charSelectBox.FormattingEnabled = true;
			resources.ApplyResources(this.charSelectBox, "charSelectBox");
			this.charSelectBox.Name = "charSelectBox";
			this.charSelectBox.Enter += new System.EventHandler(this.charSelectBox_Enter);
			// 
			// charSelectButton
			// 
			resources.ApplyResources(this.charSelectButton, "charSelectButton");
			this.charSelectButton.Name = "charSelectButton";
			this.charSelectButton.UseVisualStyleBackColor = true;
			this.charSelectButton.Click += new System.EventHandler(this.charSelectButton_Click);
			// 
			// advancedLoginOptionsPanel
			// 
			this.advancedLoginOptionsPanel.Controls.Add(this.AdvancedLoginOptionsApplyButton);
			this.advancedLoginOptionsPanel.Controls.Add(this.radioButton2);
			this.advancedLoginOptionsPanel.Controls.Add(this.radioButton1);
			resources.ApplyResources(this.advancedLoginOptionsPanel, "advancedLoginOptionsPanel");
			this.advancedLoginOptionsPanel.Name = "advancedLoginOptionsPanel";
			// 
			// AdvancedLoginOptionsApplyButton
			// 
			resources.ApplyResources(this.AdvancedLoginOptionsApplyButton, "AdvancedLoginOptionsApplyButton");
			this.AdvancedLoginOptionsApplyButton.Name = "AdvancedLoginOptionsApplyButton";
			this.AdvancedLoginOptionsApplyButton.UseVisualStyleBackColor = true;
			// 
			// radioButton2
			// 
			resources.ApplyResources(this.radioButton2, "radioButton2");
			this.radioButton2.Name = "radioButton2";
			this.radioButton2.TabStop = true;
			this.radioButton2.UseVisualStyleBackColor = true;
			// 
			// radioButton1
			// 
			resources.ApplyResources(this.radioButton1, "radioButton1");
			this.radioButton1.Name = "radioButton1";
			this.radioButton1.TabStop = true;
			this.radioButton1.UseVisualStyleBackColor = true;
			// 
			// loginForm
			// 
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.loginElements);
			this.Controls.Add(this.CogitoLogoBox);
			this.Controls.Add(this.advancedLoginOptionsPanel);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "loginForm";
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.loginForm_FormClosed);
			this.Load += new System.EventHandler(this.loginForm_Load);
			((System.ComponentModel.ISupportInitialize)(this.CogitoLogoBox)).EndInit();
			this.loginElements.ResumeLayout(false);
			this.loginElements.PerformLayout();
			this.charSelectPanel.ResumeLayout(false);
			this.advancedLoginOptionsPanel.ResumeLayout(false);
			this.advancedLoginOptionsPanel.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.PictureBox CogitoLogoBox;
		private System.Windows.Forms.GroupBox loginElements;
		private System.Windows.Forms.CheckBox rememberPasswordCheck;
		private System.Windows.Forms.Label loginStatusLabel;
		private System.Windows.Forms.Label PasswordLabel;
		private System.Windows.Forms.Label UsernameLabel;
		private System.Windows.Forms.TextBox loginPasswordField;
		private System.Windows.Forms.Button loginSubmitButton;
		private System.Windows.Forms.TextBox loginUserField;
		private System.Windows.Forms.Panel charSelectPanel;
		private System.Windows.Forms.ComboBox charSelectBox;
		private System.Windows.Forms.Button charSelectButton;
		private System.Windows.Forms.Button showAdvancedLoginButton;
		private System.Windows.Forms.Panel advancedLoginOptionsPanel;
		private System.Windows.Forms.Button AdvancedLoginOptionsApplyButton;
		private System.Windows.Forms.RadioButton radioButton2;
		private System.Windows.Forms.RadioButton radioButton1;
	}
}