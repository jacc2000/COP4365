using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp_COP_4365
{
    /// <summary>
    /// Recognizer for identifying Peak candlestick patterns.
    /// </summary>
    internal class Recognizer_Peak : Recognizer
    {
        /// <summary>
        /// Initializes a new instance of the Recognizer_Peak class.
        /// </summary>
        public Recognizer_Peak() : base("Peak", 3)
        {
        }

        /// <summary>
        /// Checks if the candlestick at the specified index forms a Peak pattern.
        /// </summary>
        /// <param name="scsList">List of SmartCandlestick objects.</param>
        /// <param name="index">Index of the candlestick to check.</param>
        /// <returns>True if Peak pattern is found; otherwise, false.</returns>
        public override bool Recognize(List<SmartCandlestick> scsList, int index)
        {
            SmartCandlestick scs = scsList[index];
            if (scs.Dictionary_Pattern.TryGetValue(Pattern_Name, out bool value))
            {
                return value;  // Return previously computed value if available
            }
            else
            {
                // Check if the index allows checking of adjacent candlesticks to form a peak
                int offset = Pattern_Length / 2;
                if ((index < offset) || (index == scsList.Count - offset))
                {
                    scs.Dictionary_Pattern.Add(Pattern_Name, false);
                    return false;  // Not enough candlesticks to form a peak
                }
                else
                {
                    // Check if the current candlestick's high is greater than those of the adjacent candlesticks
                    SmartCandlestick prev = scsList[index - offset];
                    SmartCandlestick next = scsList[index + offset];
                    bool peak = (scs.high > prev.high) && (scs.high > next.high);
                    scs.Dictionary_Pattern.Add(Pattern_Name, peak);
                    return peak;  // Return the peak evaluation result
                }
            }
        }
    }
}
