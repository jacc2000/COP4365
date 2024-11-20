using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp_COP_4365
{
    /// <summary>
    /// Recognizer for identifying Valley candlestick patterns.
    /// </summary>
    internal class Recognizer_Valley : Recognizer
    {
        /// <summary>
        /// Initializes a new instance of the Recognizer_Valley class.
        /// </summary>
        public Recognizer_Valley() : base("Valley", 3)
        {
        }

        /// <summary>
        /// Checks if the candlestick at the specified index forms a Valley pattern.
        /// </summary>
        /// <param name="scsList">List of SmartCandlestick objects.</param>
        /// <param name="index">Index of the candlestick to check.</param>
        /// <returns>True if Valley pattern is found; otherwise, false.</returns>
        public override bool Recognize(List<SmartCandlestick> scsList, int index)
        {
            SmartCandlestick scs = scsList[index];
            if (scs.Dictionary_Pattern.TryGetValue(Pattern_Name, out bool value))
            {
                return value;  // Return previously computed value if available
            }
            else
            {
                // Check if the index allows checking of adjacent candlesticks to form a valley
                int offset = Pattern_Length / 2;
                if ((index < offset) || (index == scsList.Count - offset))
                {
                    scs.Dictionary_Pattern.Add(Pattern_Name, false);
                    return false;  // Not enough candlesticks to form a valley
                }
                else
                {
                    // Check if the current candlestick's low is lower than those of the adjacent candlesticks
                    SmartCandlestick prev = scsList[index - offset];
                    SmartCandlestick next = scsList[index + offset];
                    bool valley = (scs.low < prev.low) && (scs.low < next.low);
                    scs.Dictionary_Pattern.Add(Pattern_Name, valley);
                    return valley;  // Return the valley evaluation result
                }
            }
        }
    }
}
