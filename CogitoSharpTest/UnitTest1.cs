using System;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using CogitoSharp;

//https://msdn.microsoft.com/en-us/library/ms182532.aspx#bkmk_prepare_the_walkthrough
//[ExpectedException(typeof(ArgumentOutOfRangeException))]
//Assert.AreEqual(expected, actual, 0.001, "Account not debited correctly");



namespace CogitoSharpTest
{
	[TestClass]
	public class CharacterTests
	{
		
		const string ChunkingTestString = "11122233344455";

		[TestMethod]
		public void Test_CharacterClassCollectsCorrectData(){
			
		}

		[TestMethod]
		public void Test_ForwardStringChunkingAlgorithm(){
			string[] chunk01 = CogitoSharp.Utils.StringManipulation.Chunk(ChunkingTestString, 3);
			Debug.WriteLine(String.Join("|", chunk01));
			CollectionAssert.AreEqual(new string[]{"111", "222", "333", "444", "55"}, chunk01);
		}

		[TestMethod]
		public void Test_ReverseStringChunking(){
			string[] chunk02 = CogitoSharp.Utils.StringManipulation.Chunk(ChunkingTestString, 3, false);
			Debug.WriteLine(String.Join("|", chunk02));
			CollectionAssert.AreEqual(new string[] { "11", "122", "233", "344", "455" }, chunk02);
		}

		[TestMethod]
		public void Test_NumberToSentence(){
			string n1 = CogitoSharp.Utils.Math.numberToSentence(12456789);
			string n2 = CogitoSharp.Utils.Math.numberToSentence(1234.567);
			string n3 = CogitoSharp.Utils.Math.numberToSentence(0000.000);
			string n4 = CogitoSharp.Utils.Math.numberToSentence(1230.057);
			Assert.AreEqual("twelve million four hundred fifty six thousand seven hundred eighty nine", n1);
			Assert.AreEqual("one thousand two hundred thirty four point five six seven", n2);
			Assert.AreEqual("zero", n3);
			Assert.AreEqual("one thousand two hundred thirty point zero five seven", n4);
		}
	}
}
