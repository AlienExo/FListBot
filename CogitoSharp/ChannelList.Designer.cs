namespace CogitoSharp
{
	partial class ChannelList
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
			System.Windows.Forms.ListViewGroup listViewGroup1 = new System.Windows.Forms.ListViewGroup("Public Channels", System.Windows.Forms.HorizontalAlignment.Left);
			System.Windows.Forms.ListViewGroup listViewGroup2 = new System.Windows.Forms.ListViewGroup("Private Channels", System.Windows.Forms.HorizontalAlignment.Left);
			System.Windows.Forms.ListViewItem listViewItem1 = new System.Windows.Forms.ListViewItem("FART");
			this.listViewChannels = new System.Windows.Forms.ListView();
			this.columnHeaderChannelName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.columnHeaderUserAmount = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.columnHeaderChannelMode = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.SuspendLayout();
			// 
			// listViewChannels
			// 
			this.listViewChannels.Activation = System.Windows.Forms.ItemActivation.OneClick;
			this.listViewChannels.Alignment = System.Windows.Forms.ListViewAlignment.SnapToGrid;
			this.listViewChannels.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderChannelName,
            this.columnHeaderUserAmount,
            this.columnHeaderChannelMode});
			this.listViewChannels.Dock = System.Windows.Forms.DockStyle.Fill;
			this.listViewChannels.FullRowSelect = true;
			this.listViewChannels.GridLines = true;
			listViewGroup1.Header = "Public Channels";
			listViewGroup1.Name = "listViewGroupPublicChannels";
			listViewGroup2.Header = "Private Channels";
			listViewGroup2.Name = "listViewGroupPrivateChannels";
			this.listViewChannels.Groups.AddRange(new System.Windows.Forms.ListViewGroup[] {
            listViewGroup1,
            listViewGroup2});
			this.listViewChannels.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
			this.listViewChannels.HideSelection = false;
			this.listViewChannels.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem1});
			this.listViewChannels.Location = new System.Drawing.Point(0, 0);
			this.listViewChannels.Name = "listViewChannels";
			this.listViewChannels.ShowItemToolTips = true;
			this.listViewChannels.Size = new System.Drawing.Size(384, 662);
			this.listViewChannels.Sorting = System.Windows.Forms.SortOrder.Ascending;
			this.listViewChannels.TabIndex = 0;
			this.listViewChannels.UseCompatibleStateImageBehavior = false;
			this.listViewChannels.View = System.Windows.Forms.View.Tile;
			this.listViewChannels.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.listViewChannels_ItemChecked);
			// 
			// columnHeaderChannelName
			// 
			this.columnHeaderChannelName.Text = "Channel Name";
			this.columnHeaderChannelName.Width = 91;
			// 
			// columnHeaderUserAmount
			// 
			this.columnHeaderUserAmount.Text = "Users";
			// 
			// columnHeaderChannelMode
			// 
			this.columnHeaderChannelMode.Text = "Channel Mode";
			this.columnHeaderChannelMode.Width = 82;
			// 
			// ChannelList
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(384, 662);
			this.Controls.Add(this.listViewChannels);
			this.Name = "ChannelList";
			this.Text = "ChannelList";
			this.Enter += new System.EventHandler(this.ChannelList_Enter);
			this.ResumeLayout(false);

		}

		#endregion

		protected internal System.Windows.Forms.ListView listViewChannels;
		private System.Windows.Forms.ColumnHeader columnHeaderChannelName;
		private System.Windows.Forms.ColumnHeader columnHeaderUserAmount;
		private System.Windows.Forms.ColumnHeader columnHeaderChannelMode;

	}
}