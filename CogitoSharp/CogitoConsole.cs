using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;

namespace CogitoSharp.Debug
{
	public sealed class ConsoleTextBox : TextBox{
		delegate void AppendTextCallback(string text);

		internal new void AppendText(string text)
		{
			if (this.InvokeRequired)
			{
				AppendTextCallback d = new AppendTextCallback(AppendText);
				this.Invoke(d, new object[] { text });
			}
			else
			{
				if (!text.EndsWith("\r\n")) { text += "\r\n"; }
				this.Text = this.Text + text;
				
			}
		}
	}

	public partial class CogitoConsole : Form
	{
		public CogitoConsole()
		{
			InitializeComponent();
			this.MdiParent = Core.cogitoUI;
		}

		private void input_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (e.KeyChar == (char)Keys.Enter){
				this.console.AppendText(this.input.Text);

				this.input.Text = "> ";
				//todo console parser/logic -> direct interface user <-> bot
			}
			
		}
	}
}

