using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CogitoSharp.Utils
{
	/// <summary>
	/// Mathematical utility functions
	/// </summary>
	public static class Math
	{
		private static string[] ones				= {"", "one ", "two ", "three ", "four ", "five ", "six ", "seven ", "eight ", "nine ", "ten ", "eleven ", "twelve ", "thirteen ", "fourteen ", "fifteen ", "sixteen ", "seventeen ", "eighteen ", "nineteen "};
		private static string[] _ones				= {"zero ", "one ", "two ", "three ", "four ", "five ", "six ", "seven ", "eight ", "nine "};
		private static string[] tens				= {"", "teen ", "twenty ", "thirty ", "fourty ", "fifty ", "sixty ", "seventy ", "eighty ", "ninety "};
		private static string[] units				= {"", "", "thousand ", "million ", "billion "};

		private static string[] HighScientificUnits = {"kilo ", "mega ", "giga ", "tera ", "peta ", "exa ", "zetta ", "yotta "};
		private static string[] LowScientificUnits	= {"milli ", "micro ", "nano ", "pico ", "femto ", "atto ", "zetto ", "yocto "};
		public const float Inches_to_cm		= 2.54f;
		public const float Fl_Oz_to_ml		= 29.5735296f;
		public const float Pounds_to_gram	= 453.59237f;

		/// <summary>
		/// Returns a float[] filled with values for a 'dampened spring' animation.
		/// </summary>
		/// <param name="start">Integer at which to start; x-position. Default: 0 (can be used for addition to start position)</param>
		/// <param name="amplitude">Amplitude of the spring, default 1f. Max value which it can reach.</param>
		/// <param name="damping">Level of damping, default 0.2f</param>
		/// <param name="tension">Tension of the spring, default 0.7f</param>
		/// <param name="precision">Number of data points to generate; the higher the number, the smoother the curve. Default: 50</param>
		/// <returns>A float[] with values describing the oscillation of the dampened spring.</returns>
		public static float[] dampenedSpringDelta(int start = 0, float amplitude = 1f, float damping = 0.2f, float tension = 0.7f, int precision = 50)
		{
			//dampened spring oscillation is preferable to straight-up sin wave.
			float position = -1f;
			float velocity = 0.5f;
			float[] deviations = new float[precision];
			for (int i = 0; i < precision; i++)
			{
				//insert amplitude somehow.
				velocity = velocity * (1f - damping);
				velocity -= (position - damping) * tension;
				position += velocity;
				deviations[i] = position;
			}
			float average = deviations.Average();
			for (int i = 0; i < precision; i++)
			{
				float res = deviations[i] - average;
				res *= amplitude;
				deviations[i] = res + start;
			}
			return deviations;
		}

		/// <summary>
		/// Transforms a number into its spoken representation, e.g. 123.45 to "one hundred and twenty three point fourty five"
		/// </summary>
		/// <typeparam name="T">The type of number suppled</typeparam>
		/// <param name="number">The number</param>
		/// <returns>A string with the number in spoken form.</returns>
		public static string numberToSentence<T>(T number) where T : IFormattable {
			if (float.Parse(number.ToString()) == 0f) {return "zero";}
			string[] _numStr = number.ToString().Split('.');
			string output = "";

			string[] __numStr = StringManipulation.Chunk(_numStr[0], 3, false);
			for (int j = 0; j < __numStr.Length; j++){
				string chunk = __numStr[j];
				for (int k = 0; k < chunk.Length; k++){
					int current = int.Parse(chunk[k].ToString());
					int pos = chunk.Length - k;
					switch (pos){
						case 1:
							output += ones[current];
							break;

						case 2:
							int _current = int.Parse(chunk.Substring(k));
							if (_current <= 19) { output += ones[_current]; k++; }
							else { output += tens[current]; }
							break;

						case 3:
							output += ones[current];
							output += "hundred ";
							break;
					}
				}
				output += (__numStr.Length > 1) ? units[__numStr.Length - j] : "";
			}
			if (_numStr.Length > 1) {
				output += "point ";
				foreach (char c in _numStr[1]){ output += _ones[int.Parse(c.ToString())]; }
			}
			
			return output.TrimEnd(" ".ToCharArray());
		}
		
		//public static string numberToSentence<T>(string numberStr) where T : IFormattable{
		//	float number = float.Parse(RegularExpressions.Numbers.Match(numberStr).Groups[0].Value);
		//	_number = (T)number;
		//	numberToSentence<T>();
		//}
		
		/// <summary>
		/// Returns a random element from the IEnumerable T
		/// </summary>
		/// <typeparam name="T">The type of collection from which to get the item. Must implement IEnumerable</typeparam>
		/// <param name="source">The collection from which to randomly choose an item</param>
		/// <returns>A random item from object source</returns>
		public static T RandomChoice<T>(IEnumerable<T> source){	
			Random rnd = new Random();
			IList<T> list = source as IList<T>;
			if (list != null){ return list[rnd.Next(list.Count + 1)]; }
			else{
				T result = default(T);
				int cnt = 0;
				foreach (T item in source)
				{
					cnt++;
					if (rnd.Next(cnt) == 0){ result = item; }
				}
				return result;
			}
		} //RandomChoice

		/// <summary>
		/// 
		/// </summary>
		public enum MeasurementUnit : byte {Unknown = 0, ImperialLength = 1, ImperialWeight = 2, ImperialVolume = 4, MetricLength = 8, MetricWeight = 16, MetricVolume = 32}

		/// <summary>
		/// Simple numeric struct to keep a measurement and its unit.
		/// </summary>
		/// <typeparam name="T">Numeric type of the measurement</typeparam>
		public struct Measurement<T>{
			T value;
			string UnitName;
			MeasurementUnit UnitType;
		}

		public static T parseNumberFromWords<T>(string numberSentence){
			Regex Number = new Regex(@"\d{0,9}\.{0,1}\d{0,9}");
			string numberMatch = Number.Match(numberSentence).Groups[0].Value;
			try { double.Parse(numberMatch); }
			catch (Exception){
				throw;
			}
			numberSentence = numberSentence.Replace('-', ' ');
			numberSentence = numberSentence.Replace("ty", "");
			string[] splitNumberSentence = numberSentence.Split(' ');
			foreach (string s in splitNumberSentence){
				 
			}
			throw new NotImplementedException();
		}

		/// <summary>
		/// Takes a descriptive string, e.g. "They are between 5 and 10 inches tall" and attempts to return a number of type T. 
		/// When a range is detected, the arithmetic mean is returned.
		/// All data is converted to a standard metric unit before being returned as a Measurement instance.
		/// </summary>
		/// <typeparam name="T">The numeric type the function returns. Internally, numbers are handled as doubles...?</typeparam>
		/// <param name="TextToAnalyze">The text string from which data is supposed to be parsed</param>
		/// <param name="MeasureToParseAs">If known, the type of measurement to be parsed.</param>
		/// <returns> A Measurement<!--<T>--> instance with the result as type T and the unit in a string"/> A Measurement with numeric type T</returns>
		public static Measurement<T> parseNumberFromDescription<T>(string TextToAnalyze, MeasurementUnit MeasureToParseAs = MeasurementUnit.Unknown){
			throw new NotImplementedException("METHOD ISN'T DONE YET");
			Measurement<T> Result = new Measurement<T>();
			string[] RangeIndicators = { "-", "/", " to ", " and " };
			string[] MetricIndicators = { "cm", "m", "km" };
			string[] ImperialIndicators = { "in", "inches", "inch", "feet", "foot", "\"", "'"};

			switch (MeasureToParseAs){
				case MeasurementUnit.Unknown:
				
				break;
				
				case MeasurementUnit.ImperialLength:
				
				break;
				
				case MeasurementUnit.ImperialVolume:

				break;
				
				case MeasurementUnit.ImperialWeight:
				
				break;
				
				case MeasurementUnit.MetricLength:
				
				break;
				
				case MeasurementUnit.MetricVolume:
				
				break;
				
				case MeasurementUnit.MetricWeight:
				
				break;
			}

			//TODO: If regex doesn't find anything, try Parse Number From Words

			return Result;
			//Imperial Length - inches, in, feet, f, ' "
			//Imperial Weight - 
			//Imperial Volume - gallon, quart, fl oz
			//Metric Length   - centimeter, meter, cm, m, km, etc etc
			//Metric weight   - gram, kilo, kg, g, ton 
			//metric volume   - 

			//range indicators- "to" "-" "/" ","
			//remove tokens per strip... or just collect 
		}
	}// class Math

	public static class RegularExpressions{
		internal static Regex ProfileHTMLTags = new Regex(@"<span class=.*>(.*):</span>(.*)");
		internal static Regex AgeSearch = new Regex(@"\d{1, 9}");
		internal static Regex Numbers = new Regex(@"\d{1,5}\.?\d{0,2}");
	}

	public static class StringManipulation{

		/// <summary> Reverses a string into a string and not a char[] nightmare. What the eff, C#.</summary>
		/// <param name="s">String to be reversed</param>
		/// <returns>desrever eb ot gnirtS</returns>
		public static string ReverseString(string s){
			char[] arr = s.ToCharArray();
			Array.Reverse(arr);
			return new string(arr);
		} //ReverseString

		/// <summary> Devides a string str into an IEnumerable with chunkSize elements in it</summary>
		/// <param name="str">The string to chunk</param>
		/// <param name="chunkSize">The number of chunks to divide into</param>
		/// <param name="forward">Determines if chunking is forward or reverse, e.g. XXXXXXXX into chunks of 3 can be "XXX XXX XX"(fwd) or "XX XXX XXX"(rev). Default is true.</param>
		/// <returns>An IEnumerable containing the chunks</returns>
		public static string[] Chunk(string str, int chunkSize, bool fwd = true){
			if (str == null){ return new string[0]; }
			float _chunks = ((float)str.Length/chunkSize);
			int chunks = (int)System.Math.Ceiling(_chunks);
			if (fwd == true) { 
				string[] result = Enumerable.Range(0, chunks)
					.Select(i => str.Substring(i * chunkSize, (i * chunkSize + chunkSize <= str.Length) ? chunkSize : str.Length - i * chunkSize)).ToArray<string>(); 
				return result;
			} //if
			else{
				string _str = ReverseString(str);
				string[] result = Enumerable.Range(0, chunks)
					.Select(i => ReverseString(_str.Substring(i * chunkSize, (i * chunkSize + chunkSize <= str.Length) ? chunkSize : str.Length - i * chunkSize))).Reverse().ToArray<string>(); 
				return result;
			} //else
		} //Chunk
	} //StringManipulation
}
