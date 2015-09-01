using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CogitoSharp.Utils
{
	class Math
	{
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

		protected internal static string numberToSentence<T>(ref T number) where T : IFormattable {
			throw new NotImplementedException("Function has not been implemented");
		}
	}

	class RegularExpressions{
		internal static Regex ProfileHTMLTags = new Regex(@"<span class=.*>(.*):</span>(.*)");
		internal static Regex AgeSearch = new Regex(@"\d{1, 9}");
	}
}
