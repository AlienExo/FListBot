using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace CogitoSharp
{
	/// <summary>
	/// A GUI Element that's supposed to capture, catalogue and summarize information about all members of a channel, similar to the old Cogito's .scan function.
	/// Everybody loves essentially worthless statistics!
	/// </summary>
	public partial class Scanner : Form{

		public Scanner()
		{
			InitializeComponent();
		}

		//TODO this will be rough... async mine all involved users, get data, calculate based on options, done.
		public delegate void DataMiningProgressChangedEventHandler(ProgressChangedEventArgs e);

		public delegate void DataMiningCompleteEventHandler(object sender,	DataMiningCompleteEventArgs e);

		public event DataMiningProgressChangedEventHandler ProgressChanged;

		public event DataMiningCompleteEventHandler DataMiningComplete;

		public class DataMiningCompleteEventArgs : AsyncCompletedEventArgs
		{
			public DataMiningCompleteEventArgs(bool isCancelled, object state, Exception e) : base(e, isCancelled, state){
			
			}
		}
	}
}
