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
			this.menuStrip1 = new System.Windows.Forms.MenuStrip();
			this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.saveLogAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.printToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.searchToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.selectAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
			this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.ChatLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
			this.sendButton = new System.Windows.Forms.Button();
			this.chatTabs = new System.Windows.Forms.TabControl();
			this.tabPage1 = new System.Windows.Forms.TabPage();
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.userListBox = new System.Windows.Forms.ListBox();
			this.infoBarPanel = new System.Windows.Forms.Panel();
			this.currenctCharAvatar = new System.Windows.Forms.PictureBox();
			this.menuStrip1.SuspendLayout();
			this.tableLayoutPanel1.SuspendLayout();
			this.ChatLayoutPanel.SuspendLayout();
			this.chatTabs.SuspendLayout();
			this.infoBarPanel.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.currenctCharAvatar)).BeginInit();
			this.SuspendLayout();
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
			// notifyIcon1
			// 
			resources.ApplyResources(this.notifyIcon1, "notifyIcon1");
			// 
			// tableLayoutPanel1
			// 
			resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
			this.tableLayoutPanel1.Controls.Add(this.ChatLayoutPanel, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.infoBarPanel, 0, 0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
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
			// sendButton
			// 
			resources.ApplyResources(this.sendButton, "sendButton");
			this.sendButton.Name = "sendButton";
			this.sendButton.UseVisualStyleBackColor = true;
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
			resources.ApplyResources(this.tabPage1, "tabPage1");
			this.tabPage1.Name = "tabPage1";
			this.tabPage1.UseVisualStyleBackColor = true;
			// 
			// textBox1
			// 
			resources.ApplyResources(this.textBox1, "textBox1");
			this.textBox1.Name = "textBox1";
			// 
			// userListBox
			// 
			resources.ApplyResources(this.userListBox, "userListBox");
			this.userListBox.FormattingEnabled = true;
			this.userListBox.Name = "userListBox";
			// 
			// infoBarPanel
			// 
			this.infoBarPanel.Controls.Add(this.currenctCharAvatar);
			resources.ApplyResources(this.infoBarPanel, "infoBarPanel");
			this.infoBarPanel.Name = "infoBarPanel";
			// 
			// currenctCharAvatar
			// 
			resources.ApplyResources(this.currenctCharAvatar, "currenctCharAvatar");
			this.currenctCharAvatar.Name = "currenctCharAvatar";
			this.currenctCharAvatar.TabStop = false;
			// 
			// CogitoUI
			// 
			this.AcceptButton = this.sendButton;
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.tableLayoutPanel1);
			this.Controls.Add(this.menuStrip1);
			this.MainMenuStrip = this.menuStrip1;
			this.Name = "CogitoUI";
			this.Load += new System.EventHandler(this.CogitoUI_Load);
			this.menuStrip1.ResumeLayout(false);
			this.menuStrip1.PerformLayout();
			this.tableLayoutPanel1.ResumeLayout(false);
			this.ChatLayoutPanel.ResumeLayout(false);
			this.ChatLayoutPanel.PerformLayout();
			this.chatTabs.ResumeLayout(false);
			this.infoBarPanel.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.currenctCharAvatar)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

		private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem saveLogAsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem printToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem searchToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem selectAllToolStripMenuItem;
		private System.Windows.Forms.NotifyIcon notifyIcon1;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.TableLayoutPanel ChatLayoutPanel;
		protected internal System.Windows.Forms.Button sendButton;
		protected internal System.Windows.Forms.TabControl chatTabs;
		private System.Windows.Forms.TabPage tabPage1;
		protected internal System.Windows.Forms.TextBox textBox1;
		protected internal System.Windows.Forms.ListBox userListBox;
		private System.Windows.Forms.Panel infoBarPanel;
		private System.Windows.Forms.PictureBox currenctCharAvatar;
    }
}

