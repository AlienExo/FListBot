using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

/* TODO
 * [----------] WYSIWYG BBCode Editor
 * [----------] Timer per ad; adjustable via slider (?) between VAR ad_flood as min and five minutes maximum
 * [----------] Separate thread for this sending?
 */

namespace CogitoSharp.Gimmicks
{
	/// <summary>
	/// A Control to remember your last-used RP ads and re-publish them periodically. 
	/// </summary>
	public partial class RPAdDeployer : Form
	{
		public RPAdDeployer()
		{
			InitializeComponent();
		}
	}
}
