using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp_COP_4365
{
    /// <summary>
    /// Recognizes Doji candlestick patterns, which typically indicate indecision in the market.
    /// </summary>
    internal class Recognizer_Doji : Recognizer
    {
        /// <summary>
        /// Initializes a new instance of the Recognizer_Doji class.
        /// </summary>
        public Recognizer_Doji() : base("Doji", 1)
        {
        }

        /// <summary>
        /// Evaluates whether a specified candlestick at a given index is a Doji.
        /// </summary>
        /// <param name="scsList">List of SmartCandlestick objects.</param>
        /// <param name="index">Index of the candlestick to evaluate.</param>
        /// <returns>True if the candlestick is a Doji; otherwise, false.</returns>
        public override bool Recognize(List<SmartCandlestick> scsList, int index)
        {
            SmartCandlestick scs = scsList[index];
            if (scs.Dictionary_Pattern.TryGetValue(Pattern_Name, out bool value))
            {
                return value;  // Return the previously calculated value if already computed
            }
            else
            {
                // Determine if the body range is very small relative to the total range
                bool doji = scs.bodyRange < (scs.range * 0.03m);
                scs.Dictionary_Pattern.Add(Pattern_Name, doji);  // Store the result
                return doji;  // Return the result
            }
        }
    }
}
