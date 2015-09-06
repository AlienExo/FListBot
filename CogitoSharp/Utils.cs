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
	public class Math
	{
		/// <summary>
		/// Returns a float[] filled with values for a 'dampened spring' animation.
		/// </summary>
		/// <param name="start">Integer at which to start; x-position. Default: 0 (can be used for addition to start position)</param>
		/// <param name="amplitude">Amplitude of the spring, default 1f. Max value which it can reach.</param>
		/// <param name="damping">Level of damping, default 0.2f</param>
		/// <param name="tension">Tension of the spring, default 0.7f</param>
		/// <param name="precision">Number of data points to generate; the higher the number, the smoother the curve. Default: 50</param>
		/// <returns>A float[] with values describing the oscillation of the dampened spring.</returns>
		protected internal static float[] dampenedSpringDelta(int start = 0, float amplitude = 1f, float damping = 0.2f, float tension = 0.7f, int precision = 50)
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
		protected internal static string numberToSentence<T>(ref T number) where T : IFormattable {
			throw new NotImplementedException("Function has not been implemented");
		}

		/// <summary>
		/// Returns a random element from the IEnumerable T
		/// </summary>
		/// <typeparam name="T">The type of collection from which to get the item. Must implement IEnumerable</typeparam>
		/// <param name="source">The collection from which to randomly choose an item</param>
		/// <returns>A random item from object source</returns>
		protected internal static T RandomChoice<T>(IEnumerable<T> source){	
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

		protected internal static T parseNumberFromWords<T>(string numberSentence){
			throw new NotImplementedException();
		}

		/// <summary>
		/// Takes a descriptive string, e.g. "They are between 5 and 10 inches tall" and attempts to return a number of type T. 
		/// When a range is detected, the arithmetic mean is returned.
		/// All data is converted to a standard metric unit before being returned as a <see cref="Measurement"/> instance.
		/// </summary>
		/// <typeparam name="T">The numeric type the function returns. Internally, numbers are handled as doubles...?</typeparam>
		/// <param name="profileText">The text string from which data is supposed to be taken</param>
		/// <param name="MeasureToParseAs">If known, the type of data to be parsed.</param>
		/// <returns> A Measurement<T> instance with the result as type T and the unit in a string"/> A Measurement with numeric type T</returns>
		protected internal static Measurement<T> parseNumberFromDescription<T>(string profileText, MeasurementUnit MeasureToParseAs = MeasurementUnit.Unknown){
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

	class RegularExpressions{
		internal static Regex ProfileHTMLTags = new Regex(@"<span class=.*>(.*):</span>(.*)");
		internal static Regex AgeSearch = new Regex(@"\d{1, 9}");
	}
}
