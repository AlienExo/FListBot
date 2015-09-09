using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CogitoSharp.Debug
{
	public partial class CogitoConsole : Form
	{
		public CogitoConsole()
		{
			InitializeComponent();
			this.MdiParent = Core.cogitoUI;
			this.console.ScrollBars = ScrollBars.Both;
		}

		private void input_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (e.KeyChar == (char)Keys.Enter)
			{
				this.console.AppendText(this.input.Text);
				this.input.Text = "> ";
				this.input.Select(this.input.Text.Length, 0);
				return;
				//todo console parser/logic -> direct interface user <-> bot
			}
			base.OnKeyPress(e);
		}

		
	}

	internal sealed class ConsoleTextBox : TextBox{
		delegate void AppendTextCallback(string text);

		public new void AppendText(string text)
		{
			if (this.InvokeRequired)
			{
				AppendTextCallback d = new AppendTextCallback(AppendText);
				this.Invoke(d, new object[] { text });
			}
			else
			{
				if (!text.EndsWith(Environment.NewLine)) { text += Environment.NewLine; }
				this.Text += text;
			}
		}
	}
}
