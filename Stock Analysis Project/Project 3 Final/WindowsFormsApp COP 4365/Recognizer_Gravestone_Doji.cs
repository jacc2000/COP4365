using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp_COP_4365
{
    /// <summary>
    /// Recognizes Gravestone Doji candlestick patterns, often seen as indicators of a potential bearish reversal.
    /// </summary>
    internal class Recognizer_Gravestone_Doji : Recognizer
    {
        /// <summary>
        /// Initializes a new instance of the Recognizer_Gravestone_Doji class.
        /// </summary>
        public Recognizer_Gravestone_Doji() : base("Gravestone Doji", 1)
        {
        }

        /// <summary>
        /// Evaluates whether a specified candlestick at a given index is a Gravestone Doji.
        /// </summary>
        /// <param name="scsList">List of SmartCandlestick objects.</param>
        /// <param name="index">Index of the candlestick to evaluate.</param>
        /// <returns>True if the candlestick is a Gravestone Doji; otherwise, false.</returns>
        public override bool Recognize(List<SmartCandlestick> scsList, int index)
        {
            SmartCandlestick scs = scsList[index];
            if (scs.Dictionary_Pattern.TryGetValue(Pattern_Name, out bool value))
            {
                return value;  // Return the previously calculated value if already computed
            }
            else
            {
                // Check for a long upper shadow and a small body
                bool gravestone = scs.upperTail > (scs.range * 0.66m);
                bool doji = scs.bodyRange < (scs.range * 0.03m);
                bool gravestone_doji = gravestone && doji;
                scs.Dictionary_Pattern.Add(Pattern_Name, gravestone_doji);  // Store the result
                return gravestone_doji;  // Return the result
            }
        }
    }
}
