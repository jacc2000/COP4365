using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace WindowsFormsApp_COP_4365
{
    /// <summary>
    /// Recognizer for identifying Hammer candlestick patterns.
    /// </summary>
    internal class Recognizer_Hammer : Recognizer
    {
        /// <summary>
        /// Initializes a new instance of the Recognizer_Hammer class.
        /// </summary>
        public Recognizer_Hammer() : base("Hammer", 1)
        {
        }

        /// <summary>
        /// Checks if the candlestick at the specified index is a Hammer pattern.
        /// </summary>
        /// <param name="scsList">List of SmartCandlestick objects.</param>
        /// <param name="index">Index of the candlestick to check.</param>
        /// <returns>True if Hammer pattern is found; otherwise, false.</returns>
        public override bool Recognize(List<SmartCandlestick> scsList, int index)
        {
            SmartCandlestick scs = scsList[index];
            if (scs.Dictionary_Pattern.TryGetValue(Pattern_Name, out bool value))
            {
                return value;  // Return previously computed value if available
            }
            else
            {
                // Determine if the candlestick is a Hammer by checking the body and tail proportions
                bool hammer = ((scs.range * 0.20m) < scs.bodyRange) && (scs.bodyRange < (scs.range * 0.33m)) && (scs.lowerTail > scs.range * 0.66m);
                scs.Dictionary_Pattern.Add(Pattern_Name, hammer);  // Store the result in the dictionary
                return hammer;  // Return the result
            }
        }
    }
}
