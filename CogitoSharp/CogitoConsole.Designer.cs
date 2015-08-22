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
			this.panel1 = new System.Windows.Forms.Panel();
			this.console = new ConsoleTextBox();
			this.panel2 = new System.Windows.Forms.Panel();
			this.input = new System.Windows.Forms.TextBox();
			this.panel1.SuspendLayout();
			this.panel2.SuspendLayout();
			this.SuspendLayout();
			// 
			// panel1
			// 
			this.panel1.BackColor = System.Drawing.Color.Black;
			this.panel1.Controls.Add(this.console);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel1.Location = new System.Drawing.Point(0, 0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(495, 516);
			this.panel1.TabIndex = 0;
			// 
			// console
			// 
			this.console.BackColor = System.Drawing.Color.Black;
			this.console.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.console.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.console.ForeColor = System.Drawing.Color.LimeGreen;
			this.console.Location = new System.Drawing.Point(3, 3);
			this.console.Multiline = true;
			this.console.Name = "console";
			this.console.ReadOnly = true;
			this.console.Size = new System.Drawing.Size(489, 485);
			this.console.TabIndex = 2;
			// 
			// panel2
			// 
			this.panel2.BackColor = System.Drawing.Color.Black;
			this.panel2.Controls.Add(this.input);
			this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panel2.Location = new System.Drawing.Point(0, 484);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(495, 32);
			this.panel2.TabIndex = 1;
			// 
			// input
			// 
			this.input.BackColor = System.Drawing.Color.Black;
			this.input.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.input.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.input.ForeColor = System.Drawing.Color.LimeGreen;
			this.input.Location = new System.Drawing.Point(3, 7);
			this.input.Name = "input";
			this.input.Size = new System.Drawing.Size(489, 20);
			this.input.TabIndex = 3;
			this.input.Text = ">";
			this.input.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.input_KeyPress);
			// 
			// CogitoConsole
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(495, 516);
			this.Controls.Add(this.panel2);
			this.Controls.Add(this.panel1);
			this.Name = "CogitoConsole";
			this.Text = "Console";
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			this.panel2.ResumeLayout(false);
			this.panel2.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.TextBox input;
		protected internal ConsoleTextBox console;
	}
}