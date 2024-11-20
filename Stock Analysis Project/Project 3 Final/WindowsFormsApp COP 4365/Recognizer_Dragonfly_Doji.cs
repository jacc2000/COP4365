using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp_COP_4365
{
    /// <summary>
    /// Recognizes Dragonfly Doji candlestick patterns, which may indicate potential reversals or strong support.
    /// </summary>
    internal class Recognizer_Dragonfly_Doji : Recognizer
    {
        /// <summary>
        /// Initializes a new instance of the Recognizer_Dragonfly_Doji class.
        /// </summary>
        public Recognizer_Dragonfly_Doji() : base("Dragonfly Doji", 1)
        {
        }

        /// <summary>
        /// Evaluates whether a specified candlestick at a given index is a Dragonfly Doji.
        /// </summary>
        /// <param name="scsList">List of SmartCandlestick objects.</param>
        /// <param name="index">Index of the candlestick to evaluate.</param>
        /// <returns>True if the candlestick is a Dragonfly Doji; otherwise, false.</returns>
        public override bool Recognize(List<SmartCandlestick> scsList, int index)
        {
            SmartCandlestick scs = scsList[index];
            if (scs.Dictionary_Pattern.TryGetValue(Pattern_Name, out bool value))
            {
                return value;  // Return the previously calculated value if already computed
            }
            else
            {
                // Check for a long lower shadow and a small body
                bool dragonfly = scs.lowerTail > (scs.range * 0.66m);
                bool doji = scs.bodyRange < (scs.range * 0.03m);
                bool dragonfly_doji = dragonfly && doji;
                scs.Dictionary_Pattern.Add(Pattern_Name, dragonfly_doji);  // Store the result
                return dragonfly_doji;  // Return the result
            }
        }
    }
}
