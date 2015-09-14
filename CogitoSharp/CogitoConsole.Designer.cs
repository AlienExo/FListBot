namespace CogitoSharp.Debug
{
	partial class CogitoConsole
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected virtual new void Dispose(bool disposing)
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CogitoConsole));
			this.console = new CogitoSharp.Debug.ConsoleTextBox();
			this.input = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// console
			// 
			this.console.BackColor = System.Drawing.Color.Black;
			this.console.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.console.Cursor = System.Windows.Forms.Cursors.IBeam;
			this.console.Dock = System.Windows.Forms.DockStyle.Fill;
			this.console.Font = new System.Drawing.Font("Consolas", 10F);
			this.console.ForeColor = System.Drawing.Color.Gold;
			this.console.Location = new System.Drawing.Point(0, 0);
			this.console.Margin = new System.Windows.Forms.Padding(0);
			this.console.Multiline = true;
			this.console.Name = "console";
			this.console.ReadOnly = true;
			this.console.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.console.Size = new System.Drawing.Size(542, 695);
			this.console.TabIndex = 0;
			this.console.WordWrap = false;
			// 
			// input
			// 
			this.input.BackColor = System.Drawing.Color.Black;
			this.input.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.input.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.input.Font = new System.Drawing.Font("Consolas", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.input.ForeColor = System.Drawing.Color.Gold;
			this.input.Location = new System.Drawing.Point(0, 679);
			this.input.Margin = new System.Windows.Forms.Padding(0);
			this.input.Name = "input";
			this.input.Size = new System.Drawing.Size(542, 16);
			this.input.TabIndex = 1;
			this.input.Text = "> ";
			this.input.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.input_KeyPress);
			// 
			// CogitoConsole
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.Black;
			this.ClientSize = new System.Drawing.Size(542, 695);
			this.Controls.Add(this.input);
			this.Controls.Add(this.console);
			this.Font = new System.Drawing.Font("Consolas", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.ForeColor = System.Drawing.Color.Gold;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Margin = new System.Windows.Forms.Padding(4);
			this.Name = "CogitoConsole";
			this.StartPosition = System.Windows.Forms.FormStartPosition.WindowsDefaultBounds;
			this.Text = "Console";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox input;
		internal ConsoleTextBox console;
	}
}