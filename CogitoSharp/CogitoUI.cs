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
		public static LoginForm loginForm = new LoginForm();
		public static ChatUI chatUI = new ChatUI();

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

		private void CogitoUI_ResizeEnd(object sender, EventArgs e)
		{
			
		}

		private void CogitoUI_FormClosing(object sender, FormClosingEventArgs e)
		{
			Console.WriteLine("Closing connection...");
			Core.websocket.Close();
		}
	}
}