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
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.splitContainer2 = new System.Windows.Forms.SplitContainer();
			this.userListBox = new System.Windows.Forms.ListBox();
			this.sendButton = new System.Windows.Forms.Button();
			this.mainTextBox = new System.Windows.Forms.TextBox();
			this.infoBarPanel = new System.Windows.Forms.Panel();
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.currenctCharAvatar = new System.Windows.Forms.PictureBox();
			this.CoreUITable = new System.Windows.Forms.TableLayoutPanel();
			this.chatTabs = new CogitoSharp.ChatTabControl();
			this.tabPage1 = new System.Windows.Forms.TabPage();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
			this.splitContainer2.Panel1.SuspendLayout();
			this.splitContainer2.Panel2.SuspendLayout();
			this.splitContainer2.SuspendLayout();
			this.infoBarPanel.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.currenctCharAvatar)).BeginInit();
			this.CoreUITable.SuspendLayout();
			this.chatTabs.SuspendLayout();
			this.SuspendLayout();
			// 
			// splitContainer1
			// 
			this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer1.Location = new System.Drawing.Point(3, 69);
			this.splitContainer1.Name = "splitContainer1";
			this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.Controls.Add(this.splitContainer2);
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.Controls.Add(this.sendButton);
			this.splitContainer1.Panel2.Controls.Add(this.mainTextBox);
			this.splitContainer1.Size = new System.Drawing.Size(618, 370);
			this.splitContainer1.SplitterDistance = 300;
			this.splitContainer1.TabIndex = 5;
			// 
			// splitContainer2
			// 
			this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer2.Location = new System.Drawing.Point(0, 0);
			this.splitContainer2.Name = "splitContainer2";
			// 
			// splitContainer2.Panel1
			// 
			this.splitContainer2.Panel1.Controls.Add(this.chatTabs);
			// 
			// splitContainer2.Panel2
			// 
			this.splitContainer2.Panel2.Controls.Add(this.userListBox);
			this.splitContainer2.Size = new System.Drawing.Size(618, 300);
			this.splitContainer2.SplitterDistance = 444;
			this.splitContainer2.TabIndex = 0;
			// 
			// userListBox
			// 
			this.userListBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.userListBox.FormattingEnabled = true;
			this.userListBox.Location = new System.Drawing.Point(0, 0);
			this.userListBox.Name = "userListBox";
			this.userListBox.Size = new System.Drawing.Size(170, 300);
			this.userListBox.TabIndex = 3;
			// 
			// sendButton
			// 
			this.sendButton.Dock = System.Windows.Forms.DockStyle.Right;
			this.sendButton.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.sendButton.Location = new System.Drawing.Point(537, 0);
			this.sendButton.Name = "sendButton";
			this.sendButton.Size = new System.Drawing.Size(81, 66);
			this.sendButton.TabIndex = 5;
			this.sendButton.Text = "Send";
			this.sendButton.UseVisualStyleBackColor = true;
			// 
			// mainTextBox
			// 
			this.mainTextBox.Dock = System.Windows.Forms.DockStyle.Left;
			this.mainTextBox.Location = new System.Drawing.Point(0, 0);
			this.mainTextBox.Multiline = true;
			this.mainTextBox.Name = "mainTextBox";
			this.mainTextBox.Size = new System.Drawing.Size(531, 66);
			this.mainTextBox.TabIndex = 4;
			// 
			// infoBarPanel
			// 
			this.infoBarPanel.Controls.Add(this.label2);
			this.infoBarPanel.Controls.Add(this.label1);
			this.infoBarPanel.Controls.Add(this.currenctCharAvatar);
			this.infoBarPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.infoBarPanel.Location = new System.Drawing.Point(3, 3);
			this.infoBarPanel.MaximumSize = new System.Drawing.Size(0, 56);
			this.infoBarPanel.Name = "infoBarPanel";
			this.infoBarPanel.Size = new System.Drawing.Size(618, 56);
			this.infoBarPanel.TabIndex = 1;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(59, 22);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(35, 13);
			this.label2.TabIndex = 2;
			this.label2.Text = "label2";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(59, 6);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(35, 13);
			this.label1.TabIndex = 1;
			this.label1.Text = "label1";
			// 
			// currenctCharAvatar
			// 
			this.currenctCharAvatar.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.currenctCharAvatar.Location = new System.Drawing.Point(3, 3);
			this.currenctCharAvatar.Name = "currenctCharAvatar";
			this.currenctCharAvatar.Size = new System.Drawing.Size(50, 50);
			this.currenctCharAvatar.TabIndex = 0;
			this.currenctCharAvatar.TabStop = false;
			// 
			// CoreUITable
			// 
			this.CoreUITable.AutoSize = true;
			this.CoreUITable.ColumnCount = 1;
			this.CoreUITable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.CoreUITable.Controls.Add(this.infoBarPanel, 0, 0);
			this.CoreUITable.Controls.Add(this.splitContainer1, 0, 1);
			this.CoreUITable.Dock = System.Windows.Forms.DockStyle.Fill;
			this.CoreUITable.Location = new System.Drawing.Point(0, 0);
			this.CoreUITable.Name = "CoreUITable";
			this.CoreUITable.RowCount = 2;
			this.CoreUITable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 15F));
			this.CoreUITable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 85F));
			this.CoreUITable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.CoreUITable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.CoreUITable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.CoreUITable.Size = new System.Drawing.Size(624, 442);
			this.CoreUITable.TabIndex = 12;
			// 
			// chatTabs
			// 
			this.chatTabs.Controls.Add(this.tabPage1);
			this.chatTabs.Dock = System.Windows.Forms.DockStyle.Fill;
			this.chatTabs.Location = new System.Drawing.Point(0, 0);
			this.chatTabs.Name = "chatTabs";
			this.chatTabs.SelectedIndex = 0;
			this.chatTabs.Size = new System.Drawing.Size(444, 300);
			this.chatTabs.SizeMode = System.Windows.Forms.TabSizeMode.FillToRight;
			this.chatTabs.TabIndex = 2;
			// 
			// tabPage1
			// 
			this.tabPage1.Location = new System.Drawing.Point(4, 22);
			this.tabPage1.Name = "tabPage1";
			this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage1.Size = new System.Drawing.Size(436, 274);
			this.tabPage1.TabIndex = 0;
			this.tabPage1.Text = "Console";
			this.tabPage1.UseVisualStyleBackColor = true;
			// 
			// ChatUI
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(624, 442);
			this.Controls.Add(this.CoreUITable);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MinimumSize = new System.Drawing.Size(640, 480);
			this.Name = "ChatUI";
			this.Text = "ChatUI";
			this.Load += new System.EventHandler(this.ChatUI_Load);
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel2.ResumeLayout(false);
			this.splitContainer1.Panel2.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
			this.splitContainer1.ResumeLayout(false);
			this.splitContainer2.Panel1.ResumeLayout(false);
			this.splitContainer2.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
			this.splitContainer2.ResumeLayout(false);
			this.infoBarPanel.ResumeLayout(false);
			this.infoBarPanel.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.currenctCharAvatar)).EndInit();
			this.CoreUITable.ResumeLayout(false);
			this.chatTabs.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.SplitContainer splitContainer1;
		private System.Windows.Forms.SplitContainer splitContainer2;
		protected internal ChatTabControl chatTabs;
		private System.Windows.Forms.TabPage tabPage1;
		protected internal System.Windows.Forms.ListBox userListBox;
		protected internal System.Windows.Forms.Button sendButton;
		protected internal System.Windows.Forms.TextBox mainTextBox;
		private System.Windows.Forms.Panel infoBarPanel;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.PictureBox currenctCharAvatar;
		private System.Windows.Forms.TableLayoutPanel CoreUITable;

	}
}