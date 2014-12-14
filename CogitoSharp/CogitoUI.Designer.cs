namespace CogitoSharp
{
    partial class CogitoUI
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CogitoUI));
			this.chatTabs = new System.Windows.Forms.TabControl();
			this.tabPage1 = new System.Windows.Forms.TabPage();
			this.consoleTextBox = new System.Windows.Forms.TextBox();
			this.menuStrip1 = new System.Windows.Forms.MenuStrip();
			this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.saveLogAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.printToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.searchToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.selectAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
			this.sendButton = new System.Windows.Forms.Button();
			this.userListBox = new System.Windows.Forms.ListBox();
			this.ChatLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.loginPanel = new System.Windows.Forms.Panel();
			this.loginElements = new System.Windows.Forms.GroupBox();
			this.rememberPasswordCheck = new System.Windows.Forms.CheckBox();
			this.loginStatusLabel = new System.Windows.Forms.Label();
			this.PasswordLabel = new System.Windows.Forms.Label();
			this.UsernameLabel = new System.Windows.Forms.Label();
			this.loginPasswordField = new System.Windows.Forms.TextBox();
			this.loginSubmitButton = new System.Windows.Forms.Button();
			this.loginUserField = new System.Windows.Forms.TextBox();
			this.CogitoLogoBox = new System.Windows.Forms.PictureBox();
			this.characterSelectPanel = new System.Windows.Forms.Panel();
			this.characterSelectComboBox = new System.Windows.Forms.ComboBox();
			this.characterSelectButton = new System.Windows.Forms.Button();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.infoBarPanel = new System.Windows.Forms.Panel();
			this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
			this.currenctCharAvatar = new System.Windows.Forms.PictureBox();
			this.chatTabs.SuspendLayout();
			this.tabPage1.SuspendLayout();
			this.menuStrip1.SuspendLayout();
			this.ChatLayoutPanel.SuspendLayout();
			this.loginPanel.SuspendLayout();
			this.loginElements.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.CogitoLogoBox)).BeginInit();
			this.characterSelectPanel.SuspendLayout();
			this.tableLayoutPanel1.SuspendLayout();
			this.infoBarPanel.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.currenctCharAvatar)).BeginInit();
			this.SuspendLayout();
			// 
			// chatTabs
			// 
			this.chatTabs.Controls.Add(this.tabPage1);
			resources.ApplyResources(this.chatTabs, "chatTabs");
			this.chatTabs.Name = "chatTabs";
			this.chatTabs.SelectedIndex = 0;
			// 
			// tabPage1
			// 
			this.tabPage1.Controls.Add(this.loginElements);
			this.tabPage1.Controls.Add(this.consoleTextBox);
			resources.ApplyResources(this.tabPage1, "tabPage1");
			this.tabPage1.Name = "tabPage1";
			this.tabPage1.UseVisualStyleBackColor = true;
			// 
			// consoleTextBox
			// 
			resources.ApplyResources(this.consoleTextBox, "consoleTextBox");
			this.consoleTextBox.Name = "consoleTextBox";
			this.consoleTextBox.ReadOnly = true;
			// 
			// menuStrip1
			// 
			this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem,
            this.toolStripMenuItem1});
			resources.ApplyResources(this.menuStrip1, "menuStrip1");
			this.menuStrip1.Name = "menuStrip1";
			// 
			// fileToolStripMenuItem
			// 
			this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveLogAsToolStripMenuItem,
            this.printToolStripMenuItem,
            this.exitToolStripMenuItem});
			this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
			resources.ApplyResources(this.fileToolStripMenuItem, "fileToolStripMenuItem");
			// 
			// saveLogAsToolStripMenuItem
			// 
			this.saveLogAsToolStripMenuItem.Name = "saveLogAsToolStripMenuItem";
			resources.ApplyResources(this.saveLogAsToolStripMenuItem, "saveLogAsToolStripMenuItem");
			// 
			// printToolStripMenuItem
			// 
			this.printToolStripMenuItem.Name = "printToolStripMenuItem";
			resources.ApplyResources(this.printToolStripMenuItem, "printToolStripMenuItem");
			// 
			// exitToolStripMenuItem
			// 
			this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
			resources.ApplyResources(this.exitToolStripMenuItem, "exitToolStripMenuItem");
			// 
			// editToolStripMenuItem
			// 
			this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.searchToolStripMenuItem,
            this.selectAllToolStripMenuItem});
			this.editToolStripMenuItem.Name = "editToolStripMenuItem";
			resources.ApplyResources(this.editToolStripMenuItem, "editToolStripMenuItem");
			// 
			// searchToolStripMenuItem
			// 
			this.searchToolStripMenuItem.Name = "searchToolStripMenuItem";
			resources.ApplyResources(this.searchToolStripMenuItem, "searchToolStripMenuItem");
			// 
			// selectAllToolStripMenuItem
			// 
			this.selectAllToolStripMenuItem.Name = "selectAllToolStripMenuItem";
			resources.ApplyResources(this.selectAllToolStripMenuItem, "selectAllToolStripMenuItem");
			// 
			// toolStripMenuItem1
			// 
			this.toolStripMenuItem1.Name = "toolStripMenuItem1";
			resources.ApplyResources(this.toolStripMenuItem1, "toolStripMenuItem1");
			// 
			// sendButton
			// 
			resources.ApplyResources(this.sendButton, "sendButton");
			this.sendButton.Name = "sendButton";
			this.sendButton.UseVisualStyleBackColor = true;
			this.sendButton.Click += new System.EventHandler(this.sendButton_Click);
			// 
			// userListBox
			// 
			resources.ApplyResources(this.userListBox, "userListBox");
			this.userListBox.FormattingEnabled = true;
			this.userListBox.Name = "userListBox";
			// 
			// ChatLayoutPanel
			// 
			resources.ApplyResources(this.ChatLayoutPanel, "ChatLayoutPanel");
			this.ChatLayoutPanel.Controls.Add(this.sendButton, 1, 1);
			this.ChatLayoutPanel.Controls.Add(this.chatTabs, 0, 0);
			this.ChatLayoutPanel.Controls.Add(this.textBox1, 0, 1);
			this.ChatLayoutPanel.Controls.Add(this.userListBox, 1, 0);
			this.ChatLayoutPanel.Name = "ChatLayoutPanel";
			// 
			// textBox1
			// 
			resources.ApplyResources(this.textBox1, "textBox1");
			this.textBox1.Name = "textBox1";
			this.textBox1.TextChanged += new System.EventHandler(this.textBox1_GotFocus);
			// 
			// loginPanel
			// 
			this.loginPanel.Controls.Add(this.CogitoLogoBox);
			this.loginPanel.Controls.Add(this.tableLayoutPanel1);
			this.loginPanel.Controls.Add(this.characterSelectPanel);
			resources.ApplyResources(this.loginPanel, "loginPanel");
			this.loginPanel.Name = "loginPanel";
			this.loginPanel.VisibleChanged += new System.EventHandler(this.loginPanel_VisibleChanged);
			// 
			// loginElements
			// 
			this.loginElements.Controls.Add(this.rememberPasswordCheck);
			this.loginElements.Controls.Add(this.loginStatusLabel);
			this.loginElements.Controls.Add(this.PasswordLabel);
			this.loginElements.Controls.Add(this.UsernameLabel);
			this.loginElements.Controls.Add(this.loginPasswordField);
			this.loginElements.Controls.Add(this.loginSubmitButton);
			this.loginElements.Controls.Add(this.loginUserField);
			resources.ApplyResources(this.loginElements, "loginElements");
			this.loginElements.Name = "loginElements";
			this.loginElements.TabStop = false;
			// 
			// rememberPasswordCheck
			// 
			resources.ApplyResources(this.rememberPasswordCheck, "rememberPasswordCheck");
			this.rememberPasswordCheck.Name = "rememberPasswordCheck";
			this.rememberPasswordCheck.UseVisualStyleBackColor = true;
			this.rememberPasswordCheck.CheckedChanged += new System.EventHandler(this.rememberPasswordCheck_CheckedChanged);
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
			this.loginSubmitButton.Click += new System.EventHandler(this.loginButton_Click);
			// 
			// loginUserField
			// 
			this.loginUserField.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
			resources.ApplyResources(this.loginUserField, "loginUserField");
			this.loginUserField.Name = "loginUserField";
			this.loginUserField.Enter += new System.EventHandler(this.loginUser_Enter);
			// 
			// CogitoLogoBox
			// 
			resources.ApplyResources(this.CogitoLogoBox, "CogitoLogoBox");
			this.CogitoLogoBox.Name = "CogitoLogoBox";
			this.CogitoLogoBox.TabStop = false;
			// 
			// characterSelectPanel
			// 
			this.characterSelectPanel.Controls.Add(this.characterSelectButton);
			this.characterSelectPanel.Controls.Add(this.characterSelectComboBox);
			resources.ApplyResources(this.characterSelectPanel, "characterSelectPanel");
			this.characterSelectPanel.Name = "characterSelectPanel";
			// 
			// characterSelectComboBox
			// 
			this.characterSelectComboBox.FormattingEnabled = true;
			resources.ApplyResources(this.characterSelectComboBox, "characterSelectComboBox");
			this.characterSelectComboBox.Name = "characterSelectComboBox";
			// 
			// characterSelectButton
			// 
			resources.ApplyResources(this.characterSelectButton, "characterSelectButton");
			this.characterSelectButton.Name = "characterSelectButton";
			this.characterSelectButton.UseVisualStyleBackColor = true;
			// 
			// tableLayoutPanel1
			// 
			resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
			this.tableLayoutPanel1.Controls.Add(this.ChatLayoutPanel, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.infoBarPanel, 0, 0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			// 
			// infoBarPanel
			// 
			this.infoBarPanel.Controls.Add(this.currenctCharAvatar);
			resources.ApplyResources(this.infoBarPanel, "infoBarPanel");
			this.infoBarPanel.Name = "infoBarPanel";
			// 
			// notifyIcon1
			// 
			resources.ApplyResources(this.notifyIcon1, "notifyIcon1");
			// 
			// currenctCharAvatar
			// 
			resources.ApplyResources(this.currenctCharAvatar, "currenctCharAvatar");
			this.currenctCharAvatar.Name = "currenctCharAvatar";
			this.currenctCharAvatar.TabStop = false;
			// 
			// CogitoUI
			// 
			this.AcceptButton = this.loginSubmitButton;
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.loginPanel);
			this.Controls.Add(this.menuStrip1);
			this.MainMenuStrip = this.menuStrip1;
			this.Name = "CogitoUI";
			this.Load += new System.EventHandler(this.CogitoUI_Load);
			this.chatTabs.ResumeLayout(false);
			this.tabPage1.ResumeLayout(false);
			this.tabPage1.PerformLayout();
			this.menuStrip1.ResumeLayout(false);
			this.menuStrip1.PerformLayout();
			this.ChatLayoutPanel.ResumeLayout(false);
			this.ChatLayoutPanel.PerformLayout();
			this.loginPanel.ResumeLayout(false);
			this.loginElements.ResumeLayout(false);
			this.loginElements.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.CogitoLogoBox)).EndInit();
			this.characterSelectPanel.ResumeLayout(false);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.infoBarPanel.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.currenctCharAvatar)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        protected internal System.Windows.Forms.TabControl chatTabs;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem saveLogAsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem printToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem searchToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem selectAllToolStripMenuItem;
        protected internal System.Windows.Forms.Button sendButton;
        protected internal System.Windows.Forms.ListBox userListBox;
        protected internal System.Windows.Forms.TextBox consoleTextBox;
        private System.Windows.Forms.TableLayoutPanel ChatLayoutPanel;
        protected internal System.Windows.Forms.TextBox textBox1;
		private System.Windows.Forms.Panel loginPanel;
		private System.Windows.Forms.Label PasswordLabel;
		private System.Windows.Forms.Label UsernameLabel;
		private System.Windows.Forms.TextBox loginPasswordField;
		private System.Windows.Forms.Button loginSubmitButton;
		private System.Windows.Forms.TextBox loginUserField;
		private System.Windows.Forms.Label loginStatusLabel;
		private System.Windows.Forms.CheckBox rememberPasswordCheck;
		private System.Windows.Forms.PictureBox CogitoLogoBox;
		private System.Windows.Forms.GroupBox loginElements;
		private System.Windows.Forms.Panel characterSelectPanel;
		private System.Windows.Forms.Button characterSelectButton;
		private System.Windows.Forms.ComboBox characterSelectComboBox;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.Panel infoBarPanel;
		private System.Windows.Forms.PictureBox currenctCharAvatar;
		private System.Windows.Forms.NotifyIcon notifyIcon1;
    }
}

