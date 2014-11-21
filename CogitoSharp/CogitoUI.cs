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
        public CogitoUI()
        {
            InitializeComponent();
            chatTabs.BringToFront();
        }

        private void sendButton_Click(object sender, EventArgs e)
        {
            Console.WriteLine(textBox1.Text);
            textBox1.Text = "";
        }

        private void chatTabs_Selected(object sender, EventArgs e) 
        { 
            
        }

        private void textBox1_GotFocus(object sender, EventArgs e)
        {
            Console.WriteLine(sender.ToString());
            this.AcceptButton = sendButton;
            //TODO: set sendButton to get whichever tab is in focus/front and therefore where to send what's entered
        }

        
    }

    public class ChatTab : TabPage
    { 
        
        private TextBox text = new TextBox();

        public ChatTab(string _title)
        {
            base.Name = _title;
            base.Text = _title;
            this.text.AcceptsReturn = true;
            this.SuspendLayout();
            this.Controls.Add(text);
            this.ResumeLayout();
            text.Parent = this;
            text.Dock = DockStyle.Fill;
            text.BringToFront();
        }

    }
}
