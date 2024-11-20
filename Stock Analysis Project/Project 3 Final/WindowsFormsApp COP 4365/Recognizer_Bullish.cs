using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp_COP_4365
{
    /// <summary>
    /// A recognizer for bullish candlestick patterns, indicating a potential price increase.
    /// </summary>
    internal class Recognizer_Bullish : Recognizer
    {
        /// <summary>
        /// Initializes a new instance of the Recognizer_Bullish class.
        /// </summary>
        public Recognizer_Bullish() : base("Bullish", 1)
        {
        }

        /// <summary>
        /// Identifies if the candlestick at the specified index is bullish.
        /// </summary>
        /// <param name="scsList">List of SmartCandlestick objects.</param>
        /// <param name="index">Index of the candlestick to evaluate.</param>
        /// <returns>True if the candlestick is bullish; otherwise, false.</returns>
        public override bool Recognize(List<SmartCandlestick> scsList, int index)
        {
            // Attempt to retrieve the existing pattern value from the dictionary
            SmartCandlestick scs = scsList[index];
            if (scs.Dictionary_Pattern.TryGetValue(Pattern_Name, out bool value))
            {
                return value; // Return the previously calculated value
            }
            else
            {
                // Calculate if the close price is greater than the open price
                bool bullish = scs.close > scs.open;
                scs.Dictionary_Pattern.Add(Pattern_Name, bullish); // Store the result
                return bullish; // Return the calculation result
            }
        }
    }
}
