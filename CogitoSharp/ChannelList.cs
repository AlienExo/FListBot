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
	public partial class ChannelList : Form
	{
		public ChannelList(){
			InitializeComponent();
			RegenerateChannelList();
		}

		private void ChannelList_Enter(object sender, EventArgs e){ 
			Core.SystemLog.Log("Entering channel list; attempting regeneration...");
			RegenerateChannelList(); 
		}

		private void RegenerateChannelList(){
			this.SuspendLayout();
			//this.listViewChannels.Groups.Clear();
			this.listViewChannels.Items.Clear();
			ListViewGroup JoinedChannels = new ListViewGroup("Joined Channels");
			ListViewGroup FavChannels = new ListViewGroup("Favorite Channels");
			ListViewGroup PrivateChannels = new ListViewGroup("Private Channels");
			ListViewGroup PublicChannels = new ListViewGroup("Public Channels");
			this.listViewChannels.Groups.Add(JoinedChannels);
			this.listViewChannels.Groups.Add(FavChannels);
			this.listViewChannels.Groups.Add(PublicChannels);
			this.listViewChannels.Groups.Add(PrivateChannels);
			if (Core.channels.Count < 1) { return; }
			foreach (Channel c in Core.channels){
				ListViewItem lvi = new ListViewItem(c.Name + " (" + c.Users.Count + ")");
				if (c.isJoined == true) { lvi.Group = JoinedChannels; lvi.Checked = true; lvi.Selected = true; }
				else if (c.isFavorite == true) { lvi.Group = FavChannels; }
				else if (c._key == null) { lvi.Group = PublicChannels; }
				else { lvi.Group = PrivateChannels; }
				lvi.Text = c.Name;
				if (this.listViewChannels.View == View.Details) {
					lvi.SubItems.Add(c.Users.Count.ToString());
					lvi.SubItems.Add(c.mode.ToString());
				}
				this.listViewChannels.Items.Add(lvi);
			}
			PrivateChannels.Header += " (" + PrivateChannels.Items.Count + ")";
			PublicChannels.Header  += " (" + PublicChannels.Items.Count  + ")";
			JoinedChannels.Header += " (" + JoinedChannels.Items.Count + ")";
			FavChannels.Header += " (" + FavChannels.Items.Count + ")";
			this.listViewChannels.Invalidate();
			this.ResumeLayout();
		}

		private void listViewChannels_ItemChecked(object sender, ItemCheckedEventArgs e){
			Channel ch = Core.getChannel(e.Item.Name);
			if (e.Item.Checked == true && ch.isJoined) { ch.Leave(); } 
			else { ch.Join(); }
		}
	}
}
