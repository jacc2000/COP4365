using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace WindowsFormsApp_COP_4365
{
    /// <summary>
    /// Recognizer for identifying Marubozu candlestick patterns.
    /// </summary>
    internal class Recognizer_Marubozu : Recognizer
    {
        /// <summary>
        /// Initializes a new instance of the Recognizer_Marubozu class.
        /// </summary>
        public Recognizer_Marubozu() : base("Marubozu", 1)
        {
        }

        /// <summary>
        /// Checks if the candlestick at the specified index is a Marubozu pattern.
        /// </summary>
        /// <param name="scsList">List of SmartCandlestick objects.</param>
        /// <param name="index">Index of the candlestick to check.</param>
        /// <returns>True if Marubozu pattern is found; otherwise, false.</returns>
        public override bool Recognize(List<SmartCandlestick> scsList, int index)
        {
            SmartCandlestick scs = scsList[index];
            if (scs.Dictionary_Pattern.TryGetValue(Pattern_Name, out bool value))
            {
                return value;  // Return previously computed value if available
            }
            else
            {
                // Determine if the candlestick is a Marubozu by checking if the body range is nearly the entire range
                bool marubozu = scs.bodyRange > (scs.range * 0.96m);
                scs.Dictionary_Pattern.Add(Pattern_Name, marubozu);  // Store the result in the dictionary
                return marubozu;  // Return the result
            }
        }
    }
}
