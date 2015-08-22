using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using HtmlAgilityPack;

//Computer-Assisted Social Interaction Enhancers

namespace CogitoSharp.Gimmicks
{
	public partial class CASIE : Form{

		internal const int FaveValue = 5;
		internal const int YesValue = 3;
		internal const int MaybeValue = 1;
		internal const int NoValue = 0;

		public CASIE(){
			InitializeComponent();
		}

		public CASIE(string targetCharacter){
			InitializeComponent();
		}

		internal void calculateCASIE(string targetCharacter){
			//TODO: Iterate over Fave/Yes/Maybe/No arrays, sum up differences
			//MaxValue = 100 (%)
			//Change per item = 100 / nKinks in BOTH lists (ignore customs!)
			//ChangeValue = You - His -> e.g. You: Fave, He: Fave = 5 - 5 = 0; 100 - 0 * ChangePerItem
			 
		}
	}
}
