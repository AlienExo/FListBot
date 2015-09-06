using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CogitoSharp
{
    public partial class CogitoUI : Form
    {
		internal static LoginForm loginForm = new LoginForm();
		internal static ChatUI chatUI = null;
		internal static CogitoSharp.Debug.CogitoConsole console = null;

		/// <summary> Main UI, containing all other elements. Win98, baby </summary>
		public CogitoUI()
        {
            InitializeComponent();
			this.IsMdiContainer = true;
			#if DEBUG
				CogitoUI.console = new CogitoSharp.Debug.CogitoConsole();
				console.MdiParent = this;
			#endif
		}
		
       	private void CogitoUI_Load(object sender, EventArgs e)
		{
			loginForm.MdiParent = this;
			this.Text = "Cogito v"+Application.ProductVersion;
			loginForm.Show();
		}

		private void CogitoUI_FormClosing(object sender, FormClosingEventArgs e){
			if (Core.websocket.IsAlive){
				DialogResult d1 = MessageBox.Show("This will close all connections and shut down. Are you sure?", "Please confirm Shutdown", MessageBoxButtons.YesNo);
				if (d1 == DialogResult.Yes) {
					Console.WriteLine("Program is shutting down. Closing connection...");
					Core.websocket.Close();
					base.OnFormClosing(e);
				}
				else { e.Cancel = true; }
			}
		}
	}

	/// <summary> Base Class for Boxes with images, yay </summary>
	public class OwnerDrawnListBox : Control
	{
		const int SCROLL_WIDTH = 20;
		int itemHeight = -1;
		int selectedIndex = -1;

		Bitmap offScreen;
		VScrollBar vs;
		System.Collections.ArrayList items;

		public OwnerDrawnListBox(){
			this.vs = new VScrollBar();
			this.vs.Parent = this;
			this.vs.Visible = false;
			this.vs.SmallChange = 1;
			this.vs.ValueChanged += new EventHandler(this.ScrollValueChanged);
			this.vs.LargeChange = 0;
			this.items = new System.Collections.ArrayList();
		}

		public System.Collections.ArrayList Items{ get{ return this.items; } }

		protected Bitmap OffScreen{ get{ return this.offScreen; } }

		protected VScrollBar VScrollBar{ get{ return this.vs; } }

		public event EventHandler SelectedIndexChanged;

		/// <summary> Raise the SelectedIndexChanged event </summary>
		/// <param name="e"></param>
		protected virtual void OnSelectedIndexChanged(EventArgs e)
		{
			if (this.SelectedIndexChanged != null)
				this.SelectedIndexChanged(this, e);
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
			this.SelectedIndex = this.vs.Value + (e.Y / this.ItemHeight);

			// Invalidate the control so we can draw the item as selected.
			this.Refresh();
		}

		/// <summary> Get or set index of selected item. </summary>
		public int SelectedIndex{
			get{ return this.selectedIndex; }

			set{
				this.selectedIndex = value;

				if (this.SelectedIndexChanged != null)
					this.SelectedIndexChanged(this, EventArgs.Empty);
			}
		}

		protected void ScrollValueChanged(object o, EventArgs e){ this.Refresh(); }

		protected virtual int ItemHeight{
			get{ return this.itemHeight; }
			set {this.itemHeight = value; }
		}

		// If the requested index is before the first visible index then set the
		// first item to be the requested index. If it is after the last visible
		// index, then set the last visible index to be the requested index.
		public void EnsureVisible(int index)
		{
			if (index < this.vs.Value){
				this.vs.Value = index;
				this.Refresh();
			}
			else if (index >= this.vs.Value + this.DrawCount){
				this.vs.Value = index - this.DrawCount + 1;
				this.Refresh();
			}
		}


		// Need to set focus to the control when it
		// is clicked so that keyboard events occur.
		protected override void OnClick(EventArgs e){
			this.Focus();
			base.OnClick(e);
		}

		// Selected item moves when you use the keyboard up/down keys.
		protected override void OnKeyDown(KeyEventArgs e){
			switch (e.KeyCode)
			{
				case Keys.Down:
					if (this.SelectedIndex < this.vs.Maximum){
						EnsureVisible(++this.SelectedIndex);
						this.Refresh();
					}
					break;
				case Keys.Up:
					if (this.SelectedIndex > this.vs.Minimum){
						EnsureVisible(--this.SelectedIndex);
						this.Refresh();
					}
					break;
				case Keys.PageDown:
					this.SelectedIndex = Math.Min(this.vs.Maximum, this.SelectedIndex + this.DrawCount);
					EnsureVisible(this.SelectedIndex);
					this.Refresh();
					break;
				case Keys.PageUp:
					this.SelectedIndex = Math.Max(this.vs.Minimum, this.SelectedIndex - this.DrawCount);
					EnsureVisible(this.SelectedIndex);
					this.Refresh();
					break;
				case Keys.Home:
					this.SelectedIndex = 0;
					EnsureVisible(this.SelectedIndex);
					this.Refresh();
					break;
				case Keys.End:
					this.SelectedIndex = this.items.Count - 1;
					EnsureVisible(this.SelectedIndex);
					this.Refresh();
					break;
			}
			base.OnKeyDown(e);
		}

		// Calculate how many items we can draw given the height of the control.
		protected int DrawCount{
			get{
				if (this.vs.Value + this.vs.LargeChange > this.vs.Maximum) { return this.vs.Maximum - this.vs.Value + 1; }
				else { return this.vs.LargeChange > 0 ? this.vs.LargeChange : 0; }
			}
		}

		protected override void OnResize(EventArgs e){
			int viewableItemCount = this.ClientSize.Height / this.ItemHeight;

			this.vs.Bounds = new Rectangle(this.ClientSize.Width - SCROLL_WIDTH,
				0,
				SCROLL_WIDTH,
				this.ClientSize.Height);

			// Determine if scrollbars are needed
			if (this.items.Count > viewableItemCount){
				this.vs.Visible = true;
				this.vs.LargeChange = viewableItemCount;
				this.offScreen = new Bitmap(this.ClientSize.Width - SCROLL_WIDTH - 1, this.ClientSize.Height - 2);
			}
			else{
				this.vs.Visible = false;
				this.vs.LargeChange = this.items.Count;
				this.offScreen = new Bitmap(this.ClientSize.Width - 1, this.ClientSize.Height - 2);
			}
			this.vs.Maximum = this.items.Count - 1 > 0 ? this.items.Count - 1 : 0;
		}
	}
}