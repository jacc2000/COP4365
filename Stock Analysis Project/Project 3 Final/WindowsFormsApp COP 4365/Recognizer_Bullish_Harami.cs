using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp_COP_4365
{
    /// <summary>
    /// A recognizer for bullish harami candlestick patterns, typically indicating a potential reversal to an uptrend.
    /// </summary>
    internal class Recognizer_Bullish_Harami : Recognizer
    {
        /// <summary>
        /// Initializes a new instance of the Recognizer_Bullish_Harami class.
        /// </summary>
        public Recognizer_Bullish_Harami() : base("Bullish Harami", 2)
        {
        }

        /// <summary>
        /// Identifies if a bullish harami pattern exists at the specified index in the list of candlesticks.
        /// </summary>
        /// <param name="scsList">List of SmartCandlestick objects.</param>
        /// <param name="index">Index of the candlestick to evaluate.</param>
        /// <returns>True if a bullish harami pattern is detected; otherwise, false.</returns>
        public override bool Recognize(List<SmartCandlestick> scsList, int index)
        {
            SmartCandlestick scs = scsList[index];
            // Attempt to retrieve the existing pattern value from the dictionary
            if (scs.Dictionary_Pattern.TryGetValue(Pattern_Name, out bool value))
            {
                return value; // Return the previously calculated value
            }
            else
            {
                if (index < Pattern_Length / 2) // Check for out of bounds error
                {
                    scs.Dictionary_Pattern.Add(Pattern_Name, false); // Add false if not enough data
                    return false;
                }
                else
                {
                    // Check the previous candlestick to see if it is "hugged" by the current smaller one
                    SmartCandlestick prev = scsList[index - Pattern_Length / 2];
                    // Determine if the current candle is smaller and inside the previous one
                    bool bullish = (prev.open > prev.close) && (scs.close > scs.open);
                    bool harami = (scs.topPrice < prev.topPrice) && (scs.bottomPrice > prev.bottomPrice);
                    bool bullish_harami = bullish && harami;
                    scs.Dictionary_Pattern.Add(Pattern_Name, bullish_harami); // Store the result
                    return bullish_harami; // Return the calculation result
                }
            }
        }
    }
}

