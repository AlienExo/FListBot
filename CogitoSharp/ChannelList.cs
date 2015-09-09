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
		public ChannelList()
		{
			InitializeComponent();
		}

		private void ChannelList_Enter(object sender, EventArgs e)
		{
			this.listViewChannels.Groups[0].Items.Clear();
			this.listViewChannels.Groups[1].Items.Clear();
			foreach (Channel c in Core.channels){
				if (c._key == null) { this.listViewChannels.Groups[0].Items.Add(new ListViewItem(c.name)); }
				else { this.listViewChannels.Groups[1].Items.Add(new ListViewItem(c.name)); }
			}
		}
	}
}
