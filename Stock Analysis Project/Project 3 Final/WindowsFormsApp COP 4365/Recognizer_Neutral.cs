using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp_COP_4365
{
    /// <summary>
    /// Recognizer for identifying Neutral candlestick patterns.
    /// </summary>
    internal class Recognizer_Neutral : Recognizer
    {
        /// <summary>
        /// Initializes a new instance of the Recognizer_Neutral class.
        /// </summary>
        public Recognizer_Neutral() : base("Neutral", 1)
        {
        }

        /// <summary>
        /// Checks if the candlestick at the specified index is a Neutral pattern.
        /// </summary>
        /// <param name="scsList">List of SmartCandlestick objects.</param>
        /// <param name="index">Index of the candlestick to check.</param>
        /// <returns>True if Neutral pattern is found; otherwise, false.</returns>
        public override bool Recognize(List<SmartCandlestick> scsList, int index)
        {
            SmartCandlestick scs = scsList[index];
            if (scs.Dictionary_Pattern.TryGetValue(Pattern_Name, out bool value))
            {
                return value;  // Return previously computed value if available
            }
            else
            {
                // Determine if the candlestick is Neutral by checking if the body is small relative to the range
                bool neutral = scs.bodyRange < (scs.range * 0.03m);
                scs.Dictionary_Pattern.Add(Pattern_Name, neutral);  // Store the result in the dictionary
                return neutral;  // Return the result
            }
        }
    }
}
