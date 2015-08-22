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
    public partial class CogitoUI : Form
    {
		internal static LoginForm loginForm = new LoginForm();
		internal static ChatUI chatUI = new ChatUI();
		#if DEBUG
		internal static ChatTab DEBUGTAB = new ChatTab("DEBUG TAB");
		#endif

		public CogitoUI()
        {
            InitializeComponent();
			this.IsMdiContainer = true;
		}
		
       	private void CogitoUI_Load(object sender, EventArgs e)
		{
			loginForm.MdiParent = this;
			chatUI.MdiParent = this;
			this.Text = "Cogito v"+Application.ProductVersion;
			chatUI.Hide();
			loginForm.Show();
		}

		private void CogitoUI_FormClosing(object sender, FormClosingEventArgs e)
		{
			Console.WriteLine("Program is shutting down. Closing connection...");
			if (Core.websocket.IsAlive){Core.websocket.Close();}
		}

	}
}