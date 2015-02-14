namespace CogitoSharp
{
	partial class ChatUI
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ChatUI));
			this.CoreUITable = new System.Windows.Forms.TableLayoutPanel();
			this.ChatLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
			this.sendButton = new System.Windows.Forms.Button();
			this.chatTabs = new CogitoSharp.ChatTabControl();
			this.tabPage1 = new System.Windows.Forms.TabPage();
			this.mainTextBox = new System.Windows.Forms.TextBox();
			this.userListBox = new System.Windows.Forms.ListBox();
			this.infoBarPanel = new System.Windows.Forms.Panel();
			this.currenctCharAvatar = new System.Windows.Forms.PictureBox();
			this.CoreUITable.SuspendLayout();
			this.ChatLayoutPanel.SuspendLayout();
			this.chatTabs.SuspendLayout();
			this.infoBarPanel.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.currenctCharAvatar)).BeginInit();
			this.SuspendLayout();
			// 
			// CoreUITable
			// 
			this.CoreUITable.ColumnCount = 1;
			this.CoreUITable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.CoreUITable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.CoreUITable.Controls.Add(this.ChatLayoutPanel, 0, 1);
			this.CoreUITable.Controls.Add(this.infoBarPanel, 0, 0);
			this.CoreUITable.Location = new System.Drawing.Point(0, 0);
			this.CoreUITable.Name = "CoreUITable";
			this.CoreUITable.RowCount = 2;
			this.CoreUITable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 15F));
			this.CoreUITable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 85F));
			this.CoreUITable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.CoreUITable.Size = new System.Drawing.Size(621, 416);
			this.CoreUITable.TabIndex = 12;
			// 
			// ChatLayoutPanel
			// 
			this.ChatLayoutPanel.ColumnCount = 2;
			this.ChatLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 85F));
			this.ChatLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 15F));
			this.ChatLayoutPanel.Controls.Add(this.sendButton, 1, 1);
			this.ChatLayoutPanel.Controls.Add(this.chatTabs, 0, 0);
			this.ChatLayoutPanel.Controls.Add(this.mainTextBox, 0, 1);
			this.ChatLayoutPanel.Controls.Add(this.userListBox, 1, 0);
			this.ChatLayoutPanel.Location = new System.Drawing.Point(3, 65);
			this.ChatLayoutPanel.Name = "ChatLayoutPanel";
			this.ChatLayoutPanel.RowCount = 2;
			this.ChatLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 80F));
			this.ChatLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
			this.ChatLayoutPanel.Size = new System.Drawing.Size(615, 348);
			this.ChatLayoutPanel.TabIndex = 0;
			// 
			// sendButton
			// 
			this.sendButton.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.sendButton.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.sendButton.Location = new System.Drawing.Point(528, 292);
			this.sendButton.Name = "sendButton";
			this.sendButton.Size = new System.Drawing.Size(81, 42);
			this.sendButton.TabIndex = 3;
			this.sendButton.Text = "Send";
			this.sendButton.UseVisualStyleBackColor = true;
			// 
			// chatTabs
			// 
			this.chatTabs.Controls.Add(this.tabPage1);
			this.chatTabs.Location = new System.Drawing.Point(3, 3);
			this.chatTabs.Name = "chatTabs";
			this.chatTabs.SelectedIndex = 0;
			this.chatTabs.Size = new System.Drawing.Size(516, 272);
			this.chatTabs.SizeMode = System.Windows.Forms.TabSizeMode.FillToRight;
			this.chatTabs.TabIndex = 0;
			this.chatTabs.DoubleClick += new System.EventHandler(this.chatTabs_DoubleClick);
			// 
			// tabPage1
			// 
			this.tabPage1.Location = new System.Drawing.Point(4, 22);
			this.tabPage1.Name = "tabPage1";
			this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage1.Size = new System.Drawing.Size(508, 246);
			this.tabPage1.TabIndex = 0;
			this.tabPage1.Text = "Console";
			this.tabPage1.UseVisualStyleBackColor = true;
			// 
			// mainTextBox
			// 
			this.mainTextBox.Location = new System.Drawing.Point(3, 281);
			this.mainTextBox.Multiline = true;
			this.mainTextBox.Name = "mainTextBox";
			this.mainTextBox.Size = new System.Drawing.Size(516, 64);
			this.mainTextBox.TabIndex = 2;
			// 
			// userListBox
			// 
			this.userListBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.userListBox.FormattingEnabled = true;
			this.userListBox.Location = new System.Drawing.Point(527, 11);
			this.userListBox.Name = "userListBox";
			this.userListBox.Size = new System.Drawing.Size(85, 264);
			this.userListBox.TabIndex = 0;
			// 
			// infoBarPanel
			// 
			this.infoBarPanel.Controls.Add(this.currenctCharAvatar);
			this.infoBarPanel.Location = new System.Drawing.Point(3, 3);
			this.infoBarPanel.Name = "infoBarPanel";
			this.infoBarPanel.Size = new System.Drawing.Size(615, 56);
			this.infoBarPanel.TabIndex = 1;
			// 
			// currenctCharAvatar
			// 
			this.currenctCharAvatar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
			this.currenctCharAvatar.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.currenctCharAvatar.Location = new System.Drawing.Point(3, 3);
			this.currenctCharAvatar.Name = "currenctCharAvatar";
			this.currenctCharAvatar.Size = new System.Drawing.Size(50, 50);
			this.currenctCharAvatar.TabIndex = 0;
			this.currenctCharAvatar.TabStop = false;
			// 
			// ChatUI
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(624, 442);
			this.Controls.Add(this.CoreUITable);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "ChatUI";
			this.Text = "ChatUI";
			this.Load += new System.EventHandler(this.ChatUI_Load);
			this.CoreUITable.ResumeLayout(false);
			this.ChatLayoutPanel.ResumeLayout(false);
			this.ChatLayoutPanel.PerformLayout();
			this.chatTabs.ResumeLayout(false);
			this.infoBarPanel.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.currenctCharAvatar)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel CoreUITable;
		private System.Windows.Forms.TableLayoutPanel ChatLayoutPanel;
		protected internal System.Windows.Forms.Button sendButton;
		private System.Windows.Forms.TabPage tabPage1;
		protected internal System.Windows.Forms.TextBox mainTextBox;
		protected internal System.Windows.Forms.ListBox userListBox;
		private System.Windows.Forms.Panel infoBarPanel;
		private System.Windows.Forms.PictureBox currenctCharAvatar;
		protected internal ChatTabControl chatTabs;
	}
}