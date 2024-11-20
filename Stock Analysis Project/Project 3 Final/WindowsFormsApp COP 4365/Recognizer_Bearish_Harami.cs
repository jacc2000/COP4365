using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp_COP_4365
{
    /// <summary>
    /// Recognizes the bearish harami pattern, which is a reversal pattern typically found at the end of an uptrend.
    /// </summary>
    internal class Recognizer_Bearish_Harami : Recognizer
    {
        /// <summary>
        /// Constructor initializes the Recognizer with the pattern name "Bearish Harami" and length 2.
        /// </summary>
        public Recognizer_Bearish_Harami() : base("Bearish Harami", 2)
        {
        }

        /// <summary>
        /// Determines if a bearish harami pattern exists at a specific index in the list of SmartCandlesticks.
        /// </summary>
        /// <param name="scsList">List of SmartCandlesticks.</param>
        /// <param name="index">Index to check for the pattern.</param>
        /// <returns>True if the pattern is found, otherwise false.</returns>
        public override bool Recognize(List<SmartCandlestick> scsList, int index)
        {
            SmartCandlestick scs = scsList[index];
            if (scs.Dictionary_Pattern.TryGetValue(Pattern_Name, out bool value))
            {
                return value; // Return existing pattern value if already calculated.
            }
            else
            {
                if (index < 1) // Ensure the index allows checking the previous candlestick.
                {
                    scs.Dictionary_Pattern.Add(Pattern_Name, false);
                    return false; // Return false if the pattern cannot exist due to index constraints.
                }
                SmartCandlestick prev = scsList[index - 1];
                bool bearish = (prev.open < prev.close) && (scs.close < scs.open); // Previous candlestick is bullish and current is bearish.
                bool harami = (scs.topPrice < prev.topPrice) && (scs.bottomPrice > prev.bottomPrice); // Current candlestick is contained within the previous one.
                bool bearish_harami = bearish && harami; // Combine conditions for the pattern.
                scs.Dictionary_Pattern.Add(Pattern_Name, bearish_harami); // Add result to dictionary.
                return bearish_harami; // Return the computed pattern result.
            }
        }
    }
}
