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
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.loginPanel = new System.Windows.Forms.Panel();
			this.rememberPasswordCheck = new System.Windows.Forms.CheckBox();
			this.loginStatusLabel = new System.Windows.Forms.Label();
			this.PasswordLabel = new System.Windows.Forms.Label();
			this.UsernameLabel = new System.Windows.Forms.Label();
			this.loginPassword = new System.Windows.Forms.TextBox();
			this.loginButton = new System.Windows.Forms.Button();
			this.loginUser = new System.Windows.Forms.TextBox();
			this.chatTabs.SuspendLayout();
			this.tabPage1.SuspendLayout();
			this.menuStrip1.SuspendLayout();
			this.tableLayoutPanel1.SuspendLayout();
			this.loginPanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// chatTabs
			// 
			this.chatTabs.Controls.Add(this.tabPage1);
			resources.ApplyResources(this.chatTabs, "chatTabs");
			this.chatTabs.Name = "chatTabs";
			this.chatTabs.SelectedIndex = 0;
			this.chatTabs.Selected += new System.Windows.Forms.TabControlEventHandler(this.chatTabs_Selected);
			// 
			// tabPage1
			// 
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
			// tableLayoutPanel1
			// 
			resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
			this.tableLayoutPanel1.Controls.Add(this.sendButton, 1, 1);
			this.tableLayoutPanel1.Controls.Add(this.userListBox, 1, 0);
			this.tableLayoutPanel1.Controls.Add(this.textBox1, 0, 1);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			// 
			// textBox1
			// 
			resources.ApplyResources(this.textBox1, "textBox1");
			this.textBox1.Name = "textBox1";
			this.textBox1.TextChanged += new System.EventHandler(this.textBox1_GotFocus);
			// 
			// loginPanel
			// 
			this.loginPanel.Controls.Add(this.rememberPasswordCheck);
			this.loginPanel.Controls.Add(this.loginStatusLabel);
			this.loginPanel.Controls.Add(this.PasswordLabel);
			this.loginPanel.Controls.Add(this.UsernameLabel);
			this.loginPanel.Controls.Add(this.loginPassword);
			this.loginPanel.Controls.Add(this.loginButton);
			this.loginPanel.Controls.Add(this.loginUser);
			resources.ApplyResources(this.loginPanel, "loginPanel");
			this.loginPanel.Name = "loginPanel";
			this.loginPanel.VisibleChanged += new System.EventHandler(this.loginPanel_VisibleChanged);
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
			// loginPassword
			// 
			resources.ApplyResources(this.loginPassword, "loginPassword");
			this.loginPassword.Name = "loginPassword";
			this.loginPassword.UseSystemPasswordChar = true;
			this.loginPassword.VisibleChanged += new System.EventHandler(this.loginPassword_VisibleChanged);
			// 
			// loginButton
			// 
			resources.ApplyResources(this.loginButton, "loginButton");
			this.loginButton.Name = "loginButton";
			this.loginButton.UseVisualStyleBackColor = true;
			this.loginButton.Click += new System.EventHandler(this.loginButton_Click);
			// 
			// loginUser
			// 
			resources.ApplyResources(this.loginUser, "loginUser");
			this.loginUser.Name = "loginUser";
			this.loginUser.VisibleChanged += new System.EventHandler(this.loginUser_VisibleChanged);
			// 
			// CogitoUI
			// 
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.loginPanel);
			this.Controls.Add(this.chatTabs);
			this.Controls.Add(this.tableLayoutPanel1);
			this.Controls.Add(this.menuStrip1);
			this.MainMenuStrip = this.menuStrip1;
			this.Name = "CogitoUI";
			this.chatTabs.ResumeLayout(false);
			this.tabPage1.ResumeLayout(false);
			this.tabPage1.PerformLayout();
			this.menuStrip1.ResumeLayout(false);
			this.menuStrip1.PerformLayout();
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			this.loginPanel.ResumeLayout(false);
			this.loginPanel.PerformLayout();
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
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        protected internal System.Windows.Forms.TextBox textBox1;
		private System.Windows.Forms.Panel loginPanel;
		private System.Windows.Forms.Label PasswordLabel;
		private System.Windows.Forms.Label UsernameLabel;
		private System.Windows.Forms.TextBox loginPassword;
		private System.Windows.Forms.Button loginButton;
		private System.Windows.Forms.TextBox loginUser;
		private System.Windows.Forms.Label loginStatusLabel;
		private System.Windows.Forms.CheckBox rememberPasswordCheck;
    }
}

