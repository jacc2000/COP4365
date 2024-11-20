using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp_COP_4365
{
    /// <summary>
    /// A recognizer for bullish engulfing candlestick patterns, often indicative of a reversal to an uptrend.
    /// </summary>
    internal class Recognizer_Bullish_Engulfing : Recognizer
    {
        /// <summary>
        /// Initializes a new instance of the Recognizer_Bullish_Engulfing class.
        /// </summary>
        public Recognizer_Bullish_Engulfing() : base("Bullish Engulfing", 2)
        {
        }

        /// <summary>
        /// Identifies if a bullish engulfing pattern exists at the specified index in the list of candlesticks.
        /// </summary>
        /// <param name="scsList">List of SmartCandlestick objects.</param>
        /// <param name="index">Index of the candlestick to evaluate.</param>
        /// <returns>True if a bullish engulfing pattern is detected; otherwise, false.</returns>
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
                    // Check the previous candlestick to see if it is engulfed by the current one
                    SmartCandlestick prev = scsList[index - Pattern_Length / 2];
                    // Determine if the previous candle is smaller and the current one engulfs it
                    bool bullish = (prev.open > prev.close) && (scs.close > scs.open);
                    bool engulfing = (scs.topPrice > prev.topPrice) && (scs.bottomPrice < prev.bottomPrice);
                    bool bullish_engulfing = bullish && engulfing;
                    scs.Dictionary_Pattern.Add(Pattern_Name, bullish_engulfing); // Store the result
                    return bullish_engulfing; // Return the calculation result
                }
            }
        }
    }
}
