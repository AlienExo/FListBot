namespace CogitoSharp.Gimmicks
{
	partial class Scanner{
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
			System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
			System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
			System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
			System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
			this.listViewAllChannels = new System.Windows.Forms.ListView();
			this.listViewAllProperties = new System.Windows.Forms.ListView();
			this.panel1 = new System.Windows.Forms.Panel();
			this.checkBoxIsNumeric = new System.Windows.Forms.CheckBox();
			this.checkBoxMakeStats = new System.Windows.Forms.CheckBox();
			this.checkBoxIsAnon = new System.Windows.Forms.CheckBox();
			this.checkBoxIsSimple = new System.Windows.Forms.CheckBox();
			this.panel2 = new System.Windows.Forms.Panel();
			this.panel3 = new System.Windows.Forms.Panel();
			this.chartForSelectedData = new System.Windows.Forms.DataVisualization.Charting.Chart();
			this.panel1.SuspendLayout();
			this.panel3.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.chartForSelectedData)).BeginInit();
			this.SuspendLayout();
			// 
			// listViewAllChannels
			// 
			this.listViewAllChannels.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.listViewAllChannels.Location = new System.Drawing.Point(13, 13);
			this.listViewAllChannels.Name = "listViewAllChannels";
			this.listViewAllChannels.Size = new System.Drawing.Size(415, 125);
			this.listViewAllChannels.TabIndex = 0;
			this.listViewAllChannels.UseCompatibleStateImageBehavior = false;
			this.listViewAllChannels.View = System.Windows.Forms.View.List;
			// 
			// listViewAllProperties
			// 
			this.listViewAllProperties.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.listViewAllProperties.Location = new System.Drawing.Point(13, 144);
			this.listViewAllProperties.Name = "listViewAllProperties";
			this.listViewAllProperties.Size = new System.Drawing.Size(415, 125);
			this.listViewAllProperties.TabIndex = 1;
			this.listViewAllProperties.UseCompatibleStateImageBehavior = false;
			this.listViewAllProperties.View = System.Windows.Forms.View.List;
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.checkBoxIsSimple);
			this.panel1.Controls.Add(this.checkBoxIsAnon);
			this.panel1.Controls.Add(this.checkBoxMakeStats);
			this.panel1.Controls.Add(this.checkBoxIsNumeric);
			this.panel1.Location = new System.Drawing.Point(13, 276);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(415, 25);
			this.panel1.TabIndex = 2;
			// 
			// checkBoxIsNumeric
			// 
			this.checkBoxIsNumeric.AutoSize = true;
			this.checkBoxIsNumeric.Location = new System.Drawing.Point(4, 4);
			this.checkBoxIsNumeric.Name = "checkBoxIsNumeric";
			this.checkBoxIsNumeric.Size = new System.Drawing.Size(99, 17);
			this.checkBoxIsNumeric.TabIndex = 0;
			this.checkBoxIsNumeric.Text = "Data is numeric";
			this.checkBoxIsNumeric.UseVisualStyleBackColor = true;
			// 
			// checkBoxMakeStats
			// 
			this.checkBoxMakeStats.AutoSize = true;
			this.checkBoxMakeStats.Location = new System.Drawing.Point(109, 4);
			this.checkBoxMakeStats.Name = "checkBoxMakeStats";
			this.checkBoxMakeStats.Size = new System.Drawing.Size(130, 17);
			this.checkBoxMakeStats.TabIndex = 1;
			this.checkBoxMakeStats.Text = "Report overall statistic";
			this.checkBoxMakeStats.UseVisualStyleBackColor = true;
			// 
			// checkBoxIsAnon
			// 
			this.checkBoxIsAnon.AutoSize = true;
			this.checkBoxIsAnon.Location = new System.Drawing.Point(245, 4);
			this.checkBoxIsAnon.Name = "checkBoxIsAnon";
			this.checkBoxIsAnon.Size = new System.Drawing.Size(81, 17);
			this.checkBoxIsAnon.TabIndex = 2;
			this.checkBoxIsAnon.Text = "Anonymous";
			this.checkBoxIsAnon.UseVisualStyleBackColor = true;
			// 
			// checkBoxIsSimple
			// 
			this.checkBoxIsSimple.AutoSize = true;
			this.checkBoxIsSimple.Location = new System.Drawing.Point(332, 4);
			this.checkBoxIsSimple.Name = "checkBoxIsSimple";
			this.checkBoxIsSimple.Size = new System.Drawing.Size(57, 17);
			this.checkBoxIsSimple.TabIndex = 3;
			this.checkBoxIsSimple.Text = "Simple";
			this.checkBoxIsSimple.UseVisualStyleBackColor = true;
			// 
			// panel2
			// 
			this.panel2.Location = new System.Drawing.Point(13, 308);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(207, 344);
			this.panel2.TabIndex = 3;
			// 
			// panel3
			// 
			this.panel3.Controls.Add(this.chartForSelectedData);
			this.panel3.Location = new System.Drawing.Point(226, 308);
			this.panel3.Name = "panel3";
			this.panel3.Size = new System.Drawing.Size(202, 344);
			this.panel3.TabIndex = 4;
			// 
			// chartForSelectedData
			// 
			chartArea1.Area3DStyle.Enable3D = true;
			chartArea1.Name = "ChartArea1";
			chartArea2.Area3DStyle.Enable3D = true;
			chartArea2.Name = "ChartArea2";
			this.chartForSelectedData.ChartAreas.Add(chartArea1);
			this.chartForSelectedData.ChartAreas.Add(chartArea2);
			this.chartForSelectedData.Location = new System.Drawing.Point(4, 4);
			this.chartForSelectedData.Name = "chartForSelectedData";
			series1.ChartArea = "ChartArea1";
			series1.Legend = "Legend1";
			series1.Name = "Series1";
			series2.ChartArea = "ChartArea2";
			series2.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Pie;
			series2.IsValueShownAsLabel = true;
			series2.Legend = "Legend1";
			series2.Name = "Series2";
			this.chartForSelectedData.Series.Add(series1);
			this.chartForSelectedData.Series.Add(series2);
			this.chartForSelectedData.Size = new System.Drawing.Size(195, 337);
			this.chartForSelectedData.TabIndex = 0;
			this.chartForSelectedData.Text = "chart1";
			// 
			// Scanner
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(440, 664);
			this.Controls.Add(this.panel3);
			this.Controls.Add(this.panel2);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.listViewAllProperties);
			this.Controls.Add(this.listViewAllChannels);
			this.Name = "Scanner";
			this.Text = "Scanner";
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			this.panel3.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.chartForSelectedData)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.ListView listViewAllChannels;
		private System.Windows.Forms.ListView listViewAllProperties;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.CheckBox checkBoxIsNumeric;
		private System.Windows.Forms.CheckBox checkBoxIsSimple;
		private System.Windows.Forms.CheckBox checkBoxIsAnon;
		private System.Windows.Forms.CheckBox checkBoxMakeStats;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.Panel panel3;
		private System.Windows.Forms.DataVisualization.Charting.Chart chartForSelectedData;
	}
}